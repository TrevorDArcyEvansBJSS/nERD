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
using System.IO;
using System.Xml;

namespace NClass.Core
{
  public abstract class CompositeType : TypeBase
  {
    /// <exception cref="BadSyntaxException">
    /// The <paramref name="name"/> does not fit to the syntax.
    /// </exception>
    protected CompositeType(string name) :
      base(name)
    {
    }

    public abstract bool SupportsFields { get; }

    public bool SupportsOperations
    {
      get
      {
        return
        (
          SupportsMethods || SupportsConstuctors || SupportsDestructors ||
          SupportsProperties || SupportsEvents
        );
      }
    }

    public abstract bool SupportsMethods { get; }

    public abstract bool SupportsConstuctors { get; }

    public abstract bool SupportsDestructors { get; }

    public abstract bool SupportsProperties { get; }

    public abstract bool SupportsEvents { get; }

    public abstract bool SupportsNesting { get; }

    public abstract bool IsAllowedParent { get; }

    public abstract bool IsAllowedChild { get; }

    public abstract bool HasExplicitBase { get; }

    public abstract AccessModifier DefaultMemberAccess { get; }

    protected List<Field> FieldList { get; } = new List<Field>();

    public IEnumerable<Field> Fields
    {
      get { return FieldList; }
    }

    public int FieldCount
    {
      get { return FieldList.Count; }
    }

    protected List<Operation> OperationList { get; } = new List<Operation>();

    public IEnumerable<Operation> Operations
    {
      get { return OperationList; }
    }

    public int OperationCount
    {
      get { return OperationList.Count; }
    }

    public int MemberCount
    {
      get
      {
        return FieldCount + OperationCount;
      }
    }

    private readonly List<TypeBase> _nestedChilds = new List<TypeBase>();
    public IEnumerable<TypeBase> NestedChilds
    {
      get { return _nestedChilds; }
    }

    /// <exception cref="InvalidOperationException">
    /// The type does not support fields.
    /// </exception>
    public abstract Field AddField();

    /// <exception cref="InvalidOperationException">
    /// The type does not support methods.
    /// </exception>
    public abstract Method AddMethod();

    /// <exception cref="InvalidOperationException">
    /// The type does not support constructors.
    /// </exception>
    public abstract Constructor AddConstructor();

    /// <exception cref="InvalidOperationException">
    /// The type does not support destructors.
    /// </exception>
    public abstract Destructor AddDestructor();

    /// <exception cref="InvalidOperationException">
    /// The type does not support properties.
    /// </exception>
    public abstract Property AddProperty();

    /// <exception cref="InvalidOperationException">
    /// The type does not support events.
    /// </exception>
    public abstract Event AddEvent();

    /// <exception cref="InvalidOperationException">
    /// The type does not support the given kind of member.
    /// </exception>
    public void InsertMember(MemberType type, int index)
    {
      if (type == MemberType.Field)
      {
        if (index > FieldCount)
          index = FieldCount;
      }
      else
      {
        index -= FieldCount;
        if (index > OperationCount)
          index = OperationCount;
      }

      if (index < 0)
        index = 0;

      switch (type)
      {
        case MemberType.Field:
          Field field = AddField();
          FieldList.RemoveAt(FieldCount - 1);
          FieldList.Insert(index, field);
          break;

        case MemberType.Method:
          Method method = AddMethod();
          OperationList.RemoveAt(OperationCount - 1);
          OperationList.Insert(index, method);
          break;

        case MemberType.Constructor:
          Constructor constructor = AddConstructor();
          OperationList.RemoveAt(OperationCount - 1);
          OperationList.Insert(index, constructor);
          break;

        case MemberType.Destructor:
          Destructor destructor = AddDestructor();
          OperationList.RemoveAt(OperationCount - 1);
          OperationList.Insert(index, destructor);
          break;

        case MemberType.Property:
          Property property = AddProperty();
          OperationList.RemoveAt(OperationCount - 1);
          OperationList.Insert(index, property);
          break;

        case MemberType.Event:
          Event _event = AddEvent();
          OperationList.RemoveAt(OperationCount - 1);
          OperationList.Insert(index, _event);
          break;
      }
    }

    protected void AddField(Field field)
    {
      if (field != null && !FieldList.Contains(field))
      {
        OnBeginUndoableOperation();
        FieldList.Add(field);
        field.BeginUndoableOperation += delegate { OnBeginUndoableOperation(); };
        field.Modified += delegate { Changed(); };
        Changed();
      }
    }

    protected void AddOperation(Operation operation)
    {
      if (operation != null && !OperationList.Contains(operation))
      {
        OnBeginUndoableOperation();
        OperationList.Add(operation);
        operation.BeginUndoableOperation += delegate { OnBeginUndoableOperation(); };
        operation.Modified += delegate { Changed(); };
        Changed();
      }
    }

    public Field GetField(int index)
    {
      if (index >= 0 && index < FieldList.Count)
        return FieldList[index];
      else
        return null;
    }

    public Operation GetOperation(int index)
    {
      if (index >= 0 && index < OperationList.Count)
        return OperationList[index];
      else
        return null;
    }

    public void RemoveMember(Member member)
    {
      if (member is Field)
      {
        OnBeginUndoableOperation();
        if (FieldList.Remove((Field)member))
          Changed();
      }
      else if (member is Operation)
      {
        OnBeginUndoableOperation();
        if (OperationList.Remove((Operation)member))
          Changed();
      }
    }

