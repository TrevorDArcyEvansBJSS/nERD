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
using NClass.Translations;
using System;
using System.Windows.Forms;

namespace NClass.DiagramEditor.ClassDiagram.ContextMenus
{
  internal sealed class StateShapeContextMenu : DiagramContextMenu
  {
    private readonly ToolStripMenuItem mnuSize;
    private readonly ToolStripMenuItem mnuAutoSize;
    private readonly ToolStripMenuItem mnuAutoWidth;
    private readonly ToolStripMenuItem mnuAutoHeight;

    private readonly ToolStripMenuItem mnuStage;
    private readonly ToolStripMenuItem mnuStageStart;
    private readonly ToolStripMenuItem mnuStageIntermediate;
    private readonly ToolStripMenuItem mnuStageEnd;

    private StateShapeContextMenu()
    {
      mnuAutoSize = new ToolStripMenuItem(Strings.MenuAutoSize, null, mnuAutoSize_Click);
      mnuAutoWidth = new ToolStripMenuItem(Strings.MenuAutoWidth, null, mnuAutoWidth_Click);
      mnuAutoHeight = new ToolStripMenuItem(Strings.MenuAutoHeight, null, mnuAutoHeight_Click);
      mnuSize = new ToolStripMenuItem(Strings.MenuSize, null,
        mnuAutoSize,
        mnuAutoWidth,
        mnuAutoHeight
      );

      mnuStageStart = new ToolStripMenuItem(Strings.StageStart, null, mnuStageStart_Click);
      mnuStageIntermediate = new ToolStripMenuItem(Strings.StageIntermediate, null, mnuStageIntermediate_Click);
      mnuStageEnd = new ToolStripMenuItem(Strings.StageEnd, null, mnuStageEnd_Click);
      mnuStage = new ToolStripMenuItem(Strings.Stage, null,
        mnuStageStart,
        mnuStageIntermediate,
        mnuStageEnd);

      MenuList.AddRange(ShapeContextMenu.Default.MenuItems);
      MenuList.AddRange(new ToolStripItem[] {
        mnuSize,
        new ToolStripSeparator(),
        mnuStage
      });

      UpdateTexts();
    }

    public static StateShapeContextMenu Default { get; } = new StateShapeContextMenu();

    private void UpdateTexts()
    {
      mnuSize.Text = Strings.MenuSize;
      mnuAutoSize.Text = Strings.MenuAutoSize;
      mnuAutoWidth.Text = Strings.MenuAutoWidth;
      mnuAutoHeight.Text = Strings.MenuAutoHeight;

      mnuStage.Text = Strings.Stage;
      mnuStageStart.Text = Strings.StageStart;
      mnuStageIntermediate.Text = Strings.StageIntermediate;
      mnuStageEnd.Text = Strings.StageEnd;
    }

    public override void ValidateMenuItems(Diagram diagram)
    {
      base.ValidateMenuItems(diagram);
      ShapeContextMenu.Default.ValidateMenuItems(diagram);

      if (Diagram?.TopSelectedElement is StateShape stateShape)
      {
        mnuStageStart.Checked = stateShape.State.Stage == Core.Stage.Start;
        mnuStageIntermediate.Checked = stateShape.State.Stage == Core.Stage.Intermediate;
        mnuStageEnd.Checked = stateShape.State.Stage == Core.Stage.End;
      }
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

    private void mnuStageStart_Click(object sender, EventArgs e)
    {
      UpdateStage(Core.Stage.Start);
    }

    private void mnuStageIntermediate_Click(object sender, EventArgs e)
    {
      UpdateStage(Core.Stage.Intermediate);
    }

    private void mnuStageEnd_Click(object sender, EventArgs e)
    {
      UpdateStage(Core.Stage.End);
    }

    private void UpdateStage(Core.Stage newStage)
    {
      if (Diagram?.TopSelectedElement is StateShape stateShape)
      {
        stateShape.IsActive = false;
        stateShape.State.Stage = newStage;
      }
    }
  }
}