﻿using EpForceDirectedGraph.cs;
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

      var graph = new Graph();
      var diagram = (Diagram)DocumentManager.ActiveDocument;

      // add nodes
      diagram
        .Shapes
        .ToList()
        .ForEach(x =>
        {
          graph.AddNode(new Node(x.Entity.Name));
        });

      // add edges
      diagram
        .Connections
        .ToList()
        .ForEach(x =>
        {
          graph.CreateEdge(x.Relationship.First.Name, x.Relationship.Second.Name);
        });

      var physics = new ForceDirected2D(graph, 81.76f, 40000.0f, 0.5f);
      var renderer = new NullRenderer(physics);

      const int MaxIterations = 10000;
      foreach (var _ in Enumerable.Range(0, MaxIterations))
      {
        renderer.Draw(0.05f);
      }

      // update diagram
      diagram
        .Shapes
        .ToList()
        .ForEach(x =>
        {
          var newPos = renderer.PositionById(x.Entity.Name);
          x.Location = new System.Drawing.Point((int)newPos.X, (int)newPos.Y);
        });
    }
  }
}
