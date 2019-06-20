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

namespace EpForceDirectedGraph.cs
{
  public abstract class ForceDirected<Vector> : IForceDirected where Vector : IVector
  {
    public float Stiffness { get; set; }
    public float Repulsion { get; set; }
    public float Damping { get; set; }
    public float Threshold { get; set; } = 0.01f;
    public bool WithinThreshold { get; private set; }
    public IGraph Graph { get; protected set; }

    protected readonly Dictionary<string, Point> m_nodePoints = new Dictionary<string, Point>();
    protected readonly Dictionary<string, Spring> m_edgeSprings = new Dictionary<string, Spring>();

    public ForceDirected(IGraph iGraph, float iStiffness, float iRepulsion, float iDamping)
    {
      Graph = iGraph;
      Stiffness = iStiffness;
      Repulsion = iRepulsion;
      Damping = iDamping;
    }

    public void Clear()
    {
      m_nodePoints.Clear();
      m_edgeSprings.Clear();
      Graph.Clear();
    }

    public abstract Point GetPoint(Node iNode);

    public Spring GetSpring(Edge iEdge)
    {
      if (!(m_edgeSprings.ContainsKey(iEdge.Id)))
      {
        float length = iEdge.Data.Length;
        Spring existingSpring = null;

        var fromEdges = Graph.GetEdges(iEdge.Source, iEdge.Target);
        if (fromEdges != null)
        {
          foreach (Edge e in fromEdges)
          {
            if (existingSpring == null && m_edgeSprings.ContainsKey(e.Id))
            {
              existingSpring = m_edgeSprings[e.Id];
              break;
            }
          }
        }

        if (existingSpring != null)
        {
          return new Spring(existingSpring.Point1, existingSpring.Point2, 0.0f, 0.0f);
        }

        var toEdges = Graph.GetEdges(iEdge.Target, iEdge.Source);
        if (toEdges != null)
        {
          foreach (Edge e in toEdges)
          {
            if (existingSpring == null && m_edgeSprings.ContainsKey(e.Id))
            {
              existingSpring = m_edgeSprings[e.Id];
              break;
            }
          }
        }

        if (existingSpring != null)
        {
          return new Spring(existingSpring.Point2, existingSpring.Point1, 0.0f, 0.0f);
        }

        m_edgeSprings[iEdge.Id] = new Spring(GetPoint(iEdge.Source), GetPoint(iEdge.Target), length, Stiffness);
      }

      return m_edgeSprings[iEdge.Id];
    }

    // TODO: change this for group only after node grouping
    protected void ApplyCoulombsLaw()
    {
      foreach (Node n1 in Graph.Nodes)
      {
        Point point1 = GetPoint(n1);
        foreach (Node n2 in Graph.Nodes)
        {
          Point point2 = GetPoint(n2);
          if (point1 != point2)
          {
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
              //point2.ApplyForce((direction * Repulsion) / (distance * distance * -1.0f));
              point2.ApplyForce((direction * Repulsion) / (distance * -1.0f));
            }
            else if (n2.Pinned)
            {
              //point1.ApplyForce((direction * Repulsion) / (distance * distance));
              point1.ApplyForce((direction * Repulsion) / (distance));
              point2.ApplyForce(direction * 0.0f);
            }
            else
            {
              //                             point1.ApplyForce((direction * Repulsion) / (distance * distance * 0.5f));
              //                             point2.ApplyForce((direction * Repulsion) / (distance * distance * -0.5f));
              point1.ApplyForce((direction * Repulsion) / (distance * 0.5f));
              point2.ApplyForce((direction * Repulsion) / (distance * -0.5f));
            }

          }
        }
      }
    }

    protected void ApplyHookesLaw()
    {
      foreach (Edge e in Graph.Edges)
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

    protected void AttractToCentre()
    {
      foreach (Node n in Graph.Nodes)
      {
        Point point = GetPoint(n);
        if (!point.Node.Pinned)
        {
          AbstractVector direction = point.Position * -1.0f;
          //point.ApplyForce(direction * ((float)Math.Sqrt((double)(Repulsion / 100.0f))));

          float displacement = direction.Magnitude();
          direction = direction.Normalize();
          point.ApplyForce(direction * (Stiffness * displacement * 0.4f));
        }
      }
    }

    protected void UpdateVelocity(float iTimeStep)
    {
      foreach (Node n in Graph.Nodes)
      {
        Point point = GetPoint(n);
        point.Velocity.Add(point.Acceleration * iTimeStep);
        point.Velocity.Multiply(Damping);
        point.Acceleration.SetZero();
      }
    }

    protected void UpdatePosition(float iTimeStep)
    {
      foreach (Node n in Graph.Nodes)
      {
        Point point = GetPoint(n);
        point.Position.Add(point.Velocity * iTimeStep);
      }
    }

    protected float GetTotalEnergy()
    {
      float energy = 0.0f;
      foreach (Node n in Graph.Nodes)
      {
        Point point = GetPoint(n);
        float speed = point.Velocity.Magnitude();
        energy += 0.5f * point.Mass * speed * speed;
      }

      return energy;
    }

    public void Calculate(float iTimeStep) // time in second
    {
      ApplyCoulombsLaw();
      ApplyHookesLaw();
      AttractToCentre();
      UpdateVelocity(iTimeStep);
      UpdatePosition(iTimeStep);
      if (GetTotalEnergy() < Threshold)
      {
        WithinThreshold = true;
      }
      else
        WithinThreshold = false;
    }

    public void EachEdge(EdgeAction del)
    {
      foreach (Edge e in Graph.Edges)
      {
        del(e, GetSpring(e));
      }
    }

    public void EachNode(NodeAction del)
    {
      foreach (Node n in Graph.Nodes)
      {
        del(n, GetPoint(n));
      }
    }

    public NearestPoint Nearest(AbstractVector position)
    {
      NearestPoint min = new NearestPoint();
      foreach (Node n in Graph.Nodes)
      {
        Point point = GetPoint(n);
        float distance = (point.Position - position).Magnitude();
        if (min.Distance == null || distance < min.Distance)
        {
          min.Node = n;
          min.Point = point;
          min.Distance = distance;
        }
      }
      return min;
    }

    public abstract BoundingBox GetBoundingBox();
  }
}
