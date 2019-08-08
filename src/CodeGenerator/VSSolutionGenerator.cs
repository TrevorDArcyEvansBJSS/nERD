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
using NClass.EntityRelationshipDiagram;
using System;
using System.IO;
using System.Windows.Forms;

namespace NClass.CodeGenerator
{
  internal sealed class VSSolutionGenerator : SolutionGenerator
  {
    public VSSolutionGenerator(Project project, SolutionType version) :
      base(project)
    {
      Version = version;
    }

    private SolutionType _version = SolutionType.VisualStudio2019;
    public SolutionType Version
    {
      get
      {
        return _version;
      }
      set
      {
        if (value == SolutionType.VisualStudio2017 ||
          value == SolutionType.VisualStudio2019)
        {
          _version = value;
        }
        else
        {
          throw new ArgumentOutOfRangeException($"Unknown version of Visual Studio:  {Version}");
        }
      }
    }

    private string VersionString
    {
      get
      {
        if (Version == SolutionType.VisualStudio2017)
        {
          return "Visual Studio 2017";
        }

        if (Version == SolutionType.VisualStudio2019)
        {
          return "Visual Studio 2019";
        }

        throw new ArgumentOutOfRangeException($"Unknown version of Visual Studio:  {Version}");
      }
    }

    /// <exception cref="ArgumentException">
    /// The <paramref name="model"/> has invalid language.
    /// </exception>
    protected override ProjectGenerator CreateProjectGenerator(Model model)
    {
      Language language = model.Language;

      if (language == CSharpLanguage.Instance)
      {
        return new CSharpProjectGenerator(model);
      }

      if (language == ErdLanguage.Instance)
      {
        return new SqlProjectGenerator(model);
      }

      throw new ArgumentException($"The model has an unknown language:  {language.Name}");
    }

    protected override bool GenerateSolutionFile(string location)
    {
      try
      {
        var templateDir = Path.Combine(Application.StartupPath, "Templates");
        var templatePath = Path.Combine(templateDir, "sln.template");
        var solutionDir = Path.Combine(location, SolutionName);
        var solutionPath = Path.Combine(solutionDir, SolutionName + ".sln");

        using (StreamReader reader = new StreamReader(templatePath))
        {
          using (StreamWriter writer = new StreamWriter(solutionPath, false, reader.CurrentEncoding))
          {
            while (!reader.EndOfStream)
            {
              CopyLine(reader, writer);
            }
          }
        }

        return true;
      }
      catch
      {
        return false;
      }
    }

    private void CopyLine(StreamReader reader, StreamWriter writer)
    {
      var line = reader.ReadLine();

      line = line.Replace("${VersionString}", VersionString);

      if (line.Contains("${ProjectFile}"))
      {
        var nextLine = reader.ReadLine();
        foreach (ProjectGenerator generator in ProjectGenerators)
        {
          var newLine = line.Replace("${ProjectFile}", generator.RelativeProjectFileName);
          newLine = newLine.Replace("${ProjectName}", generator.ProjectName);

          writer.WriteLine(newLine);
          writer.WriteLine(nextLine);
        }
      }
      else
      {
        writer.WriteLine(line);
      }
    }
  }
}
