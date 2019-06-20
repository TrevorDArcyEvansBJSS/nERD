using EpForceDirectedGraph.cs;
using System.Collections.Generic;
using System.Linq;

namespace Layouts
{
  public sealed class NullRenderer : AbstractRenderer
  {
    private readonly IDictionary<Node, AbstractVector> _nodePos = new Dictionary<Node, AbstractVector>();

    public NullRenderer(IForceDirected iForceDirected) :
      base(iForceDirected)
    {
    }

    public AbstractVector PositionById(string id)
    {
      return _nodePos.SingleOrDefault(x => x.Key.Id == id).Value;
    }

    public override void Clear()
    {
    }

    protected override void DrawEdge(Edge iEdge, AbstractVector iPosition1, AbstractVector iPosition2)
    {
    }

    protected override void DrawNode(Node iNode, AbstractVector iPosition)
    {
      if (_nodePos.ContainsKey(iNode))
      {
        _nodePos[iNode] = iPosition;
      }
      else
      {
        _nodePos.Add(iNode, iPosition);
      }
    }
  }
}
