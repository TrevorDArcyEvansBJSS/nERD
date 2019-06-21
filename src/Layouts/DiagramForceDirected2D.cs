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
      if (!m_nodePoints.ContainsKey(iNode.Id))
      {
        m_nodePoints[iNode.Id] = new Point(iNode.Data.InitialPosition, FDGVector2.Zero(), FDGVector2.Zero(), iNode);
      }

      return m_nodePoints[iNode.Id];
    }
  }
}
