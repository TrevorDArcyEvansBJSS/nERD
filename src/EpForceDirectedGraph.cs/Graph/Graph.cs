/*! 
@file Graph.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/epForceDirectedGraph.cs>
@date August 08, 2013
@brief Graph Interface
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

An Interface for the Graph Class.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace EpForceDirectedGraph.cs
{
  public sealed class Graph : IGraph
  {
    public IEnumerable<Node> Nodes => mNodes;
    public IEnumerable<Edge> Edges => mEdges;

    private readonly List<Node> mNodes = new List<Node>();
    private readonly List<Edge> mEdges = new List<Edge>();

    private readonly Dictionary<string, Node> m_nodeSet = new Dictionary<string, Node>();
    private readonly Dictionary<string, Dictionary<string, List<Edge>>> m_adjacencySet= new Dictionary<string, Dictionary<string, List<Edge>>>();
    private readonly List<IGraphEventListener> m_eventListeners= new List<IGraphEventListener>();

    private int m_nextNodeId = 0;
    private int m_nextEdgeId = 0;

    public void Clear()
    {
      mNodes.Clear();
      mEdges.Clear();
      m_adjacencySet.Clear();
    }

    public Node AddNode(Node iNode)
    {
      if (!m_nodeSet.ContainsKey(iNode.Id))
      {
        mNodes.Add(iNode);
      }

      m_nodeSet[iNode.Id] = iNode;
      Notify();

      return iNode;
    }

    public Edge AddEdge(Edge iEdge)
    {
      if (!mEdges.Contains(iEdge))
        mEdges.Add(iEdge);

      if (!(m_adjacencySet.ContainsKey(iEdge.Source.Id)))
      {
        m_adjacencySet[iEdge.Source.Id] = new Dictionary<string, List<Edge>>();
      }
      if (!(m_adjacencySet[iEdge.Source.Id].ContainsKey(iEdge.Target.Id)))
      {
        m_adjacencySet[iEdge.Source.Id][iEdge.Target.Id] = new List<Edge>();
      }

      if (!m_adjacencySet[iEdge.Source.Id][iEdge.Target.Id].Contains(iEdge))
      {
        m_adjacencySet[iEdge.Source.Id][iEdge.Target.Id].Add(iEdge);
      }

      Notify();

      return iEdge;
    }

    public void CreateNodes(IEnumerable<NodeData> iDataList)
    {
      foreach (var listTrav in iDataList)
      {
        CreateNode(listTrav);
      }
    }

    public void CreateNodes(IEnumerable<string> iNameList)
    {
      foreach (var listTrav in iNameList)
      {
        CreateNode(listTrav);
      }
    }

    public void CreateEdges(IEnumerable<Triple<string, string, EdgeData>> iDataList)
    {
      foreach (var listTrav in iDataList)
      {
        if (!m_nodeSet.ContainsKey(listTrav.First))
          return;
        if (!m_nodeSet.ContainsKey(listTrav.Second))
          return;
        Node node1 = m_nodeSet[listTrav.First];
        Node node2 = m_nodeSet[listTrav.Second];
        CreateEdge(node1, node2, listTrav.Third);
      }
    }

    public void CreateEdges(IEnumerable<Pair<string, string>> iDataList)
    {
      foreach (var listTrav in iDataList)
      {
        if (!m_nodeSet.ContainsKey(listTrav.First))
          return;
        if (!m_nodeSet.ContainsKey(listTrav.Second))
          return;
        Node node1 = m_nodeSet[listTrav.First];
        Node node2 = m_nodeSet[listTrav.Second];
        CreateEdge(node1, node2);
      }
    }

    public Node CreateNode(NodeData data)
    {
      Node tNewNode = new Node(m_nextNodeId.ToString(), data);
      m_nextNodeId++;
      AddNode(tNewNode);

      return tNewNode;
    }

    public Node CreateNode(string label)
    {
      NodeData data = new NodeData();
      data.Label = label;
      Node tNewNode = new Node(m_nextNodeId.ToString(), data);
      m_nextNodeId++;
      AddNode(tNewNode);

      return tNewNode;
    }

    public Edge CreateEdge(Node iSource, Node iTarget, EdgeData iData = null)
    {
      if (iSource == null || iTarget == null)
        return null;

      Edge tNewEdge = new Edge(m_nextEdgeId.ToString(), iSource, iTarget, iData);
      m_nextEdgeId++;
      AddEdge(tNewEdge);

      return tNewEdge;
    }

    public Edge CreateEdge(string iSource, string iTarget, EdgeData iData = null)
    {
      if (!m_nodeSet.ContainsKey(iSource))
        return null;
      if (!m_nodeSet.ContainsKey(iTarget))
        return null;

      Node node1 = m_nodeSet[iSource];
      Node node2 = m_nodeSet[iTarget];

      return CreateEdge(node1, node2, iData);
    }

    public IEnumerable<Edge> GetEdges(Node iNode1, Node iNode2)
    {
      if (m_adjacencySet.ContainsKey(iNode1.Id) && m_adjacencySet[iNode1.Id].ContainsKey(iNode2.Id))
      {
        return m_adjacencySet[iNode1.Id][iNode2.Id];
      }
      return null;
    }

    public IEnumerable<Edge> GetEdges(Node iNode)
    {
      List<Edge> retEdgeList = new List<Edge>();
      if (m_adjacencySet.ContainsKey(iNode.Id))
      {
        foreach (KeyValuePair<string, List<Edge>> keyPair in m_adjacencySet[iNode.Id])
        {
          foreach (Edge e in keyPair.Value)
          {
            retEdgeList.Add(e);
          }
        }
      }

      foreach (KeyValuePair<string, Dictionary<string, List<Edge>>> keyValuePair in m_adjacencySet)
      {
        if (keyValuePair.Key != iNode.Id)
        {
          foreach (KeyValuePair<string, List<Edge>> keyPair in m_adjacencySet[keyValuePair.Key])
          {
            foreach (Edge e in keyPair.Value)
            {
              retEdgeList.Add(e);
            }
          }

        }
      }

      return retEdgeList.AsEnumerable();
    }

    public void RemoveNode(Node iNode)
    {
      if (m_nodeSet.ContainsKey(iNode.Id))
      {
        m_nodeSet.Remove(iNode.Id);
      }
      mNodes.Remove(iNode);
      DetachNode(iNode);
    }

    public void DetachNode(Node iNode)
    {
      mEdges.ForEach(delegate (Edge e)
      {
        if (e.Source.Id == iNode.Id || e.Target.Id == iNode.Id)
        {
          RemoveEdge(e);
        }
      });

      Notify();
    }

    public void RemoveEdge(Edge iEdge)
    {
      mEdges.Remove(iEdge);
      foreach (KeyValuePair<string, Dictionary<string, List<Edge>>> x in m_adjacencySet)
      {
        foreach (KeyValuePair<string, List<Edge>> y in x.Value)
        {
          List<Edge> tEdges = y.Value;
          tEdges.Remove(iEdge);
          if (tEdges.Count == 0)
          {
            m_adjacencySet[x.Key].Remove(y.Key);
            break;
          }
        }
        if (x.Value.Count == 0)
        {
          m_adjacencySet.Remove(x.Key);
          break;
        }
      }

      Notify();
    }

    public Node GetNode(string label)
    {
      Node retNode = null;
      mNodes.ForEach(delegate (Node n)
      {
        if (n.Data.Label == label)
        {
          retNode = n;
        }
      });
      return retNode;
    }

    public Edge GetEdge(string label)
    {
      Edge retEdge = null;
      mEdges.ForEach(delegate (Edge e)
      {
        if (e.Data.Label == label)
        {
          retEdge = e;
        }
      });
      return retEdge;
    }

    public void Merge(Graph iMergeGraph)
    {
      foreach (Node n in iMergeGraph.Nodes)
      {
        Node mergeNode = new Node(m_nextNodeId.ToString(), n.Data);
        AddNode(mergeNode);
        m_nextNodeId++;
        mergeNode.Data.OrigID = n.Id;
      }

      foreach (Edge e in iMergeGraph.Edges)
      {
        Node fromNode = mNodes.Find(delegate (Node n)
        {
          if (e.Source.Id == n.Data.OrigID)
          {
            return true;
          }
          return false;
        });

        Node toNode = mNodes.Find(delegate (Node n)
        {
          if (e.Target.Id == n.Data.OrigID)
          {
            return true;
          }
          return false;
        });

        Edge tNewEdge = AddEdge(new Edge(m_nextEdgeId.ToString(), fromNode, toNode, e.Data));
        m_nextEdgeId++;
      }
    }

    public void FilterNodes(Predicate<Node> match)
    {
      foreach (Node n in mNodes)
      {
        if (!match(n))
          RemoveNode(n);
      }
    }

    public void FilterEdges(Predicate<Edge> match)
    {
      foreach (Edge e in mEdges)
      {
        if (!match(e))
          RemoveEdge(e);
      }
    }

    public void AddGraphListener(IGraphEventListener iListener)
    {
      m_eventListeners.Add(iListener);
    }

    private void Notify()
    {
      foreach (IGraphEventListener listener in m_eventListeners)
      {
        listener.GraphChanged();
      }
    }
  }
}
