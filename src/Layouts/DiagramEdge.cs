using EpForceDirectedGraph.cs;

namespace Layouts
{
  public sealed class DiagramEdge : IEdge
  {
    public string Id { get; private set; }
    public EdgeData Data { get; private set; }
    public INode Source { get; private set; }
    public INode Target { get; private set; }
    public bool Directed { get; set; }

    public DiagramEdge(string iId, INode iSource, INode iTarget, EdgeData iData = null)
    {
      Id = iId;
      Source = iSource;
      Target = iTarget;
      Data = (iData != null) ? iData : new EdgeData();
      Directed = false;
    }

    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      IEdge p = obj as IEdge;
      if (p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return (Id == p.Id);
    }
  }
}
