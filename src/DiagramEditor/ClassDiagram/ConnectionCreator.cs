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
using NClass.Translations;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NClass.DiagramEditor.ClassDiagram
{
  internal sealed class ConnectionCreator
  {
    private const int BorderOffset = 8;
    private const int BorderOffset2 = 12;
    private const int Radius = 5;

    private static readonly float[] DashPattern = new float[] { 3, 3 };
    private static readonly Pen FirstPen = new Pen(Color.Blue)
    {
      DashPattern = DashPattern,
      Width = 1.5F
    };
    private static readonly Pen SecondPen = new Pen(Color.Red)
    {
      DashPattern = DashPattern,
      Width = 1.5F
    };
    private static readonly Pen ArrowPen = new Pen(Color.Black)
    {
      CustomEndCap = new AdjustableArrowCap(6, 7, true)
    };

    public ConnectionCreator(Diagram diagram, RelationshipType type)
    {
      Diagram = diagram;
      Type = type;
    }

    public bool Created { get; private set; } = false;

    private Diagram Diagram { get; }

    private RelationshipType Type { get; }
    private bool FirstSelected { get; set; } = false;
    private Shape First { get; set; } = null;
    private Shape Second { get; set; } = null;

    public void MouseMove(AbsoluteMouseEventArgs e)
    {
      var mouseLocation = new Point((int)e.X, (int)e.Y);

      foreach (var shape in Diagram.Shapes)
      {
        if (shape.BorderRectangle.Contains(mouseLocation))
        {
          if (!FirstSelected)
          {
            if (First != shape)
            {
              First = shape;
              Diagram.Redraw();
            }
          }
          else
          {
            if (Second != shape)
            {
              Second = shape;
              Diagram.Redraw();
            }
          }
          return;
        }
      }

      if (!FirstSelected)
      {
        if (First != null)
        {
          First = null;
          Diagram.Redraw();
        }
      }
      else
      {
        if (Second != null)
        {
          Second = null;
          Diagram.Redraw();
        }
      }
    }

    public void MouseDown(AbsoluteMouseEventArgs e)
    {
      if (!FirstSelected)
      {
        if (First != null)
          FirstSelected = true;
      }
      else
      {
        if (Second != null)
          CreateConnection();
      }
    }

    private void CreateConnection()
    {
      switch (Type)
      {
        case RelationshipType.Association:
          CreateAssociation();
          break;

        case RelationshipType.Composition:
          CreateComposition();
          break;

        case RelationshipType.Aggregation:
          CreateAggregation();
          break;

        case RelationshipType.Generalization:
          CreateGeneralization();
          break;

        case RelationshipType.Realization:
          CreateRealization();
          break;

        case RelationshipType.Dependency:
          CreateDependency();
          break;

        case RelationshipType.Nesting:
          CreateNesting();
          break;

        case RelationshipType.Comment:
          CreateCommentRelationship();
          break;

        case RelationshipType.EntityRelationship:
          CreateEntityRelationship();
          break;

        case RelationshipType.Transition:
          CreateTransitionRelationship();
          break;

        default:
          throw new ArgumentOutOfRangeException($"Unknown RelationshipType: {Type}");
      }
      Created = true;
      Diagram.Redraw();
    }

    private void CreateAssociation()
    {
      if (First is TypeShape shape1 && Second is TypeShape shape2)
      {
        Diagram.AddAssociation(shape1.TypeBase, shape2.TypeBase);
      }
      else
      {
        MessageBox.Show(Strings.ErrorCannotCreateRelationship);
      }
    }

    private void CreateComposition()
    {
      if (First is TypeShape shape1 && Second is TypeShape shape2)
      {
        Diagram.AddComposition(shape1.TypeBase, shape2.TypeBase);
      }
      else
      {
        MessageBox.Show(Strings.ErrorCannotCreateRelationship);
      }
    }

    private void CreateAggregation()
    {
      if (First is TypeShape shape1 && Second is TypeShape shape2)
      {
        Diagram.AddAggregation(shape1.TypeBase, shape2.TypeBase);
      }
      else
      {
        MessageBox.Show(Strings.ErrorCannotCreateRelationship);
      }
    }

    private void CreateGeneralization()
    {
      if (First is CompositeTypeShape shape1 && Second is CompositeTypeShape shape2)
      {
        try
        {
          Diagram.AddGeneralization(shape1.CompositeType, shape2.CompositeType);
        }
        catch (RelationshipException)
        {
          MessageBox.Show(Strings.ErrorCannotCreateRelationship);
        }
      }
      else
      {
        MessageBox.Show(Strings.ErrorCannotCreateRelationship);
      }
    }

    private void CreateRealization()
    {
      if (First is TypeShape shape1 && Second is InterfaceShape shape2)
      {
        try
        {
          Diagram.AddRealization(shape1.TypeBase, shape2.InterfaceType);
        }
        catch (RelationshipException)
        {
          MessageBox.Show(Strings.ErrorCannotCreateRelationship);
        }
      }
      else
      {
        MessageBox.Show(Strings.ErrorCannotCreateRelationship);
      }
    }

    private void CreateDependency()
    {
      if (First is TypeShape shape1 && Second is TypeShape shape2)
      {
        Diagram.AddDependency(shape1.TypeBase, shape2.TypeBase);
      }
      else
      {
        MessageBox.Show(Strings.ErrorCannotCreateRelationship);
      }
    }

    private void CreateNesting()
    {
      if (First is CompositeTypeShape shape1 && Second is TypeShape shape2)
      {
        try
        {
          Diagram.AddNesting(shape1.CompositeType, shape2.TypeBase);
        }
        catch (RelationshipException)
        {
          MessageBox.Show(Strings.ErrorCannotCreateRelationship);
        }
      }
      else
      {
        MessageBox.Show(Strings.ErrorCannotCreateRelationship);
      }
    }

    private void CreateCommentRelationship()
    {
      if (First is CommentShape shape1)
      {
        Diagram.AddCommentRelationship(shape1.Comment, Second.Entity);
      }
      else if (Second is CommentShape shape2)
      {
        Diagram.AddCommentRelationship(shape2.Comment, First.Entity);
      }
      else
      {
        MessageBox.Show(Strings.ErrorCannotCreateRelationship);
      }
    }

    private void CreateEntityRelationship()
    {
      if (First is ClassShape shape1 && Second is ClassShape shape2)
      {
        Diagram.AddEntityRelationship(shape1.ClassType, shape2.ClassType);
      }
      else
      {
        MessageBox.Show(Strings.ErrorCannotCreateRelationship);
      }
    }

    private void CreateTransitionRelationship()
    {
      if (First is StateShape shape1 && Second is StateShape shape2)
      {
        Diagram.AddTransitionRelationship(shape1.State, shape2.State);
      }
      else
      {
        MessageBox.Show(Strings.ErrorCannotCreateRelationship);
      }
    }

    public void Draw(Graphics g)
    {
      if (First != null)
      {
        Rectangle border = First.BorderRectangle;
        border.Inflate(BorderOffset, BorderOffset);
        g.DrawRectangle(FirstPen, border);
      }

      if (Second != null)
      {
        Rectangle border = Second.BorderRectangle;
        if (Second == First)
        {
          border.Inflate(BorderOffset2, BorderOffset2);
        }
        else
        {
          border.Inflate(BorderOffset, BorderOffset);
        }
        g.DrawRectangle(SecondPen, border);
      }

      if (First != null && Second != null)
      {
        g.DrawLine(ArrowPen, First.CenterPoint, Second.CenterPoint);
      }
    }
  }
}
