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
using NClass.Translations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NClass.DiagramEditor.ClassDiagram
{
  public sealed partial class DiagramDynamicMenu : DynamicMenu
  {
    private readonly ToolStripMenuItem[] _menuItems;
    private Diagram _diagram = null;

    private DiagramDynamicMenu()
    {
      InitializeComponent();
      UpdateTexts();

      _menuItems = new ToolStripMenuItem[2] { mnuDiagram, mnuFormat };
    }

    public static DiagramDynamicMenu Default { get; } = new DiagramDynamicMenu();

    public override IEnumerable<ToolStripMenuItem> GetMenuItems()
    {
      return _menuItems;
    }

    public override ToolStrip GetToolStrip()
    {
      return elementsToolStrip;
    }

    public override void SetReference(IDocument document)
    {
      if (_diagram != null)
      {
        _diagram.SelectionChanged -= new EventHandler(Diagram_SelectionChanged);
      }

      if (document == null)
      {
        _diagram = null;
      }
      else
      {
        _diagram = document as Diagram;
        _diagram.SelectionChanged += new EventHandler(Diagram_SelectionChanged);

        mnuNewInterface.Visible = toolNewInterface.Visible = _diagram.Language.SupportsInterfaces;
        mnuNewStructure.Visible = toolNewStructure.Visible = _diagram.Language.SupportsStructures;
        mnuNewDelegate.Visible = toolNewDelegate.Visible = _diagram.Language.SupportsDelegates;
        mnuNewEnum.Visible = toolNewEnum.Visible = _diagram.Language.SupportsEnums;

        toolDelete.Enabled = _diagram.HasSelectedElement;
      }
    }

    private void Diagram_SelectionChanged(object sender, EventArgs e)
    {
      toolDelete.Enabled = (_diagram != null && _diagram.HasSelectedElement);
    }

    private void UpdateTexts()
    {
      #region Diagram menu
      mnuDiagram.Text = Strings.MenuDiagram;
      mnuAddNewElement.Text = Strings.MenuNew;
      mnuNewClass.Text = Strings.MenuClass;
      mnuNewStructure.Text = Strings.MenuStruct;
      mnuNewInterface.Text = Strings.MenuInterface;
      mnuNewEnum.Text = Strings.MenuEnum;
      mnuNewDelegate.Text = Strings.MenuDelegate;
      mnuNewComment.Text = Strings.MenuComment;
      mnuNewState.Text = Strings.MenuState;
      mnuNewAssociation.Text = Strings.MenuAssociation;
      mnuNewComposition.Text = Strings.MenuComposition;
      mnuNewAggregation.Text = Strings.MenuAggregation;
      mnuNewGeneralization.Text = Strings.MenuGeneralization;
      mnuNewRealization.Text = Strings.MenuRealization;
      mnuNewDependency.Text = Strings.MenuDependency;
      mnuNewNesting.Text = Strings.MenuNesting;
      mnuNewCommentRelationship.Text = Strings.MenuCommentRelationship;
      mnuNewEntityRelationship.Text = Strings.MenuEntityRelationship;
      mnuNewTransitionRelationship.Text = Strings.MenuTransitionRelationship;
      mnuMembersFormat.Text = Strings.MenuMembersFormat;
      mnuShowType.Text = Strings.MenuType;
      mnuShowParameters.Text = Strings.MenuParameters;
      mnuShowParameterNames.Text = Strings.MenuParameterNames;
      mnuShowInitialValue.Text = Strings.MenuInitialValue;
      mnuGenerateCode.Text = Strings.MenuGenerateCode;
      mnuSaveAsImage.Text = Strings.MenuSaveAsImage;
      #endregion

      #region Format menu
      mnuFormat.Text = Strings.MenuFormat;
      mnuAlign.Text = Strings.MenuAlign;
      mnuAlignTop.Text = Strings.MenuAlignTop;
      mnuAlignLeft.Text = Strings.MenuAlignLeft;
      mnuAlignBottom.Text = Strings.MenuAlignBottom;
      mnuAlignRight.Text = Strings.MenuAlignRight;
      mnuAlignHorizontal.Text = Strings.MenuAlignHorizontal;
      mnuAlignVertical.Text = Strings.MenuAlignVertical;
      mnuMakeSameSize.Text = Strings.MenuMakeSameSize;
      mnuSameWidth.Text = Strings.MenuSameWidth;
      mnuSameHeight.Text = Strings.MenuSameHeight;
      mnuSameSize.Text = Strings.MenuSameSize;
      mnuAutoSize.Text = Strings.MenuAutoSize;
      mnuAutoWidth.Text = Strings.MenuAutoWidth;
      mnuAutoHeight.Text = Strings.MenuAutoHeight;
      mnuCollapseAll.Text = Strings.MenuCollapseAll;
      mnuExpandAll.Text = Strings.MenuExpandAll;
      #endregion

      #region Toolbar
      toolNewClass.Text = Strings.AddNewClass;
      toolNewStructure.Text = Strings.AddNewStructure;
      toolNewInterface.Text = Strings.AddNewInterface;
      toolNewEnum.Text = Strings.AddNewEnum;
      toolNewDelegate.Text = Strings.AddNewDelegate;
      toolNewComment.Text = Strings.AddNewComment;
      toolNewState.Text = Strings.AddNewState;
      toolNewAssociation.Text = Strings.AddNewAssociation;
      toolNewComposition.Text = Strings.AddNewComposition;
      toolNewAggregation.Text = Strings.AddNewAggregation;
      toolNewGeneralization.Text = Strings.AddNewGeneralization;
      toolNewRealization.Text = Strings.AddNewRealization;
      toolNewDependency.Text = Strings.AddNewDependency;
      toolNewNesting.Text = Strings.AddNewNesting;
      toolNewCommentRelationship.Text = Strings.AddNewCommentRelationship;
      toolNewEntityRelationship.Text = Strings.AddNewEntityRelationship;
      toolNewTransitionRelationship.Text = Strings.AddNewTransitionRelationship;
      toolDelete.Text = Strings.DeleteSelectedItems;
      #endregion
    }

    #region Event handlers

    private void mnuDiagram_DropDownOpening(object sender, EventArgs e)
    {
      bool hasContent = (_diagram != null && !_diagram.IsEmpty);
      mnuGenerateCode.Enabled = hasContent;
      mnuSaveAsImage.Enabled = hasContent;
    }

    private void mnuNewClass_Click(object sender, EventArgs e)
    {
      _diagram?.CreateShape(EntityType.Class);
    }

    private void mnuNewStructure_Click(object sender, EventArgs e)
    {
      _diagram?.CreateShape(EntityType.Structure);
    }

    private void mnuNewInterface_Click(object sender, EventArgs e)
    {
      _diagram?.CreateShape(EntityType.Interface);
    }

    private void mnuNewEnum_Click(object sender, EventArgs e)
    {
      _diagram?.CreateShape(EntityType.Enum);
    }

    private void mnuNewDelegate_Click(object sender, EventArgs e)
    {
      _diagram?.CreateShape(EntityType.Delegate);
    }

    private void mnuNewComment_Click(object sender, EventArgs e)
    {
      _diagram?.CreateShape(EntityType.Comment);
    }

    private void mnuNewState_Click(object sender, EventArgs e)
    {
      _diagram?.CreateShape(EntityType.State);
    }

    private void mnuNewAssociation_Click(object sender, EventArgs e)
    {
      _diagram.CreateConnection(RelationshipType.Association);
    }

    private void mnuNewComposition_Click(object sender, EventArgs e)
    {
      _diagram.CreateConnection(RelationshipType.Composition);
    }

    private void mnuNewAggregation_Click(object sender, EventArgs e)
    {
      _diagram.CreateConnection(RelationshipType.Aggregation);
    }

    private void mnuNewGeneralization_Click(object sender, EventArgs e)
    {
      _diagram.CreateConnection(RelationshipType.Generalization);
    }

    private void mnuNewRealization_Click(object sender, EventArgs e)
    {
      _diagram.CreateConnection(RelationshipType.Realization);
    }

    private void mnuNewDependency_Click(object sender, EventArgs e)
    {
      _diagram.CreateConnection(RelationshipType.Dependency);
    }

    private void mnuNewNesting_Click(object sender, EventArgs e)
    {
      _diagram.CreateConnection(RelationshipType.Nesting);
    }

    private void mnuNewCommentRelationship_Click(object sender, EventArgs e)
    {
      _diagram.CreateConnection(RelationshipType.Comment);
    }

    private void mnuNewEntityRelationship_Click(object sender, EventArgs e)
    {
      _diagram.CreateConnection(RelationshipType.EntityRelationship);
    }

    private void mnuNewTransitionRelationship_Click(object sender, EventArgs e)
    {
      _diagram.CreateConnection(RelationshipType.Transition);
    }

    private void mnuMembersFormat_DropDownOpening(object sender, EventArgs e)
    {
      mnuShowType.Checked = Settings.Default.ShowType;
      mnuShowParameters.Checked = Settings.Default.ShowParameters;
      mnuShowParameterNames.Checked = Settings.Default.ShowParameterNames;
      mnuShowInitialValue.Checked = Settings.Default.ShowInitialValue;
    }

    private void mnuShowType_Click(object sender, EventArgs e)
    {
      Settings.Default.ShowType = ((ToolStripMenuItem)sender).Checked;
      _diagram?.Redraw();
    }

    private void mnuShowParameters_Click(object sender, EventArgs e)
    {
      Settings.Default.ShowParameters = ((ToolStripMenuItem)sender).Checked;
      _diagram?.Redraw();
    }

    private void mnuShowParameterNames_Click(object sender, EventArgs e)
    {
      Settings.Default.ShowParameterNames = ((ToolStripMenuItem)sender).Checked;
      _diagram?.Redraw();
    }

    private void mnuShowInitialValue_Click(object sender, EventArgs e)
    {
      Settings.Default.ShowInitialValue = ((ToolStripMenuItem)sender).Checked;
      _diagram?.Redraw();
    }

    private void mnuGenerateCode_Click(object sender, EventArgs e)
    {
      if (_diagram?.Project != null)
      {
        using (CodeGenerator.Dialog dialog = new CodeGenerator.Dialog())
        {
          try
          {
            dialog.ShowDialog(_diagram.Project);
          }
          catch (Exception ex)
          {
            MessageBox.Show(ex.Message, Strings.UnknownError,
              MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
      }
    }

    private void mnuSaveAsImage_Click(object sender, EventArgs e)
    {
      if (!_diagram?.IsEmpty ?? false)
        _diagram.SaveAsImage();
    }

    private void mnuFormat_DropDownOpening(object sender, EventArgs e)
    {
      bool shapeSelected = (_diagram != null && _diagram.SelectedShapeCount >= 1);
      bool multiselection = (_diagram != null && _diagram.SelectedShapeCount >= 2);

      mnuAutoWidth.Enabled = shapeSelected;
      mnuAutoHeight.Enabled = shapeSelected;
      mnuAlign.Enabled = multiselection;
      mnuMakeSameSize.Enabled = multiselection;
    }

    private void mnuAlignTop_Click(object sender, EventArgs e)
    {
      _diagram?.AlignTop();
    }

    private void mnuAlignLeft_Click(object sender, EventArgs e)
    {
      _diagram?.AlignLeft();
    }

    private void mnuAlignBottom_Click(object sender, EventArgs e)
    {
      _diagram?.AlignBottom();
    }

    private void mnuAlignRight_Click(object sender, EventArgs e)
    {
      _diagram?.AlignRight();
    }

    private void mnuAlignHorizontal_Click(object sender, EventArgs e)
    {
      _diagram?.AlignHorizontal();
    }

    private void mnuAlignVertical_Click(object sender, EventArgs e)
    {
      _diagram?.AlignVertical();
    }

    private void mnuSameWidth_Click(object sender, EventArgs e)
    {
      _diagram?.AdjustToSameWidth();
    }

    private void mnuSameHeight_Click(object sender, EventArgs e)
    {
      _diagram?.AdjustToSameHeight();
    }

    private void mnuSameSize_Click(object sender, EventArgs e)
    {
      _diagram?.AdjustToSameSize();
    }

    private void mnuAutoSize_Click(object sender, EventArgs e)
    {
      _diagram?.AutoSizeOfSelectedShapes();
    }

    private void mnuAutoWidth_Click(object sender, EventArgs e)
    {
      _diagram?.AutoWidthOfSelectedShapes();
    }

    private void mnuAutoHeight_Click(object sender, EventArgs e)
    {
      _diagram?.AutoHeightOfSelectedShapes();
    }

    private void mnuCollapseAll_Click(object sender, EventArgs e)
    {
      _diagram?.CollapseAll();
    }

    private void mnuExpandAll_Click(object sender, EventArgs e)
    {
      _diagram?.ExpandAll();
    }

    private void toolDelete_Click(object sender, EventArgs e)
    {
      _diagram?.DeleteSelectedElements();
    }

    #endregion
  }
}
