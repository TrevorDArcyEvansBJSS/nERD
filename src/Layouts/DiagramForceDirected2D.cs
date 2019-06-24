using EpForceDirectedGraph.cs;
using NClass.DiagramEditor.ClassDiagram;

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

    protected override Point GetPoint(INode iNode)
    {
      if (!mNodePoints.ContainsKey(iNode.Id))
      {
        mNodePoints[iNode.Id] = new Point(iNode.Data.InitialPosition, FDGVector2.Zero(), FDGVector2.Zero(), iNode);
      }

      return mNodePoints[iNode.Id];
    }
  }
}
