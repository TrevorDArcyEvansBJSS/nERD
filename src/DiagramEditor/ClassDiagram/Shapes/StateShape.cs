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
using NClass.DiagramEditor.ClassDiagram.ContextMenus;
using NClass.DiagramEditor.ClassDiagram.Editors;
using NClass.Translations;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NClass.DiagramEditor.ClassDiagram.Shapes
{
  internal sealed class StateShape : Shape
  {
    private const int DefaultWidth = 75;
    private const int DefaultHeight = 75;
    private const int MarginSize = 8;
    private const int HeaderHeight = 45;
    private int CornerRadius => (MinimumSize.Width + MinimumSize.Height) / 4;

    private static readonly StateEditor Editor = new StateEditor();
    private static readonly Pen BorderPen = new Pen(Color.Black);
    private static readonly SolidBrush BackgroundBrush = new SolidBrush(Color.White);
    private static readonly SolidBrush NameBrush = new SolidBrush(Color.Black);
    private static readonly StringFormat HeaderFormat = new StringFormat(StringFormat.GenericTypographic)
    {
      FormatFlags = StringFormatFlags.NoWrap,
      Trimming = StringTrimming.EllipsisCharacter
    };

    private bool _editorShowed = false;

    internal StateShape(State state) :
      base(state)
    {
      State = state;
    }

    public override IEntity Entity
    {
      get { return State; }
    }

    public State State { get; }

    protected override Size DefaultSize
    {
      get
      {
        return new Size(DefaultWidth, DefaultHeight);
      }
    }

    protected override bool CloneEntity(Diagram diagram)
    {
      return diagram.InsertState(State.Clone());
    }

    protected override int GetBorderWidth(Style style)
    {
      return style.StateBorderWidth;
    }

    protected override void OnMove(MoveEventArgs e)
    {
      base.OnMove(e);
      HideEditor();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
      base.OnResize(e);
      if (_editorShowed)
      {
        Editor.Relocate(this);
        if (!Editor.Focused)
        {
          Editor.Focus();
        }
      }
    }

    protected override void OnMouseDown(AbsoluteMouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        IsActive = true;
      }
      base.OnMouseDown(e);
    }

    protected override void OnDoubleClick(AbsoluteMouseEventArgs e)
    {
      if (Contains(e.Location) && e.Button == MouseButtons.Left)
      {
        ShowEditor();
      }
    }

    protected internal override void ShowEditor()
    {
      if (!_editorShowed)
      {
        Editor.Relocate(this);
        Editor.Init(this);
        ShowWindow(Editor);
        Editor.Focus();
        _editorShowed = true;
      }
    }

    protected internal override void HideEditor()
    {
      if (_editorShowed)
      {
        HideWindow(Editor);
        _editorShowed = false;
      }
    }

    protected internal override void MoveWindow()
    {
      HideEditor();
    }

    protected internal override IEnumerable<ToolStripItem> GetContextMenuItems(Diagram diagram)
    {
      return StateShapeContextMenu.Default.GetMenuItems(diagram);
    }

    private void DrawSurface(IGraphics g, bool onScreen, Style style)
    {
      // Update graphical objects
      BackgroundBrush.Color = style.StateBackColor;
      BorderPen.Color = style.StateBorderColor;
      BorderPen.Width = style.StateBorderWidth;
      ShadowBrush.Color = style.ShadowColor;
      NameBrush.Color = style.NameColor;

      DrawRectangleSurface(g, onScreen, style);
    }

    private void DrawRectangleSurface(IGraphics g, bool onScreen, Style style)
    {
      using (var outline = RoundedRect(BorderRectangle, CornerRadius))
      {
        // Draw shadow
        if ((!onScreen || !IsSelected) && !style.ShadowOffset.IsEmpty)
        {
          g.TranslateTransform(style.ShadowOffset.Width, style.ShadowOffset.Height);
          g.FillPath(ShadowBrush, outline);
          g.TranslateTransform(-style.ShadowOffset.Width, -style.ShadowOffset.Height);
        }

        // Draw background
        g.FillPath(BackgroundBrush, outline);

        if (State.Stage == Stage.Start)
        {
          g.FillPath(NameBrush, outline);
        }

        if (State.Stage == Stage.End)
        {
          var innerRect = BorderRectangle;
          innerRect.Inflate(-CornerRadius / 2, -CornerRadius / 2);
          using (var innerCircle = RoundedRect(innerRect, CornerRadius / 2))
          {
            g.FillPath(NameBrush, innerCircle);
          }
        }

        // Draw border
        g.DrawPath(BorderPen, outline);
      }
    }

    private void DrawHeaderText(IGraphics g, Style style)
    {
      // Update styles
      HeaderFormat.Alignment = GetHorizontalAlignment(style.HeaderAlignment);

      // Draw name
      HeaderFormat.LineAlignment = GetVerticalAlignment(style.HeaderAlignment);
      g.DrawString(State.Name, GetNameFont(style), NameBrush, CaptionRegion, HeaderFormat);
    }

    private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
      var diameter = radius * 2;
      var size = new Size(diameter, diameter);
      var arc = new Rectangle(bounds.Location, size);
      var path = new GraphicsPath();

      if (radius == 0)
      {
        path.AddRectangle(bounds);
        return path;
      }

      // top left arc
      path.AddArc(arc, 180, 90);

      // top right arc
      arc.X = bounds.Right - diameter;
      path.AddArc(arc, 270, 90);

      // bottom right arc
      arc.Y = bounds.Bottom - diameter;
      path.AddArc(arc, 0, 90);

      // bottom left arc
      arc.X = bounds.Left;
      path.AddArc(arc, 90, 90);

      path.CloseFigure();

      return path;
    }

    public override void Draw(IGraphics g, bool onScreen, Style style)
    {
      DrawSurface(g, onScreen, style);
      DrawHeaderText(g, style);
    }

    protected override float GetRequiredWidth(Graphics g, Style style)
    {
      return Width;
    }

    protected override int GetRequiredHeight()
    {
      return Height;
    }

    private static Font GetNameFont(Style style)
    {
      return style.NameFont;
    }

    private static StringAlignment GetHorizontalAlignment(ContentAlignment alignment)
    {
      switch (alignment)
      {
        case ContentAlignment.BottomLeft:
        case ContentAlignment.MiddleLeft:
        case ContentAlignment.TopLeft:
          return StringAlignment.Near;

        case ContentAlignment.BottomCenter:
        case ContentAlignment.MiddleCenter:
        case ContentAlignment.TopCenter:
        default:
          return StringAlignment.Center;

        case ContentAlignment.BottomRight:
        case ContentAlignment.MiddleRight:
        case ContentAlignment.TopRight:
          return StringAlignment.Far;
      }
    }

    private static StringAlignment GetVerticalAlignment(ContentAlignment alignment)
    {
      switch (alignment)
      {
        case ContentAlignment.TopLeft:
        case ContentAlignment.TopCenter:
        case ContentAlignment.TopRight:
          return StringAlignment.Near;

        case ContentAlignment.MiddleLeft:
        case ContentAlignment.MiddleCenter:
        case ContentAlignment.MiddleRight:
        default:
          return StringAlignment.Center;

        case ContentAlignment.BottomLeft:
        case ContentAlignment.BottomCenter:
        case ContentAlignment.BottomRight:
          return StringAlignment.Far;
      }
    }

    private RectangleF CaptionRegion
    {
      get
      {
        return new RectangleF(
          Left + MarginSize, Top + MarginSize,
          Width - MarginSize * 2, HeaderHeight - MarginSize * 2
        );
      }
    }

    public override string ToString()
    {
      return $"{Strings.State}: {State.Name}";
    }
  }
}
