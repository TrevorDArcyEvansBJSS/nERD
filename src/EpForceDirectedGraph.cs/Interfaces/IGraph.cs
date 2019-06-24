/*! 
@file IGraph.cs
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

An Interface for the IGraph.
*/

using System;
using System.Collections.Generic;

namespace EpForceDirectedGraph.cs
{
  public interface IGraph
  {
    void Clear();
    INode AddNode(INode iNode);
    IEdge AddEdge(IEdge iEdge);
    void CreateNodes(IEnumerable<NodeData> iDataList);
    void CreateNodes(IEnumerable<string> iNameList);
    void CreateEdges(IEnumerable<Tuple<string, string, EdgeData>> iDataList);
    void CreateEdges(IEnumerable<Tuple<string, string>> iDataList);
    INode CreateNode(NodeData data);
    INode CreateNode(string name);
    IEdge CreateEdge(INode iSource, INode iTarget, EdgeData iData = null);
    IEdge CreateEdge(string iSource, string iTarget, EdgeData iData = null);
    IEnumerable<IEdge> GetEdges(INode iNode1, INode iNode2);
    void RemoveNode(INode iNode);
    void DetachNode(INode iNode);
    void RemoveEdge(IEdge iEdge);
    void Merge(IGraph iMergeGraph);
    void FilterNodes(Predicate<INode> match);
    void FilterEdges(Predicate<IEdge> match);
    void AddGraphListener(IGraphEventListener iListener);

    IEnumerable<INode> Nodes { get; }
    IEnumerable<IEdge> Edges { get; }
  }
}
