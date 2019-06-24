using EpForceDirectedGraph.cs;
using NClass.DiagramEditor.ClassDiagram;
using System;
using System.Linq;

namespace Layouts
{
  public sealed class DiagramForceDirected2D : ForceDirected<FDGVector2>
  {
    private readonly Diagram _diagram;

    public DiagramForceDirected2D(Diagram diagram, IGraph iGraph, float iStiffness, float iRepulsion, float iDamping) :
      base(iGraph, iStiffness, iRepulsion, iDamping)
    {
      _diagram = diagram;
    }

    protected override Particle GetParticle(INode iNode)
    {
      if (!mNodePoints.ContainsKey(iNode.Id))
      {
        var shape = _diagram.Shapes.Single(x => x.Entity.Id == Guid.Parse(iNode.Id));
        mNodePoints[iNode.Id] = new Particle(iNode.Data.InitialPosition, FDGVector2.Zero(), FDGVector2.Zero(), iNode)
        {
          Width = shape.Size.Width,
          Height = shape.Size.Height
        };
      }

      return mNodePoints[iNode.Id];
    }
  }
}
