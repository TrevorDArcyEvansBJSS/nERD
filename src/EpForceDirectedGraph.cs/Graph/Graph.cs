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
    public IEnumerable<INode> Nodes => mNodes;
    public IEnumerable<IEdge> Edges => mEdges;

    private readonly List<INode> mNodes = new List<INode>();
    private readonly List<IEdge> mEdges = new List<IEdge>();

    private readonly Dictionary<string, INode> m_nodeSet = new Dictionary<string, INode>();
    private readonly Dictionary<string, Dictionary<string, List<IEdge>>> m_adjacencySet= new Dictionary<string, Dictionary<string, List<IEdge>>>();
    private readonly List<IGraphEventListener> m_eventListeners= new List<IGraphEventListener>();
    private readonly ICreator mCreator;

    private int m_nextNodeId = 0;
    private int m_nextEdgeId = 0;

    public Graph(ICreator creator)
    {
      mCreator = creator;
    }

    public void Clear()
    {
      mNodes.Clear();
      mEdges.Clear();
      m_adjacencySet.Clear();
    }

    public INode AddNode(INode iNode)
    {
      if (!m_nodeSet.ContainsKey(iNode.Id))
      {
        mNodes.Add(iNode);
      }

      m_nodeSet[iNode.Id] = iNode;

      Notify();

      return iNode;
    }

    public IEdge AddEdge(IEdge iEdge)
    {
      if (!mEdges.Contains(iEdge))
        mEdges.Add(iEdge);

      if (!(m_adjacencySet.ContainsKey(iEdge.Source.Id)))
      {
        m_adjacencySet[iEdge.Source.Id] = new Dictionary<string, List<IEdge>>();
      }
      if (!(m_adjacencySet[iEdge.Source.Id].ContainsKey(iEdge.Target.Id)))
      {
        m_adjacencySet[iEdge.Source.Id][iEdge.Target.Id] = new List<IEdge>();
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

    public void CreateEdges(IEnumerable<Tuple<string, string, EdgeData>> iDataList)
    {
      foreach (var listTrav in iDataList)
      {
        if (!m_nodeSet.ContainsKey(listTrav.Item1))
          return;
        if (!m_nodeSet.ContainsKey(listTrav.Item2))
          return;
        var node1 = m_nodeSet[listTrav.Item1];
        var node2 = m_nodeSet[listTrav.Item2];
        CreateEdge(node1, node2, listTrav.Item3);
      }
    }

    public void CreateEdges(IEnumerable<Tuple<string, string>> iDataList)
    {
      foreach (var listTrav in iDataList)
      {
        if (!m_nodeSet.ContainsKey(listTrav.Item1))
          return;
        if (!m_nodeSet.ContainsKey(listTrav.Item2))
          return;

        var node1 = m_nodeSet[listTrav.Item1];
        var node2 = m_nodeSet[listTrav.Item2];
        CreateEdge(node1, node2);
      }
    }

    public INode CreateNode(NodeData data)
    {
      var tNewNode = mCreator.CreateNode(m_nextNodeId.ToString(), data);
      m_nextNodeId++;
      AddNode(tNewNode);

      return tNewNode;
    }

    public INode CreateNode(string label)
    {
      NodeData data = new NodeData();
      data.Label = label;
      var tNewNode = mCreator.CreateNode(m_nextNodeId.ToString(), data);
      m_nextNodeId++;
      AddNode(tNewNode);

      return tNewNode;
    }

    public IEdge CreateEdge(INode iSource, INode iTarget, EdgeData iData = null)
    {
      if (iSource == null || iTarget == null)
        return null;

      var tNewEdge = mCreator.CreateEdge(m_nextEdgeId.ToString(), iSource, iTarget, iData);
      m_nextEdgeId++;
      AddEdge(tNewEdge);

      return tNewEdge;
    }

    public IEdge CreateEdge(string iSource, string iTarget, EdgeData iData = null)
    {
      if (!m_nodeSet.ContainsKey(iSource))
        return null;
      if (!m_nodeSet.ContainsKey(iTarget))
        return null;

      var node1 = m_nodeSet[iSource];
      var node2 = m_nodeSet[iTarget];

      return CreateEdge(node1, node2, iData);
    }

    public IEnumerable<IEdge> GetEdges(INode iNode1, INode iNode2)
    {
      if (m_adjacencySet.ContainsKey(iNode1.Id) && m_adjacencySet[iNode1.Id].ContainsKey(iNode2.Id))
      {
        return m_adjacencySet[iNode1.Id][iNode2.Id];
      }
      return Enumerable.Empty<IEdge>();
    }

    public IEnumerable<IEdge> GetEdges(INode iNode)
    {
      var retEdgeList = new List<IEdge>();
      if (m_adjacencySet.ContainsKey(iNode.Id))
      {
        foreach (var keyPair in m_adjacencySet[iNode.Id])
        {
          foreach (var e in keyPair.Value)
          {
            retEdgeList.Add(e);
          }
        }
      }

      foreach (var keyValuePair in m_adjacencySet)
      {
        if (keyValuePair.Key != iNode.Id)
        {
          foreach (var keyPair in m_adjacencySet[keyValuePair.Key])
          {
            foreach (var e in keyPair.Value)
            {
              retEdgeList.Add(e);
            }
          }
        }
      }

      return retEdgeList.AsEnumerable();
    }

    public void RemoveNode(INode iNode)
    {
      if (m_nodeSet.ContainsKey(iNode.Id))
      {
        m_nodeSet.Remove(iNode.Id);
      }
      mNodes.Remove(iNode);
      DetachNode(iNode);
    }

    public void DetachNode(INode iNode)
    {
      mEdges.ForEach(delegate (IEdge e)
      {
        if (e.Source.Id == iNode.Id || e.Target.Id == iNode.Id)
        {
          RemoveEdge(e);
        }
      });

      Notify();
    }

    public void RemoveEdge(IEdge iEdge)
    {
      mEdges.Remove(iEdge);
      foreach (var x in m_adjacencySet)
      {
        foreach (var y in x.Value)
        {
          List<IEdge> tEdges = y.Value;
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

    public INode GetNode(string label)
    {
      INode retNode = null;
      mNodes.ForEach(delegate (INode n)
      {
        if (n.Data.Label == label)
        {
          retNode = n;
        }
      });
      return retNode;
    }

    public IEdge GetEdge(string label)
    {
      IEdge retEdge = null;
      mEdges.ForEach(delegate (IEdge e)
      {
        if (e.Data.Label == label)
        {
          retEdge = e;
        }
      });
      return retEdge;
    }

    public void Merge(IGraph iMergeGraph)
    {
      foreach (var n in iMergeGraph.Nodes)
      {
        var mergeNode = mCreator.CreateNode(m_nextNodeId.ToString(), n.Data);
        AddNode(mergeNode);
        m_nextNodeId++;
        mergeNode.Data.OrigID = n.Id;
      }

      foreach (var e in iMergeGraph.Edges)
      {
        var fromNode = mNodes.Find(delegate (INode n)
        {
          if (e.Source.Id == n.Data.OrigID)
          {
            return true;
          }
          return false;
        });

        var toNode = mNodes.Find(delegate (INode n)
        {
          if (e.Target.Id == n.Data.OrigID)
          {
            return true;
          }
          return false;
        });

        var tNewEdge = AddEdge(mCreator.CreateEdge(m_nextEdgeId.ToString(), fromNode, toNode, e.Data));
        m_nextEdgeId++;
      }
    }

    public void FilterNodes(Predicate<INode> match)
    {
      foreach (var n in mNodes)
      {
        if (!match(n))
          RemoveNode(n);
      }
    }

    public void FilterEdges(Predicate<IEdge> match)
    {
      foreach (var e in mEdges)
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
