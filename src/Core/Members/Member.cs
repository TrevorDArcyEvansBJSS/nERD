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

using System;
using NClass.Translations;

namespace NClass.Core
{
  public abstract class Member : LanguageElement
  {
    /// <exception cref="BadSyntaxException">
    /// The <paramref name="name"/> does not fit to the syntax.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The language of <paramref name="parent"/> does not equal.
    /// </exception>
    protected Member(string name, CompositeType parent)
    {
      if (parent == null)
        throw new ArgumentNullException("parent");
      if (parent.Language != this.Language)
        throw new ArgumentException(Strings.ErrorLanguagesDoNotEqual);

      Initializing = true;
      Parent = parent;
      Name = name;
      ValidType = DefaultType;
      Initializing = false;
    }

    public abstract MemberType MemberType
    {
      get;
    }

    private CompositeType _parent;
    public CompositeType Parent
    {
      get
      {
        return _parent;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException("value");

        if (_parent != value)
        {
          OnBeginUndoableOperation();
          _parent = value;
          Changed();
        }
      }
    }

    private string _name;
    /// <exception cref="BadSyntaxException">
    /// The <paramref name="value"/> does not fit to the syntax.
    /// </exception>
    public override string Name
    {
      get
      {
        return _name;
      }
      set
      {

        string newName = Language.GetValidName(value, true);

        if (newName != _name)
        {
          OnBeginUndoableOperation();
          _name = newName;
          Changed();
        }
      }
    }

    protected string ValidName
    {
      set
      {
        if (_name != value)
        {
          OnBeginUndoableOperation();
          _name = value;
          Changed();
        }
      }
    }

    public virtual bool IsNameReadonly
    {
      get { return false; }
    }

    private string _type;
    /// <exception cref="BadSyntaxException">
    /// The <paramref name="value"/> does not fit to the syntax.
    /// </exception>
    public virtual string Type
    {
      get
      {
        return _type;
      }
      set
      {
        string newType = Language.GetValidTypeName(value);

        if (newType != _type)
        {
          OnBeginUndoableOperation();
          _type = newType;
          Changed();
        }
      }
    }

    protected string ValidType
    {
      set
      {
        if (_type != value)
        {
          OnBeginUndoableOperation();
          _type = value;
          Changed();
        }
      }
    }

    public virtual bool IsTypeReadonly
    {
      get { return false; }
    }

    protected abstract string DefaultType
    {
      get;
    }

    private AccessModifier _accessModifier = AccessModifier.Default;
    /// <exception cref="BadSyntaxException">
    /// Cannot set access visibility.
    /// </exception>
    public virtual AccessModifier AccessModifier
    {
      get
      {
        return _accessModifier;
      }
      set
      {
        if (!Language.IsValidModifier(value))
          throw new BadSyntaxException(Strings.ErrorInvalidModifier);

        if (_accessModifier != value)
        {
          OnBeginUndoableOperation();
          _accessModifier = value;
          Changed();
        }
      }
    }

    public virtual AccessModifier DefaultAccess
    {
      get { return Parent.DefaultMemberAccess; }
    }

    public AccessModifier Access
    {
      get
      {
        if (AccessModifier == AccessModifier.Default)
          return DefaultAccess;
        else
          return AccessModifier;
      }
    }

    public virtual bool IsAccessModifiable
    {
      get { return true; }
    }

    public abstract bool IsModifierless
    {
      get;
    }

    /// <exception cref="BadSyntaxException">
    /// Cannot set static modifier.
    /// </exception>
    public abstract bool IsStatic
    {
      get;
      set;
    }

    /// <exception cref="BadSyntaxException">
    /// Cannot set hider modifier.
    /// </exception>
    public abstract bool IsHider
    {
      get;
      set;
    }

    public abstract Language Language
    {
      get;
    }

    public string GetUmlDescription()
    {
      return GetUmlDescription(true, true, true, true);
    }

    public string GetUmlDescription(bool getType, bool getParameters, bool getParameterNames)
    {
      return GetUmlDescription(getType, getParameters, getParameterNames, getType);
    }

    public abstract string GetUmlDescription(bool getType, bool getParameters, bool getParameterNames, bool getInitValue);

    /// <exception cref="BadSyntaxException">
    /// The <paramref name="declaration"/> does not fit to the syntax.
    /// </exception>
    public abstract void InitFromString(string declaration);

    protected virtual void CopyFrom(Member member)
    {
      _name = member._name;
      _type = member._type;
      _accessModifier = member._accessModifier;
    }
  }
}
