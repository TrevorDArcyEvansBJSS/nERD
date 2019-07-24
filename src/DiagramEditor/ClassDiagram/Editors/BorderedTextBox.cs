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
using System.ComponentModel;
using System.Windows.Forms;

namespace NClass.DiagramEditor.ClassDiagram.Editors
{
  public sealed class BorderedTextBox : UserControl
  {
    private sealed class TabTextBox : TextBox
    {
      protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
      {
        if (keyData == Keys.Enter && AcceptsReturn)
        {
          OnKeyDown(new KeyEventArgs(keyData));
          return true;
        }
        else if (keyData == Keys.Tab && AcceptsTab)
        {
          OnKeyDown(new KeyEventArgs(keyData));
          return true;
        }
        else
        {
          return base.ProcessCmdKey(ref msg, keyData);
        }
      }
    }

    public BorderedTextBox()
    {
      TextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      TextBox.BorderStyle = BorderStyle.FixedSingle;
      TextBox.Location = new Point(-1, -1);
      TextBox.AcceptsReturn = true;
      Panel.Dock = DockStyle.Fill;
      Panel.Size = TextBox.Size - new Size(2, 0);
      Panel.Controls.Add(TextBox);

      TextBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
      TextBox.TextChanged += new EventHandler(textBox_TextChanged);
      TextBox.Validating += new CancelEventHandler(textBox_Validating);
      TextBox.GotFocus += new EventHandler(textBox_GotFocus);
      TextBox.LostFocus += new EventHandler(textBox_LostFocus);

      Padding = new Padding(1);
      BorderColor = SystemColors.ControlDark;
      Controls.Add(Panel);
    }

    [DefaultValue(typeof(Color), "ControlDark")]
    public Color BorderColor
    {
      get { return base.BackColor; }
      set { base.BackColor = value; }
    }

    [DefaultValue(typeof(Color), "Window")]
    public new Color BackColor
    {
      get { return TextBox.BackColor; }
      set { TextBox.BackColor = value; }
    }

    public bool ReadOnly
    {
      get { return TextBox.ReadOnly; }
      set { TextBox.ReadOnly = value; }
    }

    public override string Text
    {
      get { return TextBox.Text; }
      set { TextBox.Text = value; }
    }

    [DefaultValue(true)]
    public bool AcceptsReturn
    {
      get { return TextBox.AcceptsReturn; }
      set { TextBox.AcceptsReturn = value; }
    }

    [DefaultValue(false)]
    public bool AcceptsTab
    {
      get { return TextBox.AcceptsTab; }
      set { TextBox.AcceptsTab = value; }
    }

    /// <exception cref="ArgumentOutOfRangeException">
    /// The assigned value is less than zero.
    /// </exception>
    public int SelectionStart
    {
      get { return TextBox.SelectionStart; }
      set { TextBox.SelectionStart = value; }
    }

    private TabTextBox TextBox { get; } = new TabTextBox();

    public Panel Panel { get; } = new Panel();

    private void textBox_KeyDown(object sender, KeyEventArgs e)
    {
      OnKeyDown(e);
    }

    private void textBox_TextChanged(object sender, EventArgs e)
    {
      OnTextChanged(e);
    }

    private void textBox_GotFocus(object sender, EventArgs e)
    {
      OnGotFocus(e);
    }

    private void textBox_LostFocus(object sender, EventArgs e)
    {
      OnLostFocus(e);
    }

    private void textBox_Validating(object sender, CancelEventArgs e)
    {
      OnValidating(e);
    }

    protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
    {
      base.SetBoundsCore(x, y, width, TextBox.PreferredHeight, specified);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        TextBox.Dispose();
        Panel.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
