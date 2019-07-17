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
using NClass.DiagramEditor.ClassDiagram.Shapes;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace NClass.DiagramEditor.ClassDiagram.Connections
{
  public sealed class TransitionConnection : Connection
  {
    private static readonly Pen LinePen = new Pen(Color.Black)
    {
      MiterLimit = 2.0F,
      LineJoin = LineJoin.MiterClipped
    };

    public TransitionConnection(Transition trans, Shape startShape, Shape endShape) :
      base(trans, startShape, endShape)
    {
      Transition = trans;
    }

    public Transition Transition { get; }

    public override Relationship Relationship => Transition;

    protected override Size EndCapSize
    {
      get { return Arrowhead.ClosedArrowSize; }
    }

    protected override void DrawEndCap(IGraphics g, bool onScreen, Style style)
    {
      LinePen.Color = style.TransitionColor;
      LinePen.Width = style.TransitionWidth;
      using (var brush = new SolidBrush(style.TransitionColor))
      {
        g.FillPath(brush, Arrowhead.ClosedArrowPath);
        g.DrawPath(LinePen, Arrowhead.ClosedArrowPath);
      }
    }

    protected override bool CloneRelationship(Diagram diagram, Shape first, Shape second)
    {
      var firstType = first.Entity as State;
      var secondType = second.Entity as State;

      if (firstType != null && secondType != null)
      {
        var clone = Transition.Clone(firstType, secondType);
        return diagram.InsertTransitionRelationship(clone);
      }
      else
      {
        return false;
      }
    }
  }
}

