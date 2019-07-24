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
using NClass.Translations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NClass.GUI
{
  public partial class TabBar : Control
  {
    private sealed class Tab
    {
      private const int MinWidth = 60;
      private const int TextMargin = 20;

      private readonly TabBar _parent;
      private string _text;

      public Tab(IDocument document, TabBar parent)
      {
        Document = document;
        _parent = parent;
        document.Renamed += document_Renamed;
        _text = document.Name;
        TextWidth = MeasureWidth(_text);
      }

      private void document_Renamed(object sender, EventArgs e)
      {
        Text = Document.Name;
      }

      public IDocument Document { get; }

      public string Text
      {
        get
        {
          return _text;
        }
        private set
        {
          if (_text != value)
          {
            _text = value;
            TextWidth = MeasureWidth(_text);
            _parent.Invalidate();
          }
        }
      }

      public float TextWidth { get; private set; }

      public int Width
      {
        get
        {
          return Math.Max(MinWidth, (int)TextWidth + TextMargin);
        }
      }

      public bool IsActive
      {
        get { return (Document == _parent._docManager.ActiveDocument); }
      }

      public void Detached()
      {
        Document.Renamed -= document_Renamed;
      }

      private float MeasureWidth(string text)
      {
        Graphics g = _parent.CreateGraphics();

        SizeF textSize = g.MeasureString(text, _parent.activeTabFont,
          _parent.MaxTabWidth, _parent.StringFormat);
        g.Dispose();

        return textSize.Width;
      }

      public override string ToString()
      {
        return _text;
      }
    }

    private const int LeftMargin = 3;
    private const int TopMargin = 3;
    private const int ClosingSignSize = 8;

    private readonly List<Tab> _tabs = new List<Tab>();
    private Tab _activeTab = null;
    private Tab _grabbedTab = null;
    private int _originalPosition = 0;
    private bool _activeCloseButton = false;
    private readonly StringFormat StringFormat = new StringFormat(StringFormat.GenericTypographic)
    {
      Alignment = StringAlignment.Center,
      LineAlignment = StringAlignment.Center,
      Trimming = StringTrimming.EllipsisCharacter
    };
    private Font activeTabFont;

    public TabBar()
    {
      InitializeComponent();
      UpdateTexts();
      BackColor = SystemColors.Control;
      SetStyle(ControlStyles.Selectable, false);
      SetStyle(ControlStyles.ResizeRedraw, true);

      StringFormat.FormatFlags |= StringFormatFlags.NoWrap;

      activeTabFont = new Font(Font, FontStyle.Bold);
    }

    private void UpdateTexts()
    {
      mnuClose.Text = Strings.MenuCloseTab;
      mnuCloseAll.Text = Strings.MenuCloseAllTabs;
      mnuCloseAllButThis.Text = Strings.MenuCloseAllTabsButThis;
    }

    private DocumentManager _docManager = null;
    [Browsable(false)]
    public DocumentManager DocumentManager
    {
      get
      {
        return _docManager;
      }
      set
      {
        if (_docManager != value)
        {
          if (_docManager != null)
          {
            _docManager.ActiveDocumentChanged -= docManager_ActiveDocumentChanged;
            _docManager.DocumentAdded -= docManager_DocumentAdded;
            _docManager.DocumentRemoved -= docManager_DocumentRemoved;
            _docManager.DocumentMoved -= docManager_DocumentMoved;
            ClearTabs();
          }
          _docManager = value;
          if (_docManager != null)
          {
            _docManager.ActiveDocumentChanged += docManager_ActiveDocumentChanged;
            _docManager.DocumentAdded += docManager_DocumentAdded;
            _docManager.DocumentRemoved += docManager_DocumentRemoved;
            _docManager.DocumentMoved += docManager_DocumentMoved;
            CreateTabs();
          }
        }
      }
    }

    [DefaultValue(typeof(Color), "Control")]
    public override Color BackColor
    {
      get
      {
        if (Parent != null && !DesignMode &&
          (_docManager == null || !_docManager.HasDocument))
        {
          return Parent.BackColor;
        }
        else
        {
          return base.BackColor;
        }
      }
      set
      {
        base.BackColor = value;
      }
    }

    [DefaultValue(typeof(Color), "ControlDark")]
    public Color BorderColor { get; set; } = SystemColors.ControlDark;

    [DefaultValue(typeof(Color), "White")]
    public Color ActiveTabColor { get; set; } = Color.White;

    [DefaultValue(typeof(Color), "ControlLight")]
    public Color InactiveTabColor { get; set; } = SystemColors.ControlLight;

    private int _maxTabWidth = 200;
    [DefaultValue(200)]
    public int MaxTabWidth
    {
      get
      {
        return _maxTabWidth;
      }
      set
      {
        _maxTabWidth = value;
        if (_maxTabWidth < 50)
          _maxTabWidth = 50;
      }
    }

    protected override Size DefaultSize
    {
      get
      {
        return new Size(100, 25);
      }
    }

    private void CreateTabs()
    {
      foreach (IDocument doc in _docManager.Documents)
      {
        Tab tab = new Tab(doc, this);
        _tabs.Add(tab);
        if (doc == _docManager.ActiveDocument)
          _activeTab = tab;
      }
    }

    private void ClearTabs()
    {
      foreach (Tab tab in _tabs)
        tab.Detached();
      _tabs.Clear();
      _activeTab = null;
    }

    private void docManager_ActiveDocumentChanged(object sender, DocumentEventArgs e)
    {
      foreach (Tab tab in _tabs)
      {
        if (tab.Document == _docManager.ActiveDocument)
        {
          _activeTab = tab;
          break;
        }
      }
      Invalidate();
    }

    private void docManager_DocumentAdded(object sender, DocumentEventArgs e)
    {
      Tab tab = new Tab(e.Document, this);
      _tabs.Add(tab);
    }

    private void docManager_DocumentRemoved(object sender, DocumentEventArgs e)
    {
      for (int index = 0; index < _tabs.Count; index++)
      {
        if (_tabs[index].Document == e.Document)
        {
          _tabs.RemoveAt(index);
          Invalidate();
          return;
        }
      }
    }

    private void docManager_DocumentMoved(object sender, DocumentMovedEventArgs e)
    {
      Tab tab = _tabs[e.OldPostion];

      if (e.NewPosition > e.OldPostion)
      {
        for (int i = e.OldPostion; i < e.NewPosition; i++)
          _tabs[i] = _tabs[i + 1];
      }
      else // e.NewPosition < e.OldPostion
      {
        for (int i = e.OldPostion; i > e.NewPosition; i--)
          _tabs[i] = _tabs[i - 1];
      }
      _tabs[e.NewPosition] = tab;
      Invalidate();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);

      Tab selectedTab = PickTab(e.Location);
      if (selectedTab != null)
      {
        if (e.Button == MouseButtons.Middle)
        {
          _docManager.Close(selectedTab.Document);
        }
        else
        {
          _docManager.ActiveDocument = selectedTab.Document;
          _grabbedTab = selectedTab;
          _originalPosition = e.X;
        }
      }
      else if (_docManager.HasDocument && IsOverClosingSign(e.Location))
      {
        _docManager.Close(_docManager.ActiveDocument);
      }
    }

    protected override void OnMouseDoubleClick(MouseEventArgs e)
    {
      base.OnMouseDoubleClick(e);

      Tab selectedTab = PickTab(e.Location);
      if (selectedTab != null && e.Button == MouseButtons.Left)
      {
        _docManager.Close(selectedTab.Document);
      }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);

      if (e.Button == MouseButtons.Left && _grabbedTab != null)
      {
        MoveTab(_grabbedTab, e.Location);
      }

      bool overClosingSign = IsOverClosingSign(e.Location);
      if (_activeCloseButton != overClosingSign)
      {
        _activeCloseButton = overClosingSign;
        Invalidate();
      }
    }

    private bool IsOverClosingSign(Point location)
    {
      int margin = (Height - ClosingSignSize) / 2;
      int left = Width - margin - ClosingSignSize;

      return (
        location.X >= left && location.X <= left + ClosingSignSize &&
        location.Y >= margin && location.Y <= margin + ClosingSignSize
      );
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);
      _grabbedTab = null;
    }

    protected override void OnMouseLeave(EventArgs e)
    {
      base.OnMouseLeave(e);

      if (_activeCloseButton)
      {
        _activeCloseButton = false;
        Invalidate();
      }
    }

    protected override void OnFontChanged(EventArgs e)
    {
      base.OnFontChanged(e);
      activeTabFont.Dispose();
      activeTabFont = new Font(Font, FontStyle.Bold);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);

      if (_tabs.Count > 0)
      {
        DrawTabs(e.Graphics);
        DrawCloseIcon(e.Graphics);
      }
    }

    private Tab PickTab(Point point)
    {
      int x = LeftMargin;

      foreach (Tab tab in _tabs)
      {
        if (point.X >= x && point.X < x + tab.Width)
          return tab;
        x += (int)tab.Width;
      }
      return null;
    }

    private void MoveTab(Tab grabbedTab, Point destination)
    {
      Tab newNeighbourTab = PickTab(destination);

      if (newNeighbourTab == grabbedTab)
      {
        _originalPosition = destination.X;
      }
      else if (newNeighbourTab != null)
      {
        int oldIndex = _tabs.IndexOf(grabbedTab);
        int newIndex = _tabs.IndexOf(newNeighbourTab);

        if (newIndex > oldIndex) // Moving right
        {
          if (destination.X >= _originalPosition)
            _docManager.MoveDocument(grabbedTab.Document, newIndex - oldIndex);
        }
        else if (newIndex < oldIndex) // Moving left
        {
          if (destination.X <= _originalPosition)
            _docManager.MoveDocument(grabbedTab.Document, newIndex - oldIndex);
        }
      }
    }

    private void DrawTabs(Graphics g)
    {
      Pen borderPen = new Pen(BorderColor);
      Brush activeTabBrush = new SolidBrush(ActiveTabColor);
      Brush inactiveTabBrush = new LinearGradientBrush(
        new Rectangle(0, 5, Width, Height - 1), ActiveTabColor,
        SystemColors.ControlLight, LinearGradientMode.Vertical);
      Brush textBrush;
      int left = LeftMargin;

      if (ForeColor.IsKnownColor)
        textBrush = SystemBrushes.FromSystemColor(ForeColor);
      else
        textBrush = new SolidBrush(ForeColor);

      g.DrawLine(borderPen, 0, Height - 1, left, Height - 1);
      foreach (Tab tab in _tabs)
      {
        bool isActiveTab = (tab == _activeTab);
        int top = (isActiveTab ? TopMargin : TopMargin + 2);
        Brush tabBrush = (isActiveTab ? activeTabBrush : inactiveTabBrush);
        Rectangle tabRectangle = new Rectangle(left, top, tab.Width, Height - top);

        // To display bottom line for inactive tabs
        if (!isActiveTab)
          tabRectangle.Height--;

        g.FillRectangle(tabBrush, tabRectangle); // Draw background
        g.DrawRectangle(borderPen, tabRectangle); // Draw border
        Font font = (isActiveTab) ? activeTabFont : Font;
        g.DrawString(tab.Text, font, textBrush, tabRectangle, StringFormat);

        left += tab.Width;
      }
      g.DrawLine(borderPen, left, Height - 1, Width - 1, Height - 1);

      borderPen.Dispose();
      if (!ForeColor.IsKnownColor)
        textBrush.Dispose();
      activeTabBrush.Dispose();
      inactiveTabBrush.Dispose();
    }

    private void DrawCloseIcon(Graphics g)
    {
      Color lineColor = _activeCloseButton ? SystemColors.ControlText : SystemColors.ControlDark;
      int margin = (Height - ClosingSignSize) / 2;
      int left = Width - margin - ClosingSignSize;
      Pen linePen = new Pen(lineColor, 2);

      g.DrawLine(linePen, left, margin, left + ClosingSignSize, margin + ClosingSignSize);
      g.DrawLine(linePen, left, margin + ClosingSignSize, left + ClosingSignSize, margin);
      linePen.Dispose();
    }

    private void contextMenu_Opening(object sender, CancelEventArgs e)
    {
      if (_docManager == null || !_docManager.HasDocument)
      {
        e.Cancel = true;
      }
    }

    private void mnuClose_Click(object sender, EventArgs e)
    {
      if (_activeTab != null)
      {
        _docManager?.Close(_activeTab.Document);
      }
    }

    private void mnuCloseAll_Click(object sender, EventArgs e)
    {
      _docManager?.CloseAll();
    }

    private void mnuCloseAllButThis_Click(object sender, EventArgs e)
    {
      if (_activeTab != null)
      {
        _docManager?.CloseAllOthers(_activeTab.Document);
      }
    }
  }
}
