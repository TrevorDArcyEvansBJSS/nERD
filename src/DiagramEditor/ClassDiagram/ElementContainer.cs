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
using NClass.DiagramEditor.ClassDiagram.Connections;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using System.Collections.Generic;
using System.Drawing;

namespace NClass.DiagramEditor.ClassDiagram
{
  internal class ElementContainer : IClipboardItem
  {
    private const int BaseOffset = 20;

    private readonly List<Shape> _shapes = new List<Shape>();
    private readonly List<Connection> _connections = new List<Connection>();
    private readonly Dictionary<Shape, Shape> _pastedShapes = new Dictionary<Shape, Shape>();
    private int _currentOffset = 0;

    public void AddShape(Shape shape)
    {
      _shapes.Add(shape);
      _pastedShapes.Add(shape, null);
    }

    public void AddConnection(Connection connection)
    {
      _connections.Add(connection);
    }

    public void Paste(IDocument document)
    {
      Diagram diagram = (Diagram)document;
      if (diagram != null)
      {
        bool success = false;

        _currentOffset += BaseOffset;
        Size offset = new Size(
          (int)((diagram.Offset.X + _currentOffset) / diagram.Zoom),
          (int)((diagram.Offset.Y + _currentOffset) / diagram.Zoom));

        foreach (Shape shape in _shapes)
        {
          Shape pasted = shape.Paste(diagram, offset);
          _pastedShapes[shape] = pasted;
          success |= (pasted != null);
          offset += new Size(BaseOffset, BaseOffset);
        }
        foreach (Connection connection in _connections)
        {
          Shape first = GetShape(connection.Relationship.First);
          Shape second = GetShape(connection.Relationship.Second);

          if (first != null && _pastedShapes[first] != null &&
            second != null && _pastedShapes[second] != null)
          {
            Connection pasted = connection.Paste(
              diagram, offset, _pastedShapes[first], _pastedShapes[second]);
            success |= (pasted != null);
          }
        }

        if (success)
        {
          Clipboard.Clear();
        }
      }
    }

    public Shape GetShape(IEntity entity)
    {
      foreach (Shape shape in _shapes)
      {
        if (shape.Entity == entity)
          return shape;
      }
      return null;
    }
  }
}
