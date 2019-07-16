﻿// NClass - Free class diagram editor
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

using System.Xml;

namespace NClass.Core
{
  public sealed class State : Element, IEntity
  {
    public State(string name)
    {
      Initializing = true;
      Name = name;
      Initializing = false;
    }

    private string _name;
    public string Name
    {
      get
      {
        return _name;
      }
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

    public EntityType EntityType => EntityType.State;

    public event SerializeEventHandler Serializing;
    public event SerializeEventHandler Deserializing;

    public void Deserialize(XmlElement node)
    {
      var nameChild = node["Name"];
      Name = nameChild?.InnerText;

      OnDeserializing(new SerializeEventArgs(node));
    }

    public void Serialize(XmlElement node)
    {
      var child = node.OwnerDocument.CreateElement("Name");
      child.InnerText = Name;
      node.AppendChild(child);

      OnSerializing(new SerializeEventArgs(node));
    }

    public State Clone()
    {
      return new State(Name);
    }

    private void OnSerializing(SerializeEventArgs e)
    {
      Serializing?.Invoke(this, e);
    }

    private void OnDeserializing(SerializeEventArgs e)
    {
      Deserializing?.Invoke(this, e);
    }
  }
}