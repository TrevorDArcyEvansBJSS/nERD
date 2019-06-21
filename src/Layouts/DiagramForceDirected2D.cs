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

    public override BoundingBox GetBoundingBox()
    {
      BoundingBox boundingBox = new BoundingBox();
      FDGVector2 bottomLeft = FDGVector2.Identity().Multiply(BoundingBox.DefaultBB * -1.0f) as FDGVector2;
      FDGVector2 topRight = FDGVector2.Identity().Multiply(BoundingBox.DefaultBB) as FDGVector2;
      foreach (var n in Graph.Nodes.Cast<DiagramNode>())
      {
        FDGVector2 position = GetPoint(n).Position as FDGVector2;

        if (position.X < bottomLeft.X)
          bottomLeft.X = position.X;
        if (position.Y < bottomLeft.Y)
          bottomLeft.Y = position.Y;
        if (position.X > topRight.X)
          topRight.X = position.X;
        if (position.Y > topRight.Y)
          topRight.Y = position.Y;
      }

      AbstractVector padding = (topRight - bottomLeft).Multiply(BoundingBox.DefaultPadding);
      boundingBox.BottomLeftFront = bottomLeft.Subtract(padding);
      boundingBox.TopRightBack = topRight.Add(padding);

      return boundingBox;
    }
  }
}
