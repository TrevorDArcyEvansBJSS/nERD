using EpForceDirectedGraph.cs;
using Layouts.Lang;
using Layouts.Properties;
using NClass.Core;
using NClass.DiagramEditor.ClassDiagram;
using NClass.GUI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Layouts
{
  public sealed class LayoutsPlugin : Plugin
  {
    private readonly ToolStripMenuItem _menuItem;

    public LayoutsPlugin(NClassEnvironment environment) :
      base(environment)
    {
      _menuItem = new ToolStripMenuItem
      {
        Text = Strings.Menu_Title,
        Image = Resources.Layouts_16,
        ToolTipText = Strings.Menu_ToolTip
      };
      _menuItem.DropDownItems.Add(Strings.ForceDirected_Menu_Title, null, DoForceDirectedLayout);
    }

    public override bool IsAvailable
    {
      get { return DocumentManager.HasDocument; }
    }

    public override ToolStripItem MenuItem
    {
      get { return _menuItem; }
    }

    private void DoForceDirectedLayout(object sender, EventArgs e)
    {
      if (!DocumentManager.HasDocument)
      {
        return;
      }

      var creator = new DiagramCreator();
      var graph = new Graph(creator);
      var diagram = (Diagram)DocumentManager.ActiveDocument;

      // add nodes
      diagram
        .Shapes
        .ToList()
        .ForEach(x =>
        {
          graph.AddNode(new DiagramNode(x.Entity.Id.ToString()));
        });

      // add edges
      diagram
        .Connections
        .ToList()
        .ForEach(x =>
        {
          var source = new DiagramNode(x.Relationship.First.Id.ToString());
          var target = new DiagramNode(x.Relationship.Second.Id.ToString());
          var edge = new DiagramEdge(x.Relationship.Id.ToString(), source, target);
          graph.AddEdge(edge);
        });

      const float Stiffness = 81.76f;
      const float Repulsion = 400000.0f;
      const float Damping = 0.5f;
      var physics = new DiagramForceDirected2D(graph, Stiffness, Repulsion, Damping);

      const int MaxIterations = 10000;
      foreach (var _ in Enumerable.Range(0, MaxIterations))
      {
        physics.Calculate(0.05f);
      }

      // update diagram
      physics.EachNode(delegate (INode node, Point pos)
      {
        var nodeId = Guid.Parse(node.Id);
        var shape = diagram.Shapes.Single(x => x.Entity.Id == nodeId);
        shape.Location = new System.Drawing.Point((int)pos.Position.X, (int)pos.Position.Y);
      });
      physics.EachEdge(delegate (IEdge edge, Spring spring)
      {
        var connId = Guid.Parse(edge.Id);
        var conn = diagram.Connections.Single(x => x.Relationship.Id == connId);
        conn.AutoRoute();
      });
    }
  }
}
