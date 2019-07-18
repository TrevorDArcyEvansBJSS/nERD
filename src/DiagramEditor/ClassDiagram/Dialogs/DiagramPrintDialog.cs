﻿// NClass - Free class diagram editor
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

using NClass.Translations;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace NClass.DiagramEditor.ClassDiagram.Dialogs
{

  public partial class DiagramPrintDialog : Form
  {
    private int _pageIndex = 0;
    private int _rows = 1;
    private int _columns = 1;
    private bool _selectedOnly = false;
    private Style _selectedStyle = Style.CurrentStyle;
    private Style _printingStyle = null;

    public DiagramPrintDialog()
    {
      InitializeComponent();
      printPreview.AutoZoom = true;
      printDocument.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
    }

    public IDocument Document { get; set; } = null;

    private int PageCount
    {
      get { return _rows * _columns; }
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      UpdateTexts();
      LoadSettings();
      LoadStyles();
    }

    private void UpdateTexts()
    {
      this.Text = Strings.Print;
      btnPrinter.Text = Strings.ButtonSelectPrinter;
      btnPageSetup.Text = Strings.ButtonPageSetup;
      lblStyle.Text = Strings.Style;
      lblPages.Text = Strings.Pages;
      chkSelectedOnly.Text = Strings.PrintOnlySelectedElements;
      btnPrint.Text = Strings.ButtonPrint;
      btnCancel.Text = Strings.ButtonCancel;

      int buttonWidth = Math.Max(btnPrinter.Width, btnPageSetup.Width);
      btnPrinter.Width = buttonWidth;
      btnPageSetup.Width = buttonWidth;

      int minLeft = btnPrinter.Left + buttonWidth + 6;
      lblStyle.Left = minLeft;
      lblPages.Left = minLeft;

      minLeft = Math.Max(lblStyle.Right, lblPages.Right);
      cboStyle.Left = minLeft + 6;
      numColumns.Left = minLeft + 6;
      lblX.Left = numColumns.Right + 1;
      numRows.Left = lblX.Right + 1;
      chkSelectedOnly.Left = numRows.Right + 14;
    }

    private void LoadSettings()
    {
      PrintingSettings settings = Settings.Default.PrintingSettings;
      if (settings != null)
      {
        printDocument.DefaultPageSettings.Landscape = settings.Landscape;
        printDocument.DefaultPageSettings.Margins = settings.Margins;
        printDocument.DefaultPageSettings.PaperSize = settings.PaperSize;
        printDocument.DefaultPageSettings.PaperSource = settings.PaperSource;
        printDocument.DefaultPageSettings.PrinterSettings.PrinterName = settings.PrinterName;
      }

      if (!pageSetupDialog.PrinterSettings.IsValid)
      {
        printDocument.PrinterSettings = new PrinterSettings();
        pageSetupDialog.Document = printDocument;
      }
    }

    private void LoadStyles()
    {
      cboStyle.Items.Clear();
      foreach (Style style in Style.AvailableStyles)
      {
        cboStyle.Items.Add(style);
        if (style == Style.CurrentStyle)
          cboStyle.SelectedItem = style;
      }
    }

    private void SaveSettings()
    {
      Settings.Default.PrintingSettings = new PrintingSettings()
      {
        Landscape = printDocument.DefaultPageSettings.Landscape,
        Margins = printDocument.DefaultPageSettings.Margins,
        PaperSize = printDocument.DefaultPageSettings.PaperSize,
        PaperSource = printDocument.DefaultPageSettings.PaperSource,
        PrinterName = printDocument.PrinterSettings.PrinterName
      };
      Settings.Default.Save();
    }

    public new DialogResult ShowDialog()
    {
      return ShowDialog(null);
    }

    public new DialogResult ShowDialog(IWin32Window owner)
    {
      if (printDocument.PrinterSettings.IsValid)
      {
        printPreview.InvalidatePreview();
        return base.ShowDialog(owner);
      }
      else
      {
        MessageBox.Show(Strings.ErrorNoPrinters, Strings.Error,
          MessageBoxButtons.OK, MessageBoxIcon.Error);

        return DialogResult.Cancel;
      }
    }

    private void Print()
    {
      try
      {
        printDocument.Print();
        SaveSettings();
      }
      catch (InvalidPrinterException ex)
      {
        MessageBox.Show(string.Format(Strings.ErrorPrinting, ex.Message),
          Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private static Style MakeShadowsOpaque(Style selectedStyle)
    {
      Style converted = selectedStyle.Clone();
      converted.ShadowColor = DisableTransparency(converted.ShadowColor);
      return converted;
    }

    private static Color DisableTransparency(Color color)
    {
      int red = color.R * color.A / 255 + (255 - color.A);
      int green = color.G * color.A / 255 + (255 - color.A);
      int blue = color.B * color.A / 255 + (255 - color.A);

      return Color.FromArgb(red, green, blue);
    }

    private void printDocument_BeginPrint(object sender, PrintEventArgs e)
    {
      if (Document != null && printDocument.PrinterSettings.IsValid)
      {
        _pageIndex = 0;
        _printingStyle = MakeShadowsOpaque(_selectedStyle);
        printDocument.DocumentName = Document.Name;
      }
      else
      {
        e.Cancel = true;
      }
    }

    private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
    {
      // Scale the page to match sizes of the screen
      e.Graphics.PageUnit = GraphicsUnit.Inch;
      e.Graphics.PageScale = 1 / DiagramElement.Graphics.DpiX;

      // Get the phisical page margins
      float marginScale = DiagramElement.Graphics.DpiX / 100;
      RectangleF marginBounds = e.MarginBounds;
      if (!printDocument.PrintController.IsPreview)
        marginBounds.Offset(-e.PageSettings.HardMarginX, -e.PageSettings.HardMarginY);
      marginBounds = new RectangleF(
        marginBounds.X * marginScale, marginBounds.Y * marginScale,
        marginBounds.Width * marginScale, marginBounds.Height * marginScale);

      // Get logical area information
      RectangleF drawingArea = Document.GetPrintingArea(_selectedOnly);
      int column = _pageIndex % _columns;
      int row = _pageIndex / _columns;

      // Get zooming information if diagram is too big
      float scaleX = _columns * marginBounds.Width / drawingArea.Width;
      float scaleY = _rows * marginBounds.Height / drawingArea.Height;
      float scale = Math.Min(scaleX, scaleY);
      if (scale > 1) scale = 1; // No need for zooming in

      // Set the printing clip region
      RectangleF clipBounds = marginBounds;
      if (column == 0)
      {
        clipBounds.X = 0;
        clipBounds.Width += marginBounds.Left;
      }
      if (row == 0)
      {
        clipBounds.Y = 0;
        clipBounds.Height += marginBounds.Top;
      }
      if (column == _columns - 1)
      {
        clipBounds.Width += marginBounds.Left;
      }
      if (row == _rows - 1)
      {
        clipBounds.Height += marginBounds.Top;
      }
      e.Graphics.SetClip(clipBounds);

      // Moving the image to it's right position
      e.Graphics.TranslateTransform(-column * marginBounds.Width, -row * marginBounds.Height);
      e.Graphics.TranslateTransform(marginBounds.Left, marginBounds.Top);
      e.Graphics.ScaleTransform(scale, scale);
      e.Graphics.TranslateTransform(-drawingArea.Left, -drawingArea.Top);

      // Printing
      IGraphics graphics = new GdiGraphics(e.Graphics);
      Document.Print(graphics, _selectedOnly, _printingStyle);
      e.HasMorePages = (++_pageIndex < PageCount);
    }

    private void printDocument_EndPrint(object sender, PrintEventArgs e)
    {
      _printingStyle?.Dispose();
      _printingStyle = null;
    }

    private void printPreview_Click(object sender, EventArgs e)
    {
      if (printPreview.AutoZoom)
      {
        printPreview.AutoZoom = false;
        printPreview.Zoom = 1.0;
      }
      else
      {
        printPreview.AutoZoom = true;
      }
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      Print();
    }

    private void btnPrinter_Click(object sender, EventArgs e)
    {
      if (selectPrinterDialog.ShowDialog() == DialogResult.OK)
      {
        Print();
        this.Close();
      }
    }

    private void btnPageSetup_Click(object sender, EventArgs e)
    {
      Margins originalMargins = pageSetupDialog.PageSettings.Margins;

      if (System.Globalization.RegionInfo.CurrentRegion.IsMetric && !MonoHelper.IsRunningOnMono)
      {
        // This is necessary because of a bug in PageSetupDialog control.
        // More information: http://support.microsoft.com/?id=814355
        pageSetupDialog.PageSettings.Margins = PrinterUnitConvert.Convert(
          pageSetupDialog.PageSettings.Margins,
          PrinterUnit.Display, PrinterUnit.TenthsOfAMillimeter);
      }

      if (pageSetupDialog.ShowDialog() == DialogResult.OK)
        printPreview.InvalidatePreview();
      else
        pageSetupDialog.PageSettings.Margins = originalMargins;
    }

    private void cboStyle_SelectedIndexChanged(object sender, EventArgs e)
    {
      Style style = cboStyle.SelectedItem as Style;
      if (style != null)
      {
        _selectedStyle = style;
        printPreview.InvalidatePreview();
      }
    }

    private void numColumns_ValueChanged(object sender, EventArgs e)
    {
      _columns = (int)numColumns.Value;
      printPreview.Columns = _columns;
      printPreview.AutoZoom = true;
      printPreview.InvalidatePreview();
    }

    private void numRows_ValueChanged(object sender, EventArgs e)
    {
      _rows = (int)numRows.Value;
      printPreview.Rows = _rows;
      printPreview.AutoZoom = true;
      printPreview.InvalidatePreview();
    }

    private void chkSelectedOnly_CheckedChanged(object sender, EventArgs e)
    {
      _selectedOnly = (chkSelectedOnly.Checked);
      printPreview.InvalidatePreview();
    }
  }
}