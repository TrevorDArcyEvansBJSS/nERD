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
using System.Xml;

namespace NClass.Core
{
    public enum Stage
    {
      Start,
      Intermediate,
      End
    }

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

    private Stage _stage = Stage.Intermediate;
    public Stage Stage
    {
      get
      {
        return _stage;
      }
      set
      {
        if (_stage != value)
        {
          OnBeginUndoableOperation();
          _stage = value;
          Changed();
        }
      }
    }

    public EntityType EntityType => EntityType.State;

    public event SerializeEventHandler Serializing;
    public event SerializeEventHandler Deserializing;

    public void Deserialize(XmlElement node)
    {
      Name = node["Name"]?.InnerText;
      Stage = (Stage)Enum.Parse(typeof(Stage), node["Stage"]?.InnerText ?? Stage.Intermediate.ToString());

      OnDeserializing(new SerializeEventArgs(node));
    }

    public void Serialize(XmlElement node)
    {
      var nameChild = node.OwnerDocument.CreateElement("Name");
      nameChild.InnerText = Name;
      node.AppendChild(nameChild);

      var stageChild = node.OwnerDocument.CreateElement("Stage");
      stageChild.InnerText = Stage.ToString();
      node.AppendChild(stageChild);

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