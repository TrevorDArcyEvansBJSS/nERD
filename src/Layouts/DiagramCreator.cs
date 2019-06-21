using EpForceDirectedGraph.cs;

namespace Layouts
{
  public sealed class DiagramCreator : ICreator
  {
    public IEdge CreateEdge(string iId, INode iSource, INode iTarget, EdgeData iData = null)
    {
      return new DiagramEdge(iId, iSource, iTarget, iData);
    }

    public INode CreateNode(string iId, NodeData iData = null)
    {
      return new DiagramNode(iId, iData);
    }
  }
}
