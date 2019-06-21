using EpForceDirectedGraph.cs;
using System.Linq;

namespace Layouts
{
  public sealed class DiagramForceDirected2D : ForceDirected<FDGVector2>
  {
    public DiagramForceDirected2D(IGraph iGraph, float iStiffness, float iRepulsion, float iDamping) :
      base(iGraph, iStiffness, iRepulsion, iDamping)
    {
    }

    public override Point GetPoint(INode iNode)
    {
      if (!(m_nodePoints.ContainsKey(iNode.Id)))
      {
        FDGVector2 iniPosition = iNode.Data.InitialPosition as FDGVector2;
        if (iniPosition == null)
        {
          iniPosition = FDGVector2.Random() as FDGVector2;
        }
        m_nodePoints[iNode.Id] = new Point(iniPosition, FDGVector2.Zero(), FDGVector2.Zero(), iNode);
      }

      return m_nodePoints[iNode.Id];
    }
  }
}
