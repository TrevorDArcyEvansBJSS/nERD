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

using NClass.DiagramEditor.ClassDiagram.Shapes;
using NClass.DiagramEditor.Properties;
using NClass.Translations;
using System;
using System.Windows.Forms;

namespace NClass.DiagramEditor.ClassDiagram.ContextMenus
{
  internal sealed class TypeShapeContextMenu : DiagramContextMenu
  {
    private readonly ToolStripMenuItem mnuSize;
    private readonly ToolStripMenuItem mnuAutoSize;
    private readonly ToolStripMenuItem mnuAutoWidth;
    private readonly ToolStripMenuItem mnuAutoHeight;
    private readonly ToolStripMenuItem mnuCollapseAllSelected;
    private readonly ToolStripMenuItem mnuExpandAllSelected;
    private readonly ToolStripMenuItem mnuEditMembers;

    private TypeShapeContextMenu()
    {
      mnuEditMembers = new ToolStripMenuItem(Strings.MenuEditMembers, Resources.EditMembers, mnuEditMembers_Click);
      mnuAutoSize = new ToolStripMenuItem(Strings.MenuAutoSize, null, mnuAutoSize_Click);
      mnuAutoWidth = new ToolStripMenuItem(Strings.MenuAutoWidth, null, mnuAutoWidth_Click);
      mnuAutoHeight = new ToolStripMenuItem(Strings.MenuAutoHeight, null, mnuAutoHeight_Click);
      mnuCollapseAllSelected = new ToolStripMenuItem(Strings.MenuCollapseAllSelected, null, mnuCollapseAllSelected_Click);
      mnuExpandAllSelected = new ToolStripMenuItem(Strings.MenuExpandAllSelected, null, mnuExpandAllSelected_Click);
      mnuSize = new ToolStripMenuItem(Strings.MenuSize, null,
        mnuAutoSize,
        mnuAutoWidth,
        mnuAutoHeight,
        new ToolStripSeparator(),
        mnuCollapseAllSelected,
        mnuExpandAllSelected
      );

      MenuList.AddRange(ShapeContextMenu.Default.MenuItems);
      MenuList.AddRange(new ToolStripItem[] {
        mnuSize,
        new ToolStripSeparator(),
        mnuEditMembers,
      });

      UpdateTexts();
    }

    public static TypeShapeContextMenu Default { get; } = new TypeShapeContextMenu();

    private void UpdateTexts()
    {
      mnuSize.Text = Strings.MenuSize;
      mnuAutoSize.Text = Strings.MenuAutoSize;
      mnuAutoWidth.Text = Strings.MenuAutoWidth;
      mnuAutoHeight.Text = Strings.MenuAutoHeight;
      mnuCollapseAllSelected.Text = Strings.MenuCollapseAllSelected;
      mnuExpandAllSelected.Text = Strings.MenuExpandAllSelected;
      mnuEditMembers.Text = Strings.MenuEditMembers;
    }

    public override void ValidateMenuItems(Diagram diagram)
    {
      base.ValidateMenuItems(diagram);
      ShapeContextMenu.Default.ValidateMenuItems(diagram);
      mnuEditMembers.Enabled = (diagram.SelectedElementCount == 1);
    }

    private void mnuAutoSize_Click(object sender, EventArgs e)
    {
      Diagram?.AutoSizeOfSelectedShapes();
    }

    private void mnuAutoWidth_Click(object sender, EventArgs e)
    {
      Diagram?.AutoWidthOfSelectedShapes();
    }

    private void mnuAutoHeight_Click(object sender, EventArgs e)
    {
      Diagram?.AutoHeightOfSelectedShapes();
    }

    private void mnuCollapseAllSelected_Click(object sender, EventArgs e)
    {
      Diagram?.CollapseAll(true);
    }

    private void mnuExpandAllSelected_Click(object sender, EventArgs e)
    {
      Diagram?.ExpandAll(true);
    }

    private void mnuEditMembers_Click(object sender, EventArgs e)
    {
      if (Diagram?.TopSelectedElement is TypeShape typeShape)
      {
        typeShape.IsActive = false;
        typeShape.EditMembers();
      }
    }
  }
}