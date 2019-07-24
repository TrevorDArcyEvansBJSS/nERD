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

using NClass.Translations;
using System;
using System.Collections.Generic;
using System.Xml;

namespace NClass.Core
{
  public abstract class ClassType : SingleInheritanceType
  {
    private int _derivedClassCount = 0;

    /// <exception cref="BadSyntaxException">
    /// The <paramref name="name"/> does not fit to the syntax.
    /// </exception>
    protected ClassType(string name) :
      base(name)
    {
    }

    public sealed override EntityType EntityType
    {
      get { return EntityType.Class; }
    }

    private ClassModifier _modifier = ClassModifier.None;
    /// <exception cref="BadSyntaxException">
    /// The <paramref name="value"/> does not fit to the syntax.
    /// </exception>
    public virtual ClassModifier Modifier
    {
      get
      {
        return _modifier;
      }
      set
      {
        if (_modifier != value)
        {
          if (value == ClassModifier.Static && (IsSuperClass || HasExplicitBase))
            throw new BadSyntaxException(Strings.ErrorInvalidModifier);
          if (value == ClassModifier.Sealed && IsSuperClass)
            throw new BadSyntaxException(Strings.ErrorInvalidModifier);

          OnBeginUndoableOperation();
          _modifier = value;
          Changed();
        }
      }
    }

    public override bool SupportsFields
    {
      get { return true; }
    }

    public override bool SupportsMethods
    {
      get { return true; }
    }

    public override bool SupportsConstuctors
    {
      get { return true; }
    }

    public override bool SupportsNesting
    {
      get { return true; }
    }

    public override bool IsAllowedParent
    {
      get
      {
        return (
          Modifier != ClassModifier.Sealed &&
          Modifier != ClassModifier.Static
        );
      }
    }

    public override bool IsAllowedChild
    {
      get
      {
        return (Modifier != ClassModifier.Static);
      }
    }

    public override bool HasExplicitBase
    {
      get
      {
        return (_baseClass != null);
      }
    }

    public bool IsSuperClass
    {
      get { return (_derivedClassCount > 0); }
    }

    public sealed override string Signature
    {
      get
      {
        string accessString = Language.GetAccessString(Access, false);
        string modifierString = Language.GetClassModifierString(Modifier, false);

        if (Modifier == ClassModifier.None)
          return string.Format("{0} Class", accessString);
        else
          return string.Format("{0} {1} Class", accessString, modifierString);
      }
    }

    public override string Stereotype
    {
      get { return null; }
    }

    /// <exception cref="RelationshipException">
    /// The base and derived types do not equal.-or-
    /// The <paramref name="value"/> is descendant of the type.
    /// </exception>
    public override SingleInheritanceType Base
    {
      get
      {
        return BaseClass;
      }
      set
      {
        if (value != null && !(value is ClassType))
          throw new RelationshipException(Strings.ErrorInvalidBaseType);

        BaseClass = (ClassType)value;
      }
    }

    private ClassType _baseClass = null;
    /// <exception cref="RelationshipException">
    /// The language of <paramref name="value"/> does not equal.-or-
    /// <paramref name="value"/> is static or sealed class.-or-
    /// The <paramref name="value"/> is descendant of the class.
    /// </exception>
    public virtual ClassType BaseClass
    {
      get
      {
        return _baseClass;
      }
      set
      {
        if (value == _baseClass)
          return;

        if (value == null)
        {
          OnBeginUndoableOperation();
          _baseClass._derivedClassCount--;
          _baseClass = null;
          Changed();
          return;
        }

        if (value == this)
          throw new RelationshipException(Strings.ErrorInvalidBaseType);

        if (value.Modifier == ClassModifier.Sealed ||
          value.Modifier == ClassModifier.Static)
        {
          throw new RelationshipException(Strings.ErrorCannotInherit);
        }
        if (value.IsAncestor(this))
        {
          throw new RelationshipException(string.Format(Strings.ErrorCyclicBase,
            Strings.Class));
        }
        if (value.Language != Language)
          throw new RelationshipException(Strings.ErrorLanguagesDoNotEqual);

        OnBeginUndoableOperation();
        _baseClass = value;
        _baseClass._derivedClassCount++;
        Changed();
      }
    }

    public override IEnumerable<Operation> OverridableOperations
    {
      get
      {
        for (int i = 0; i < OperationList.Count; i++)
        {
          if (OperationList[i].Overridable)
            yield return OperationList[i];
        }
      }
    }

    private bool IsAncestor(ClassType classType)
    {
      if (BaseClass != null && BaseClass.IsAncestor(classType))
        return true;
      else
        return (classType == this);
    }

    protected override void CopyFrom(TypeBase type)
    {
      base.CopyFrom(type);
      ClassType classType = (ClassType)type;
      _modifier = classType._modifier;
    }

    public abstract ClassType Clone();

    protected internal override void Serialize(XmlElement node)
    {
      base.Serialize(node);

      XmlElement child = node.OwnerDocument.CreateElement("Modifier");
      child.InnerText = Modifier.ToString();
      node.AppendChild(child);
    }

    /// <exception cref="BadSyntaxException">
    /// An error occured while deserializing.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The XML document is corrupt.
    /// </exception>
    protected internal override void Deserialize(XmlElement node)
    {
      RaisePreChangedEvent = RaiseChangedEvent = false;

      XmlElement child = node["Modifier"];
      if (child != null)
        Modifier = Language.TryParseClassModifier(child.InnerText);

      base.Deserialize(node);
      RaiseChangedEvent = RaisePreChangedEvent = true;
    }
  }
}
