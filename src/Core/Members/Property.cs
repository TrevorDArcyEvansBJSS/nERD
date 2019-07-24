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

namespace NClass.Core
{
  public abstract class Property : Operation
  {
    /// <exception cref="BadSyntaxException">
    /// The <paramref name="name"/> does not fit to the syntax.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The language of <paramref name="parent"/> does not equal.
    /// </exception>
    protected Property(string name, CompositeType parent) :
      base(name, parent)
    {
    }

    public sealed override MemberType MemberType
    {
      get { return MemberType.Property; }
    }

    public override bool HasBody
    {
      get { return true; }
    }

    public bool HasImplementation
    {
      get
      {
        return (!IsAbstract && !(Parent is InterfaceType));
      }
    }

    private bool _isReadonly = false;
    public bool IsReadonly
    {
      get
      {
        return _isReadonly;
      }
      set
      {
        if (_isReadonly != value)
        {
          if (value)
            _isWriteonly = false;
          OnBeginUndoableOperation();
          _isReadonly = value;
          Changed();
        }
      }
    }

    private bool _isWriteonly = false;
    public bool IsWriteonly
    {
      get
      {
        return _isWriteonly;
      }
      set
      {
        if (_isWriteonly != value)
        {
          if (value)
            _isReadonly = false;
          OnBeginUndoableOperation();
          _isWriteonly = value;
          Changed();
        }
      }
    }

    private AccessModifier _readAccess = AccessModifier.Default;
    /// <exception cref="BadSyntaxException">
    /// Cannot set accessor modifier.
    /// </exception>
    public AccessModifier ReadAccess
    {
      get
      {
        return _readAccess;
      }
      protected set
      {
        if (value == _readAccess)
          return;

        if (value == AccessModifier.Default || (value != Access &&
          WriteAccess == AccessModifier.Default && !IsReadonly && !IsWriteonly))
        {
          OnBeginUndoableOperation();
          _readAccess = value;
          Changed();
        }
        else
        {
          throw new BadSyntaxException(Strings.ErrorAccessorModifier);
        }
      }
    }

    private AccessModifier _writeAccess = AccessModifier.Default;
    /// <exception cref="BadSyntaxException">
    /// Cannot set accessor modifier.
    /// </exception>
    public AccessModifier WriteAccess
    {
      get
      {
        return _writeAccess;
      }
      protected set
      {
        if (value == _writeAccess)
          return;

        if (value == AccessModifier.Default || (value != Access &&
          ReadAccess == AccessModifier.Default && !IsReadonly && !IsWriteonly))
        {
          OnBeginUndoableOperation();
          _writeAccess = value;
          Changed();
        }
        else
        {
          throw new BadSyntaxException(Strings.ErrorAccessorModifier);
        }
      }
    }

    protected override void CopyFrom(Member member)
    {
      base.CopyFrom(member);

      Property property = (Property)member;
      _isReadonly = property._isReadonly;
      _isWriteonly = property._isWriteonly;
      _readAccess = property._readAccess;
      _writeAccess = property._writeAccess;
    }
  }
}
