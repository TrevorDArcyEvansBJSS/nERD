/*! 
@file ForceDirected2D.cs
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

An Interface for the ForceDirected2D Class.
*/

namespace EpForceDirectedGraph.cs
{
  public sealed class ForceDirected2D : ForceDirected<FDGVector2>
  {
    public ForceDirected2D(IGraph iGraph, float iStiffness, float iRepulsion, float iDamping) :
      base(iGraph, iStiffness, iRepulsion, iDamping)
    {
    }

    public override Point GetPoint(Node iNode)
    {
      if (!(m_nodePoints.ContainsKey(iNode.Id)))
      {
        FDGVector2 iniPosition = iNode.Data.InitialPosition as FDGVector2;
        if (iniPosition == null)
          iniPosition = FDGVector2.Random() as FDGVector2;
        m_nodePoints[iNode.Id] = new Point(iniPosition, FDGVector2.Zero(), FDGVector2.Zero(), iNode);
      }

      return m_nodePoints[iNode.Id];
    }

    public override BoundingBox GetBoundingBox()
    {
      BoundingBox boundingBox = new BoundingBox();
      FDGVector2 bottomLeft = FDGVector2.Identity().Multiply(BoundingBox.DefaultBB * -1.0f) as FDGVector2;
      FDGVector2 topRight = FDGVector2.Identity().Multiply(BoundingBox.DefaultBB) as FDGVector2;
      foreach (Node n in Graph.Nodes)
      {
        FDGVector2 position = GetPoint(n).Position as FDGVector2;

        if (position.X < bottomLeft.X)
          bottomLeft.X = position.X;
        if (position.Y < bottomLeft.Y)
          bottomLeft.Y = position.Y;
        if (position.X > topRight.X)
          topRight.X = position.X;
        if (position.Y > topRight.Y)
          topRight.Y = position.Y;
      }
      AbstractVector padding = (topRight - bottomLeft).Multiply(BoundingBox.DefaultPadding);
      boundingBox.BottomLeftFront = bottomLeft.Subtract(padding);
      boundingBox.TopRightBack = topRight.Add(padding);

      return boundingBox;
    }
  }
}
