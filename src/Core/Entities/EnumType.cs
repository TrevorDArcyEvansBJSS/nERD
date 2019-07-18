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
using System.Collections.Generic;
using System.Xml;

namespace NClass.Core
{
  public abstract class EnumType : TypeBase
  {
    private readonly List<EnumValue> _values = new List<EnumValue>();

    /// <exception cref="BadSyntaxException">
    /// The <paramref name="name"/> does not fit to the syntax.
    /// </exception>
    protected EnumType(string name) : base(name)
    {
    }

    public sealed override EntityType EntityType
    {
      get { return EntityType.Enum; }
    }

    public IEnumerable<EnumValue> Values
    {
      get { return _values; }
    }

    public int ValueCount
    {
      get { return _values.Count; }
    }

    public sealed override string Signature
    {
      get
      {
        return (Language.GetAccessString(Access, false) + " Enum");
      }
    }

    public override string Stereotype
    {
      get { return "«enumeration»"; }
    }

    /// <exception cref="BadSyntaxException">
    /// The name does not fit to the syntax.
    /// </exception>
    /// <exception cref="ReservedNameException">
    /// The name is a reserved name.
    /// </exception>
    public abstract EnumValue AddValue(string declaration);

    /// <exception cref="ReservedNameException">
    /// The name is a reserved name.
    /// </exception>
    protected void AddValue(EnumValue newValue)
    {
      if (newValue != null)
      {
        foreach (EnumValue value in Values)
        {
          if (value.Name == newValue.Name)
            throw new ReservedNameException(newValue.Name);
        }

        OnBeginUndoableOperation();
        _values.Add(newValue);
        newValue.BeginUndoableOperation += delegate { OnBeginUndoableOperation(); };
        newValue.Modified += delegate { Changed(); };
        Changed();
      }
    }

    public EnumValue GetValue(int index)
    {
      if (index >= 0 && index < _values.Count)
        return _values[index];
      else
        return null;
    }

    /// <exception cref="BadSyntaxException">
    /// The name does not fit to the syntax.
    /// </exception>
    /// <exception cref="ReservedNameException">
    /// The name is a reserved name.
    /// </exception>
    public abstract EnumValue ModifyValue(EnumValue value, string declaration);

    /// <exception cref="ReservedNameException">
    /// The new name is a reserved name.
    /// </exception>
    protected bool ChangeValue(EnumValue oldValue, EnumValue newValue)
    {
      if (oldValue == null || newValue == null)
        return false;

      int index = -1;
      for (int i = 0; i < _values.Count; i++)
      {
        if (_values[i] == oldValue)
          index = i;
        else if (_values[i].Name == newValue.Name)
          throw new ReservedNameException(newValue.Name);
      }

      if (index == -1)
      {
        return false;
      }
      else
      {
        OnBeginUndoableOperation();
        _values[index] = newValue;
        Changed();
        return true;
      }
    }

    public void RemoveValue(EnumValue value)
    {
      OnBeginUndoableOperation();
      if (_values.Remove(value))
        Changed();
    }

    public override bool MoveUpItem(object item)
    {
      if (item is EnumValue)
      {
        OnBeginUndoableOperation();
      }

      if (item is EnumValue && MoveUp(_values, item))
      {
        Changed();
        return true;
      }
      else
      {
        return false;
      }
    }

    public override bool MoveDownItem(object item)
    {
      if (item is EnumValue)
      {
        OnBeginUndoableOperation();
      }

      if (item is EnumValue && MoveDown(_values, item))
      {
        Changed();
        return true;
      }
      else
      {
        return false;
      }
    }

    protected override void CopyFrom(TypeBase type)
    {
      base.CopyFrom(type);

      EnumType enumType = (EnumType)type;
      _values.Clear();
      _values.Capacity = enumType._values.Capacity;
      foreach (EnumValue value in enumType._values)
      {
        _values.Add(value.Clone());
      }
    }

    public abstract EnumType Clone();

    protected internal override void Serialize(XmlElement node)
    {
      base.Serialize(node);

      foreach (EnumValue value in _values)
      {
        XmlElement child = node.OwnerDocument.CreateElement("Value");
        child.InnerText = value.ToString();
        node.AppendChild(child);
      }
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

      XmlNodeList nodeList = node.SelectNodes("Value");
      foreach (XmlNode valueNode in nodeList)
        AddValue(valueNode.InnerText);

      base.Deserialize(node);
      RaisePreChangedEvent = RaiseChangedEvent = true;
    }
  }
}
