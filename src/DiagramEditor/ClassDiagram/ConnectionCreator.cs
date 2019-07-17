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
using System.Drawing;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using NClass.Translations;

namespace NClass.DiagramEditor.ClassDiagram
{
  internal class ConnectionCreator
  {
    private const int BorderOffset = 8;
    private const int BorderOffset2 = 12;
    private const int Radius = 5;
    private static readonly float[] dashPattern = new float[] { 3, 3 };
    private static readonly Pen firstPen;
    private static readonly Pen secondPen;
    private static readonly Pen arrowPen;

    static ConnectionCreator()
    {
      firstPen = new Pen(Color.Blue);
      firstPen.DashPattern = dashPattern;
      firstPen.Width = 1.5F;
      secondPen = new Pen(Color.Red);
      secondPen.DashPattern = dashPattern;
      secondPen.Width = 1.5F;
      arrowPen = new Pen(Color.Black);
      arrowPen.CustomEndCap = new AdjustableArrowCap(6, 7, true);
    }

    public ConnectionCreator(Diagram diagram, RelationshipType type)
    {
      this.Diagram = diagram;
      this.Type = type;
    }

    public bool Created { get; private set; } = false;

    public Diagram Diagram { get; }

    public RelationshipType Type { get; }
    public bool FirstSelected { get; set; } = false;
    public Shape First { get; set; } = null;
    public Shape Second { get; set; } = null;

    public void MouseMove(AbsoluteMouseEventArgs e)
    {
      Point mouseLocation = new Point((int)e.X, (int)e.Y);

      foreach (Shape shape in Diagram.Shapes)
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
      TypeShape shape1 = First as TypeShape;
      TypeShape shape2 = Second as TypeShape;

      if (shape1 != null && shape2 != null)
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
      TypeShape shape1 = First as TypeShape;
      TypeShape shape2 = Second as TypeShape;

      if (shape1 != null && shape2 != null)
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
      TypeShape shape1 = First as TypeShape;
      TypeShape shape2 = Second as TypeShape;

      if (shape1 != null && shape2 != null)
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
      CompositeTypeShape shape1 = First as CompositeTypeShape;
      CompositeTypeShape shape2 = Second as CompositeTypeShape;

      if (shape1 != null && shape2 != null)
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
      TypeShape shape1 = First as TypeShape;
      InterfaceShape shape2 = Second as InterfaceShape;

      if (shape1 != null && shape2 != null)
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
      TypeShape shape1 = First as TypeShape;
      TypeShape shape2 = Second as TypeShape;

      if (shape1 != null && shape2 != null)
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
      CompositeTypeShape shape1 = First as CompositeTypeShape;
      TypeShape shape2 = Second as TypeShape;

      if (shape1 != null && shape2 != null)
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
      CommentShape shape1 = First as CommentShape;
      CommentShape shape2 = Second as CommentShape;

      if (shape1 != null)
      {
        Diagram.AddCommentRelationship(shape1.Comment, Second.Entity);
      }
      else if (shape2 != null)
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
      var shape1 = First as ClassShape;
      var shape2 = Second as ClassShape;

      if (shape1 != null && shape2 != null)
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
      var shape1 = First as StateShape;
      var shape2 = Second as StateShape;

      if (shape1 != null && shape2 != null)
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
        g.DrawRectangle(firstPen, border);
      }

      if (Second != null)
      {
        Rectangle border = Second.BorderRectangle;
        if (Second == First)
          border.Inflate(BorderOffset2, BorderOffset2);
        else
          border.Inflate(BorderOffset, BorderOffset);
        g.DrawRectangle(secondPen, border);
      }

      if (First != null && Second != null)
      {
        g.DrawLine(arrowPen, First.CenterPoint, Second.CenterPoint);
      }
    }
  }
}
