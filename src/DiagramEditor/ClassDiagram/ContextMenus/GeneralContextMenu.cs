// NClass - Free class diagram editor
// Copyright (C) 2006-2007 Balazs Tihanyi
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

using NClass.DiagramEditor.Properties;
using NClass.Translations;
using System;
using System.Windows.Forms;

namespace NClass.DiagramEditor.ClassDiagram.ContextMenus
{
  public sealed class GeneralContextMenu : DiagramContextMenu
  {
    private readonly ToolStripMenuItem mnuCut;
    private readonly ToolStripMenuItem mnuCopy;
    private readonly ToolStripMenuItem mnuDelete;
    private readonly ToolStripMenuItem mnuCopyAsImage;
    private readonly ToolStripMenuItem mnuSaveAsImage;

    private GeneralContextMenu()
    {
      mnuCut = new ToolStripMenuItem(Strings.MenuCut, Resources.Cut, mnuCut_Click);
      mnuCopy = new ToolStripMenuItem(Strings.MenuCopy, Resources.Copy, mnuCopy_Click);
      mnuDelete = new ToolStripMenuItem(Strings.MenuDelete, Resources.Delete, mnuDelete_Click);
      mnuCopyAsImage = new ToolStripMenuItem(Strings.MenuCopyImageToClipboard, Resources.CopyAsImage, mnuCopyAsImage_Click);
      mnuSaveAsImage = new ToolStripMenuItem(Strings.MenuSaveSelectionAsImage, Resources.Image, mnuSaveAsImage_Click);

      MenuList.AddRange(new ToolStripItem[] {
        mnuCut,
        mnuCopy,
        mnuDelete,
        new ToolStripSeparator(),
        mnuCopyAsImage,
        mnuSaveAsImage,
      });

      UpdateTexts();
    }

    public static GeneralContextMenu Default { get; } = new GeneralContextMenu();

    private void UpdateTexts()
    {
      mnuCut.Text = Strings.MenuCut;
      mnuCopy.Text = Strings.MenuCopy;
      mnuDelete.Text = Strings.MenuDelete;
      mnuCopyAsImage.Text = Strings.MenuCopyImageToClipboard;
      mnuSaveAsImage.Text = Strings.MenuSaveSelectionAsImage;
    }

    public override void ValidateMenuItems(Diagram diagram)
    {
      base.ValidateMenuItems(diagram);
      mnuCut.Enabled = diagram.CanCutToClipboard;
      mnuCopy.Enabled = diagram.CanCopyToClipboard;
    }

    private void mnuCut_Click(object sender, EventArgs e)
    {
      Diagram?.Cut();
    }

    private void mnuCopy_Click(object sender, EventArgs e)
    {
      Diagram?.Copy();
    }

    private void mnuDelete_Click(object sender, EventArgs e)
    {
      Diagram?.DeleteSelectedElements();
    }

    private void mnuCopyAsImage_Click(object sender, EventArgs e)
    {
      Diagram?.CopyAsImage();
    }

    private void mnuSaveAsImage_Click(object sender, EventArgs e)
    {
      Diagram?.SaveAsImage(true);
    }
  }
}