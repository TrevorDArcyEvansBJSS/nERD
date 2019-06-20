using Layouts.Lang;
using Layouts.Properties;
using NClass.GUI;
using System;
using System.Windows.Forms;

namespace Layouts
{
  public sealed class LayoutsPlugin : Plugin
  {
    private readonly ToolStripMenuItem _menuItem;

    public LayoutsPlugin(NClassEnvironment environment)
      : base(environment)
    {
      _menuItem = new ToolStripMenuItem
      {
        Text = Strings.Menu_Title,
        Image = Resources.Layouts_16,
        ToolTipText = Strings.Menu_ToolTip
      };
      _menuItem.Click += MenuItem_Click;
    }

    public override bool IsAvailable
    {
      get { return DocumentManager.HasDocument; }
    }

    public override ToolStripItem MenuItem
    {
      get { return _menuItem; }
    }

    private void MenuItem_Click(object sender, EventArgs e)
    {
      if(!DocumentManager.HasDocument)
      {
        return;
      }

      // TODO   Layout
    }
  }
}
