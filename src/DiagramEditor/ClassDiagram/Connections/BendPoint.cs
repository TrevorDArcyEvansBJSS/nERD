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

using NClass.DiagramEditor.ClassDiagram.Shapes;
using System;
using System.Drawing;
using System.Xml;

namespace NClass.DiagramEditor.ClassDiagram.Connections
{
  public sealed class BendPoint
  {
    private const int Spacing = Connection.Spacing;
    internal const int SquareSize = 8;

    private static readonly Color DarkStartColor = Color.Blue;
    private static readonly Color DarkEndColor = Color.Red;
    private static readonly Color LightStartColor = Color.FromArgb(178, 178, 255);
    private static readonly Color LightEndColor = Color.FromArgb(255, 178, 178);
    private static readonly Pen SquarePen = new Pen(Color.Black);
    private static readonly SolidBrush SquareBrush = new SolidBrush(Color.Black);

    private readonly Shape _relativeShape;
    private Size _relativePosition = Size.Empty;

    public BendPoint(Shape relativeShape, bool relativeToStartShape)
    {
      if (relativeShape == null)
        throw new ArgumentNullException("relativeShape");

      _relativeShape = relativeShape;
      RelativeToStartShape = relativeToStartShape;
    }

    public BendPoint(Shape relativeShape, bool relativeToStartShape, bool autoPosition) :
      this(relativeShape, relativeToStartShape)
    {
      AutoPosition = autoPosition;
    }

    public bool RelativeToStartShape { get; set; }

    internal bool AutoPosition { get; set; } = true;

    public int X
    {
      get
      {
        return (_relativeShape.X + _relativePosition.Width);
      }
      internal set
      {
        _relativePosition.Width = value - _relativeShape.X;
      }
    }

    public int Y
    {
      get
      {
        return (_relativeShape.Y + _relativePosition.Height);
      }
      internal set
      {
        _relativePosition.Height = value - _relativeShape.Y;
      }
    }

    public Point Location
    {
      get
      {
        return (_relativeShape.Location + _relativePosition);
      }
      set
      {
        if (value.X > _relativeShape.Left - Spacing &&
          value.X < _relativeShape.Right + Spacing &&
          value.Y > _relativeShape.Top - Spacing &&
          value.Y < _relativeShape.Bottom + Spacing)
        {
          if (X <= _relativeShape.Left - Spacing)
          {
            X = _relativeShape.Left - Spacing;
            Y = value.Y;
          }
          else if (X >= _relativeShape.Right + Spacing)
          {
            X = _relativeShape.Right + Spacing;
            Y = value.Y;
          }
          else if (Y <= _relativeShape.Top - Spacing)
          {
            X = value.X;
            Y = _relativeShape.Top - Spacing;
          }
          else
          {
            X = value.X;
            Y = _relativeShape.Bottom + Spacing;
          }
        }
        else
        {
          X = value.X;
          Y = value.Y;
        }
      }
    }

    public object Clone()
    {
      return this.MemberwiseClone();
    }

    internal void Draw(Graphics g, bool onScreen, float zoom, Point offset)
    {
      int x = (int)(X * zoom) - SquareSize / 2 - offset.X;
      int y = (int)(Y * zoom) - SquareSize / 2 - offset.Y;
      Rectangle square = new Rectangle(x, y, SquareSize, SquareSize);

      if (AutoPosition)
      {
        SquarePen.Color = RelativeToStartShape ? LightStartColor : LightEndColor;
        g.DrawRectangle(SquarePen, square.X, square.Y, square.Width, square.Height);
      }
      else
      {
        SquarePen.Color = RelativeToStartShape ? DarkStartColor : DarkEndColor;
        SquareBrush.Color = RelativeToStartShape ? LightStartColor : LightEndColor;

        g.FillRectangle(SquareBrush, square);
        g.DrawRectangle(SquarePen, square);
      }
    }

    internal bool Contains(PointF point, float zoom)
    {
      float halfSize = SquareSize / zoom / 2;

      return (
        point.X >= X - halfSize && point.X <= X + halfSize &&
        point.Y >= Y - halfSize && point.Y <= Y + halfSize
      );
    }

    internal void ShapeResized(Size size)
    {
      if (X >= _relativeShape.Left && X <= _relativeShape.Right && Y > _relativeShape.Top)
        Y += size.Height;

      if (Y >= _relativeShape.Top && Y <= _relativeShape.Bottom && X > _relativeShape.Left)
        X += size.Width;
    }

    internal void Serialize(XmlElement node)
    {
      XmlDocument document = node.OwnerDocument;

      XmlElement xNode = document.CreateElement("X");
      xNode.InnerText = X.ToString();
      node.AppendChild(xNode);

      XmlElement yNode = document.CreateElement("Y");
      yNode.InnerText = Y.ToString();
      node.AppendChild(yNode);
    }

    internal void Deserialize(XmlElement node)
    {
      XmlElement xNode = node["X"];
      if (xNode != null)
      {
        int x;
        int.TryParse(xNode.InnerText, out x);
        X = x;
      }
      XmlElement yNode = node["Y"];
      if (yNode != null)
      {
        int y;
        int.TryParse(yNode.InnerText, out y);
        Y = y;
      }
    }
  }
}