    internal void AddNestedChild(TypeBase type)
    {
      if (type != null && !_nestedChilds.Contains(type))
      {
        OnBeginUndoableOperation();
        _nestedChilds.Add(type);
        Changed();
      }
    }

    internal void RemoveNestedChild(TypeBase type)
    {
      if (type != null)
      {
        OnBeginUndoableOperation();
      }

      if (type != null && _nestedChilds.Remove(type))
        Changed();
    }

    public Operation GetDefinedOperation(Operation operation)
    {
      if (operation == null)
        return null;

      for (int i = 0; i < OperationList.Count; i++)
      {
        if (OperationList[i].HasSameSignatureAs(operation))
          return OperationList[i];
      }

      return null;
    }

    public sealed override bool MoveUpItem(object item)
    {
      if (item is Field)
      {
        OnBeginUndoableOperation();
        if (MoveUp(FieldList, item))
        {
          Changed();
          return true;
        }
      }
      else if (item is Operation)
      {
        OnBeginUndoableOperation();
        if (MoveUp(OperationList, item))
        {
          Changed();
          return true;
        }
      }
      return false;
    }

    public sealed override bool MoveDownItem(object item)
    {
      if (item is Field)
      {
        OnBeginUndoableOperation();
        if (MoveDown(FieldList, item))
        {
          Changed();
          return true;
        }
      }
      else if (item is Operation)
      {
        OnBeginUndoableOperation();
        if (MoveDown(OperationList, item))
        {
          Changed();
          return true;
        }
      }
      return false;
    }

    public void SortMembers(SortingMode sortingMode)
    {
      switch (sortingMode)
      {
        case SortingMode.ByName:
          OnBeginUndoableOperation();
          FieldList.Sort(MemberComparisonByName);
          OperationList.Sort(MemberComparisonByName);
          Changed();
          break;

        case SortingMode.ByAccess:
          OnBeginUndoableOperation();
          FieldList.Sort(MemberComparisonByAccess);
          OperationList.Sort(MemberComparisonByAccess);
          Changed();
          break;

        case SortingMode.ByKind:
          OnBeginUndoableOperation();
          FieldList.Sort(MemberComparisonByKind);
          OperationList.Sort(MemberComparisonByKind);
          Changed();
          break;
      }
    }

    private static int MemberComparisonByName(Member member1, Member member2)
    {
      return member1.Name.CompareTo(member2.Name);
    }

    private static int MemberComparisonByAccess(Member member1, Member member2)
    {
      int access1 = (int)member1.Access;
      int access2 = (int)member2.Access;

      if (access1 == access2)
        return MemberComparisonByKind(member1, member2);
      else
        return access1 - access2;
    }

    private static int MemberComparisonByKind(Member member1, Member member2)
    {
      int ret = GetMemberOrdinal(member1) - GetMemberOrdinal(member2);

      if (ret == 0)
        return MemberComparisonByName(member1, member2);

      return ret;
    }

    private static int GetMemberOrdinal(Member member)
    {
      if (member is Field)
      {
        if (((Field)member).IsConstant)
          return 0;
        else
          return 1;
      }
      if (member is Property)
      {
        Property property = (Property)member;

        if (property.IsReadonly)
          return 2;
        else if (property.IsWriteonly)
          return 3;
        else
          return 4;
      }
      if (member is Constructor)
        return 5;
      if (member is Method && ((Method)member).IsOperator)
        return 6;
      if (member is Destructor)
        return 8; // (!)
      if (member is Method)
        return 7;
      if (member is Event)
        return 9;

      // Unreachable case
      return 10;
    }

    protected override void CopyFrom(TypeBase type)
    {
      base.CopyFrom(type);

      CompositeType compositeType = (CompositeType)type;
      FieldList.Clear();
      FieldList.Capacity = compositeType.FieldList.Capacity;
      OperationList.Clear();
      OperationList.Capacity = compositeType.OperationList.Capacity;

      foreach (Field field in compositeType.FieldList)
      {
        AddField(field.Clone(this));
      }
      foreach (Operation operation in compositeType.OperationList)
      {
        AddOperation(operation.Clone(this));
      }
    }

    protected internal override void Serialize(XmlElement node)
    {
      base.Serialize(node);

      foreach (Field field in FieldList)
      {
        XmlElement child = node.OwnerDocument.CreateElement("Member");
        child.SetAttribute("type", field.MemberType.ToString());
        child.InnerText = field.ToString();
        node.AppendChild(child);
      }
      foreach (Operation operation in OperationList)
      {
        XmlElement child = node.OwnerDocument.CreateElement("Member");
        child.SetAttribute("type", operation.MemberType.ToString());
        child.InnerText = operation.ToString();
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

      foreach (XmlElement childNode in node.SelectNodes("Member|Field|Operation"))
      {
        string type = childNode.GetAttribute("type");

        if (type == "Field" || type == "CSharpField" || type == "JavaField")
        {
          Field field = AddField();
          field.InitFromString(childNode.InnerText);
        }
        else
        {
          Operation operation = GetOperation(type);

          if (operation == null)
          {
            throw new InvalidDataException(
              Strings.ErrorCorruptSaveFormat);
          }
          operation.InitFromString(childNode.InnerText);
        }
      }

      base.Deserialize(node);
      RaisePreChangedEvent = RaiseChangedEvent = true;
    }

    private Operation GetOperation(string type)
    {
      switch (type)
      {
        case "Constructor":
          return AddConstructor();

        case "Destructor":
          return AddDestructor();

        case "Method":
          return AddMethod();

        case "Property":
          return AddProperty();

        case "Event":
          return AddEvent();

        default:
          return null;
      }
    }
  }
}