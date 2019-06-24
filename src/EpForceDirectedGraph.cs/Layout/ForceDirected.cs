/*! 
@file ForceDirected.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/epForceDirectedGraph.cs>
@date August 08, 2013
@brief ForceDirected Interface
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

An Interface for the ForceDirected Class.
*/

using System.Collections.Generic;
using System.Linq;

namespace EpForceDirectedGraph.cs
{
  public abstract class ForceDirected<Vector> : IForceDirected where Vector : IVector
  {
    public float Stiffness { get; set; }
    public float Repulsion { get; set; }
    public float Damping { get; set; }
    public float Threshold { get; set; } = 0.01f;
    public IGraph Graph { get; private set; }

    protected readonly Dictionary<string, Particle> mNodePoints = new Dictionary<string, Particle>();
    protected readonly Dictionary<string, Spring> mEdgeSprings = new Dictionary<string, Spring>();

    public ForceDirected(IGraph iGraph, float iStiffness, float iRepulsion, float iDamping)
    {
      Graph = iGraph;
      Stiffness = iStiffness;
      Repulsion = iRepulsion;
      Damping = iDamping;
    }

    public void Clear()
    {
      mNodePoints.Clear();
      mEdgeSprings.Clear();
      Graph.Clear();
    }

    protected abstract Particle GetParticle(INode iNode);

    private Spring GetSpring(IEdge iEdge)
    {
      if (mEdgeSprings.ContainsKey(iEdge.Id))
      {
        return mEdgeSprings[iEdge.Id];
      }

      Spring existingSpring = null;

      var fromEdges = Graph.GetEdges(iEdge.Source, iEdge.Target);
      foreach (var e in fromEdges)
      {
        if (existingSpring == null && mEdgeSprings.ContainsKey(e.Id))
        {
          existingSpring = mEdgeSprings[e.Id];
          break;
        }
      }

      if (existingSpring != null)
      {
        return new Spring(existingSpring.Point1, existingSpring.Point2, 0.0f, 0.0f);
      }

      var toEdges = Graph.GetEdges(iEdge.Target, iEdge.Source);
      foreach (var e in toEdges)
      {
        if (existingSpring == null && mEdgeSprings.ContainsKey(e.Id))
        {
          existingSpring = mEdgeSprings[e.Id];
          break;
        }
      }

      if (existingSpring != null)
      {
        return new Spring(existingSpring.Point2, existingSpring.Point1, 0.0f, 0.0f);
      }

      float length = iEdge.Data.Length;
      mEdgeSprings[iEdge.Id] = new Spring(GetParticle(iEdge.Source), GetParticle(iEdge.Target), length, Stiffness);

      return mEdgeSprings[iEdge.Id];
    }

    // TODO: change this for group only after node grouping
    private void ApplyCoulombsLaw()
    {
      foreach (var n1 in Graph.Nodes)
      {
        Particle point1 = GetParticle(n1);
        foreach (var n2 in Graph.Nodes)
        {
          Particle point2 = GetParticle(n2);
          if (point1 == point2)
          {
            continue;
          }

          AbstractVector d = point1.Position - point2.Position;
          float distance = d.Magnitude() + 0.1f;
          AbstractVector direction = d.Normalize();
          if (n1.Pinned && n2.Pinned)
          {
            point1.ApplyForce(direction * 0.0f);
            point2.ApplyForce(direction * 0.0f);
          }
          else if (n1.Pinned)
          {
            point1.ApplyForce(direction * 0.0f);
            point2.ApplyForce((direction * Repulsion) / (distance * -1.0f));
          }
          else if (n2.Pinned)
          {
            point1.ApplyForce((direction * Repulsion) / (distance));
            point2.ApplyForce(direction * 0.0f);
          }
          else
          {
            point1.ApplyForce((direction * Repulsion) / (distance * 0.5f));
            point2.ApplyForce((direction * Repulsion) / (distance * -0.5f));
          }
        }
      }
    }

    private void ApplyHookesLaw()
    {
      foreach (var e in Graph.Edges)
      {
        Spring spring = GetSpring(e);
        AbstractVector d = spring.Point2.Position - spring.Point1.Position;
        float displacement = spring.Length - d.Magnitude();
        AbstractVector direction = d.Normalize();

        if (spring.Point1.Node.Pinned && spring.Point2.Node.Pinned)
        {
          spring.Point1.ApplyForce(direction * 0.0f);
          spring.Point2.ApplyForce(direction * 0.0f);
        }
        else if (spring.Point1.Node.Pinned)
        {
          spring.Point1.ApplyForce(direction * 0.0f);
          spring.Point2.ApplyForce(direction * (spring.K * displacement));
        }
        else if (spring.Point2.Node.Pinned)
        {
          spring.Point1.ApplyForce(direction * (spring.K * displacement * -1.0f));
          spring.Point2.ApplyForce(direction * 0.0f);
        }
        else
        {
          spring.Point1.ApplyForce(direction * (spring.K * displacement * -0.5f));
          spring.Point2.ApplyForce(direction * (spring.K * displacement * 0.5f));
        }
      }
    }

    private void AttractToCentre()
    {
      foreach (var n in Graph.Nodes)
      {
        Particle point = GetParticle(n);
        if (point.Node.Pinned)
        {
          continue;
        }

        AbstractVector direction = point.Position * -1.0f;
        float displacement = direction.Magnitude();
        direction = direction.Normalize();
        point.ApplyForce(direction * (Stiffness * displacement * 0.4f));
      }
    }

    private void UpdateVelocity(float iTimeStep)
    {
      foreach (var n in Graph.Nodes)
      {
        Particle point = GetParticle(n);
        point.Velocity.Add(point.Acceleration * iTimeStep);
        point.Velocity.Multiply(Damping);
        point.Acceleration.SetZero();
      }
    }

    private void UpdatePosition(float iTimeStep)
    {
      foreach (var n in Graph.Nodes)
      {
        Particle point = GetParticle(n);

        var otherNodes = Graph.Nodes.Except(new[] { n });
        foreach (var otherNode in otherNodes)
        {
          var otherParticle = GetParticle(otherNode);
          var otherBBox = otherParticle.BoundingBox;
          if (otherBBox.Intersects(point.BoundingBox))
          {
            point.Velocity.SetZero();
            otherParticle.Velocity.SetZero();
            otherNode.Pinned = n.Pinned = true;
          }
          else
          {
            point.Position.Add(point.Velocity * iTimeStep);
          }
        }
      }
    }

    public float TotalEnergy
    {
      get
      {
        float energy = 0.0f;
        foreach (var n in Graph.Nodes)
        {
          Particle point = GetParticle(n);
          float speed = point.Velocity.Magnitude();
          energy += 0.5f * point.Mass * speed * speed;
        }

        return energy;
      }
    }

    // time step in second
    public void Calculate(float iTimeStep)
    {
      ApplyCoulombsLaw();
      ApplyHookesLaw();
      AttractToCentre();
      UpdateVelocity(iTimeStep);
      UpdatePosition(iTimeStep);
    }

    public void EachEdge(EdgeAction del)
    {
      foreach (var e in Graph.Edges)
      {
        del(e, GetSpring(e));
      }
    }

    public void EachNode(NodeAction del)
    {
      foreach (var n in Graph.Nodes)
      {
        del(n, GetParticle(n));
      }
    }
  }
}
