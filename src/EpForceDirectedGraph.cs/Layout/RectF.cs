using System;

namespace EpForceDirectedGraph.cs
{
  public sealed class RectF
  {
    public readonly static RectF Empty = new RectF(0, 0, 0, 0);

    public float T, B, L, R;

    public RectF(float l, float b, float r, float t)
    {
      T = t;
      B = b;
      L = l;
      R = r;
    }

    public bool Intersects(RectF other)
    {
      return !(L > other.R || other.L > R || other.B > T || B > other.T);
    }

    public RectF Intersection(RectF other)
    {
      if (!Intersects(other))
        return Empty;

      float[] horiz = { L, R, other.L, other.R };
      Array.Sort(horiz);
      float[] vert = { B, T, other.B, other.T };
      Array.Sort(vert);

      return new RectF(horiz[1], vert[1], horiz[2], vert[2]);
    }

    public float Area()
    {
      return (T - B) * (R - L);
    }
  }
}
