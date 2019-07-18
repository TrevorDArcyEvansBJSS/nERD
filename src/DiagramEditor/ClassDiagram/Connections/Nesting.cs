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

namespace NClass.DiagramEditor.ClassDiagram.Connections
{
  internal sealed class Nesting : Connection
  {
    private const int Radius = 9;
    private const int Diameter = Radius * 2;
    private const int CrossSize = 8;
    private static readonly Pen LinePen = new Pen(Color.Black);

    public Nesting(NestingRelationship nesting, Shape startShape, Shape endShape): 
      base(nesting, startShape, endShape)
    {
      NestingRelationship = nesting;
    }

    internal NestingRelationship NestingRelationship { get; }

    public override Relationship Relationship
    {
      get { return NestingRelationship; }
    }

    protected override Size StartCapSize
    {
      get { return new Size(Diameter, Diameter); }
    }

    protected override int StartSelectionOffset
    {
      get { return Diameter; }
    }

    protected override void DrawStartCap(IGraphics g, bool onScreen, Style style)
    {
      LinePen.Color = style.RelationshipColor;
      LinePen.Width = style.RelationshipWidth;

      g.FillEllipse(Brushes.White, -Radius, 0, Diameter, Diameter);
      g.DrawEllipse(LinePen, -Radius, 0, Diameter, Diameter);
      g.DrawLine(LinePen, 0, Radius - CrossSize / 2, 0, Radius + CrossSize / 2);
      g.DrawLine(LinePen, -CrossSize / 2, Radius, CrossSize / 2, Radius);
    }

    protected override bool CloneRelationship(Diagram diagram, Shape first, Shape second)
    {
      if (first.Entity is CompositeType firstType && second.Entity is TypeBase secondType)
      {
        NestingRelationship clone = NestingRelationship.Clone(firstType, secondType);
        return diagram.InsertNesting(clone);
      }
      else
      {
        return false;
      }
    }
  }
}
