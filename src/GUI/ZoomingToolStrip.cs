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

using NClass.DiagramEditor;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;

namespace NClass.GUI
{
  [ToolStripItemDesignerAvailability(
    ToolStripItemDesignerAvailability.ToolStrip |
    ToolStripItemDesignerAvailability.StatusStrip)]
  [DefaultEvent("ZoomValueChanged")]
  [DefaultProperty("ZoomValue")]
  public sealed class ZoomingToolStrip : ToolStripItem
  {
    private const float MaxValue = Canvas.MaxZoom;
    private const float MinValue = Canvas.MinZoom;
    private const float DefaultValue = 1.0F;
    private const int MinWidth = 100;
    private const int PrecisionSize = 4;

    private static readonly int SliderWidth = Properties.Resources.Slider.Width;
    private static readonly int SliderHeight = Properties.Resources.Slider.Height;
    private static readonly Pen LinePen = SystemPens.ControlDarkDark;
    private static readonly Pen DisabledPen = SystemPens.ControlDark;

    public event EventHandler ZoomValueChanged;

    private float _zoomValue = DefaultValue;
    [DefaultValue(DefaultValue)]
    public float ZoomValue
    {
      get
      {
        return _zoomValue;
      }
      set
      {
        if (value > MaxValue) value = MaxValue;
        if (value < MinValue) value = MinValue;

        if (_zoomValue != value)
        {
          _zoomValue = value;
          OnZoomValueChanged(EventArgs.Empty);
          Invalidate();
        }
      }
    }

    protected override Size DefaultSize
    {
      get
      {
        return new Size(MinWidth, base.DefaultSize.Height);
      }
    }

    public override Size GetPreferredSize(Size constrainingSize)
    {
      return new Size(
        Math.Max(MinWidth, Size.Width), Size.Height);
    }

    private void MoveSlider(int location, bool snapToCenter)
    {
      int center = Width / 2;

      // Mono hack for a ToolStripItem layout problem
      if (MonoHelper.IsRunningOnMono)
        location -= this.Bounds.Left;

      if (snapToCenter && Math.Abs(location - center) <= PrecisionSize)
        location = center;

      if (location < center)
      {
        int left = SliderWidth / 2;
        float scale = (DefaultValue - MinValue) / (center - left);
        ZoomValue = (location - left) * scale + MinValue;
      }
      else // location >= center
      {
        int right = Width - SliderWidth / 2;
        float scale = (MaxValue - DefaultValue) / (right - center);
        ZoomValue = (location - center) * scale + DefaultValue;
      }
    }

    private void OnZoomValueChanged(EventArgs e)
    {
      ZoomValueChanged?.Invoke(this, e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);

      if (Enabled && e.Button == MouseButtons.Left)
      {
        bool snapToCenter = (Control.ModifierKeys == Keys.None);
        MoveSlider(e.X, snapToCenter);
      }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseDown(e);

      if (Enabled && e.Button == MouseButtons.Left)
      {
        bool snapToCenter = (Control.ModifierKeys == Keys.None);
        MoveSlider(e.X, snapToCenter);
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      Graphics g = e.Graphics;

      Point center = new Point(Width / 2, Height / 2);
      int left = SliderWidth / 2;
      int right = Width - SliderWidth / 2;
      int top = center.Y - 3;
      int bottom = center.Y + 3;

      // Draw horizontal and vertical lines
      Pen pen = (Enabled) ? LinePen : DisabledPen;
      g.DrawLine(pen, left, center.Y, right, center.Y);
      g.DrawLine(pen, center.X, top, center.X, bottom);

      if (Enabled)
        DrawSlider(g, TrackBarThumbState.Normal);
      else
        DrawSlider(g, TrackBarThumbState.Disabled);
    }

    private void DrawSlider(Graphics g, TrackBarThumbState state)
    {
      int sliderLocation;

      if (ZoomValue < DefaultValue)
      {
        int regionWidth = Width / 2 - SliderWidth / 2;
        float scale = (float)regionWidth / (DefaultValue - MinValue);
        sliderLocation = (int)Math.Round(scale * (ZoomValue - MinValue)) + SliderWidth / 2;
      }
      else
      {
        int regionWidth = Width / 2 - SliderWidth / 2;
        float scale = (float)regionWidth / (MaxValue - DefaultValue);
        sliderLocation = (int)Math.Round(scale * (ZoomValue - DefaultValue)) + Width / 2;
      }

      Image slider;
      if (state == TrackBarThumbState.Disabled)
        slider = Properties.Resources.DisabledSlider;
      else
        slider = Properties.Resources.Slider;
      int top = Height / 2 - SliderHeight / 2;
      int left = sliderLocation - SliderWidth / 2;

      g.DrawImage(slider, left, top, SliderWidth, SliderHeight);
    }
  }
}