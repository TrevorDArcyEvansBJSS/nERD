/*! 
@file Point.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/epForceDirectedGraph.cs>
@date August 08, 2013
@brief Point Interface
@version 1.0

@section LICENSE

The MIT License (MIT)

Copyright (c) 2013 Woong Gyu La <juhgiyo@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

@section DESCRIPTION

An Interface for the Point Class.
*/

namespace EpForceDirectedGraph.cs
{
  public sealed class Particle
  {
    public Particle(AbstractVector iPosition, AbstractVector iVelocity, AbstractVector iAcceleration, INode iNode)
    {
      Position = iPosition;
      Node = iNode;
      Velocity = iVelocity;
      Acceleration = iAcceleration;
    }

    public override int GetHashCode()
    {
      return Position.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      Particle p = obj as Particle;
      if ((System.Object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return Position == p.Position;
    }

    public bool Equals(Particle p)
    {
      // If parameter is null return false:
      if ((object)p == null)
      {
        return false;
      }

      // Return true if the fields match:
      return Position == p.Position;
    }

    public static bool operator ==(Particle a, Particle b)
    {
      // If both are null, or both are same instance, return true.
      if (System.Object.ReferenceEquals(a, b))
      {
        return true;
      }

      // If one is null, but not both, return false.
      if (((object)a == null) || ((object)b == null))
      {
        return false;
      }

      // Return true if the fields match:
      return (a.Position == b.Position);
    }

    public static bool operator !=(Particle a, Particle b)
    {
      return !(a == b);
    }

    public void ApplyForce(AbstractVector force)
    {
      Acceleration.Add(force / Mass);
    }

    public AbstractVector Position { get; set; }
    public INode Node { get; private set; }

    public float Mass
    {
      get
      {
        return Node.Data.Mass;
      }
      private set
      {
        Node.Data.Mass = value;
      }
    }

    public AbstractVector Velocity { get; private set; }
    public AbstractVector Acceleration { get; private set; }
    public RectF BoundingBox
    {
      get
      {
        return new RectF(
          Position.X,
          Position.Y,
          Position.X + Width,
          Position.Y + Height);
      }
    }
    public float Width { get; set; } = 10f;
    public float Height { get; set; } = 10f;
  }
}
