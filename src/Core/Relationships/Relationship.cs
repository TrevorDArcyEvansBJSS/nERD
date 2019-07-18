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
  public abstract class Relationship : Element, ISerializableElement
  {
    public event EventHandler Attaching;
    public event EventHandler Detaching;
    public event SerializeEventHandler Serializing;
    public event SerializeEventHandler Deserializing;

    public IEntity First { get; protected set; }
    public IEntity Second { get; protected set; }
    public RelationshipType RelationshipType { get; protected set; }

    private string _label = string.Empty;
    public virtual string Label
    {
      get
      {
        return _label;
      }
      set
      {
        if (value == "")
          value = null;

        if (_label != value && SupportsLabel)
        {
          OnBeginUndoableOperation();
          _label = value;
          Changed();
        }
      }
    }

    public bool SupportsLabel { get; protected set; } = false;

    private bool _attached = false;
    /// <exception cref="RelationshipException">
    /// Cannot finalize relationship.
    /// </exception>
    internal void Attach()
    {
      if (!_attached)
      {
        OnAttaching(EventArgs.Empty);
      }
      _attached = true;
    }

    internal void Detach()
    {
      if (_attached)
      {
        OnDetaching(EventArgs.Empty);
      }
      _attached = false;
    }

    protected virtual void CopyFrom(Relationship relationship)
    {
      _label = relationship._label;
    }

    public virtual void Serialize(XmlElement node)
    {
      if (SupportsLabel && Label != null)
      {
        XmlElement labelNode = node.OwnerDocument.CreateElement("Label");
        labelNode.InnerText = Label.ToString();
        node.AppendChild(labelNode);
      }
      OnSerializing(new SerializeEventArgs(node));
    }

    public virtual void Deserialize(XmlElement node)
    {
      if (SupportsLabel)
      {
        XmlElement labelNode = node["Label"];
        if (labelNode != null)
        {
          Label = labelNode.InnerText;
        }
      }
      OnDeserializing(new SerializeEventArgs(node));
    }

    protected virtual void OnAttaching(EventArgs e)
    {
      Attaching?.Invoke(this, e);
    }

    protected virtual void OnDetaching(EventArgs e)
    {
      Detaching?.Invoke(this, e);
    }

    private void OnSerializing(SerializeEventArgs e)
    {
      Serializing?.Invoke(this, e);
    }

    private void OnDeserializing(SerializeEventArgs e)
    {
      Deserializing?.Invoke(this, e);
    }

    public abstract override string ToString();
  }
}
