// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// 
// This program is free software; you can redistribute it and/or modify it under 
// the terms of the GNU General Public License as published by the Free Software 
// Foundation; either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with 
// this program; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using NClass.Core;
using NClass.CSharp;
using NClass.Translations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NClass.CodeGenerator
{
  internal sealed class SqlProjectGenerator : ProjectGenerator
  {
    public SqlProjectGenerator(Model model) :
      base(model)
    {
    }

    public override string RelativeProjectFileName
    {
      get { return null; }
    }

    protected override SourceFileGenerator CreateSourceFileGenerator(TypeBase type)
    {
      return null;
    }

    protected override bool GenerateProjectFiles(string location)
    {
      var sb = new StringBuilder();
      try
      {
        var entities = Model.Entities.OfType<CSharpClass>();
        var links = Model.Relationships.OfType<EntityRelationship>();

        // check for any unsupported .NET types
        var fieldTypeNames = entities.SelectMany(ent => ent.Fields.OfType<CSharpField>()).Select(field => field.Type);
        var propTypeNames = entities.SelectMany(ent => ent.Operations.OfType<CSharpProperty>()).Select(op => op.Type);
        if (fieldTypeNames.Any(fieldTypeName => !NetToSqlTypeMap.TryGetValue(fieldTypeName.ToLowerInvariant(), out _)) ||
          propTypeNames.Any(propTypeName => !NetToSqlTypeMap.TryGetValue(propTypeName.ToLowerInvariant(), out _)))
        {
          sb.AppendLine($"-- {Strings.SqlGenError_UnsupportedType}");
          return false;
        }

        // check for loop relationships - unsupported as no reliable way to determine foreign key
        if (links.Any(link => link.First.Id == link.Second.Id))
        {
          sb.AppendLine($"-- {Strings.SqlGenError_LoopRelationship}");
          return false;
        }

        // check for two links between two entities - BAD
        foreach (var link in links)
        {
          var otherLinks = links.Except(new[] { link });
          if (otherLinks.Any(otherLink =>
            (otherLink.First.Id == link.First.Id && otherLink.Second.Id == link.Second.Id) ||
            (otherLink.First.Id == link.Second.Id && otherLink.Second.Id == link.First.Id)))
          {
            sb.AppendLine($"-- {Strings.SqlGenError_TwoLinks}");
            return false;
          }
        }


        // create all entities
        foreach (var entity in entities)
        {
          WriteTable(sb, entity);
        }
        sb.AppendLine();

        // write primary key
        foreach (var entity in entities)
        {
          WritePrimaryKey(sb, entity);
        }
        sb.AppendLine();

        // create all links aka foreign keys
        foreach (var link in links)
        {
          WriteForeignKey(sb, link);
        }
        sb.AppendLine();

        return true;
      }
      finally
      {
        var fileName = Path.ChangeExtension(Model.Name, ".sql");
        var filePath = Path.Combine(location, fileName);
        File.WriteAllText(filePath, sb.ToString());
      }
    }

    private void WriteTable(StringBuilder sb, CSharpClass type)
    {
      sb.AppendLine($"CREATE TABLE {type.Name}");
      sb.AppendLine($"(");

      foreach (var field in type.Fields.OfType<CSharpField>())
      {
        sb.AppendLine($"  {field.Name} {NetToSqlTypeMap[field.Type.ToLowerInvariant()]},");
      }

      foreach (var op in type.Operations.OfType<CSharpProperty>())
      {
        sb.AppendLine($"  {op.Name} {NetToSqlTypeMap[op.Type.ToLowerInvariant()]},");
      }

      sb.AppendLine($");");
      sb.AppendLine();
    }

    private void WritePrimaryKey(StringBuilder sb, CSharpClass type)
    {
      var pk = GetPrimaryKeyMember(type);
      if (pk != null)
      {
        sb.AppendLine($"ALTER TABLE {type.Name} ADD PRIMARY KEY({pk.Name});");
      }
    }

    private void WriteForeignKey(StringBuilder sb, EntityRelationship link)
    {
      // create link tables
      if ((link.StartMultiplicity == MultiplicityType.ZeroOrMany || link.StartMultiplicity == MultiplicityType.OneOrMany) &&
        (link.EndMultiplicity == MultiplicityType.ZeroOrMany || link.EndMultiplicity == MultiplicityType.OneOrMany))
      {
        sb.AppendLine();
        sb.AppendLine($"-- generate link table: [{link.First.Name}] >+--+< [{link.Second.Name}]");
        WriteLinkTable(sb, link);
        sb.AppendLine();
        return;
      }

      // [First] --> [Second]
      var fk1 = GetForeignKeyMember((CSharpClass)link.Second, link.First.Name);
      if (fk1 != null)
      {
        sb.AppendLine($"ALTER TABLE {link.Second.Name} ADD FOREIGN KEY({fk1.Name}) REFERENCES {link.First.Name}({fk1.Name})");
      }

      // [Second] --> [First]
      var fk2 = GetForeignKeyMember((CSharpClass)link.First, link.Second.Name);
      if (fk2 != null)
      {
        sb.AppendLine($"ALTER TABLE {link.First.Name} ADD FOREIGN KEY({fk2.Name}) REFERENCES {link.Second.Name}({fk2.Name})");
      }
    }

    private void WriteLinkTable(StringBuilder sb, EntityRelationship link)
    {
      var linkTable = new CSharpClass
      {
        Name = $"{link.First.Name}_{link.Second.Name}"
      };
      var firstId = linkTable.AddProperty();
      firstId.Name = $"{link.First.Name}Id";
      firstId.Type = GetPrimaryKeyMember((CSharpClass)link.First).Type;

      var secondId = linkTable.AddProperty();
      secondId.Name = $"{link.Second.Name}Id";
      secondId.Type = GetPrimaryKeyMember((CSharpClass)link.Second).Type;

      WriteTable(sb, linkTable);

      sb.AppendLine($"ALTER TABLE {linkTable.Name} ADD FOREIGN KEY({firstId.Name}) REFERENCES {link.First.Name}({firstId.Name})");
      sb.AppendLine($"ALTER TABLE {linkTable.Name} ADD FOREIGN KEY({secondId.Name}) REFERENCES {link.Second.Name}({secondId.Name})");
    }

    private static Member GetPrimaryKeyMember(CSharpClass type)
    {
      var pk = GetForeignKeyMember(type, string.Empty);
      return pk;
    }

    private static Member GetForeignKeyMember(CSharpClass type, string otherTypeName)
    {
      // give preference to Property over Field
      var fk = type.Operations.OfType<CSharpProperty>().SingleOrDefault(op => op.Name.ToLowerInvariant() == $"{otherTypeName.ToLowerInvariant()}id") as Member ??
                type.Fields.OfType<CSharpField>().SingleOrDefault(field => field.Name.ToLowerInvariant() == $"{otherTypeName.ToLowerInvariant()}id");
      return fk;
    }

    // [.NET type (lowercase)] --> [MS SQL type]
    private readonly static Dictionary<string, string> NetToSqlTypeMap = new Dictionary<string, string>
    {
      { "int", "int" },
      { "string", "nvarchar(max)" },
      { "bool", "bit" },
      { "datetime", "datetime2" },
      { "decimal", "decimal" },
      { "float", "float" },
      { "double", "float" },
      { "guid", "uniqueidentifier" }
    };
  }
}
