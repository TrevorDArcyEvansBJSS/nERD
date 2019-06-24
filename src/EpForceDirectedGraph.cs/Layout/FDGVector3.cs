/*! 
@file FDGVector3.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/epForceDirectedGraph.cs>
@date August 08, 2013
@brief Vector3 Interface
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

An Interface for the FDGVector3 Class.
*/

using System;

namespace EpForceDirectedGraph.cs
{
  public sealed class FDGVector3 : AbstractVector
  {
    public FDGVector3() :
      base()
    {
      X = 0.0f;
      Y = 0.0f;
      Z = 0.0f;
    }

    public FDGVector3(float iX, float iY, float iZ) :
      base()
    {
      X = iX;
      Y = iY;
      Z = iZ;
    }

    public override int GetHashCode()
    {
      return (int)X ^ (int)Y ^ (int)Z;
    }

    public override bool Equals(object obj)
    {
      // If parameter is null return false.
      if (obj == null)
      {
        return false;
      }

      // If parameter cannot be cast to Point return false.
      FDGVector3 p = obj as FDGVector3;
      if (p is null)
      {
        return false;
      }

      // Return true if the fields match:
      return (X == p.X) && (Y == p.Y) && (Z == p.Z);
    }

    public bool Equals(FDGVector3 p)
    {
      // If parameter is null return false:
      if (p is null)
      {
        return false;
      }

      // Return true if the fields match:
      return (X == p.X) && (Y == p.Y) && (Z == p.Z);
    }

    public static bool operator ==(FDGVector3 a, FDGVector3 b)
    {
      // If both are null, or both are same instance, return true.
      if (ReferenceEquals(a, b))
      {
        return true;
      }

      // If one is null, but not both, return false.
      if ((a is null) || (b is null))
      {
        return false;
      }

      // Return true if the fields match:
      return (a.X == b.X) && (a.Y == b.Y) && (a.Z == b.Z);
    }

    public static bool operator !=(FDGVector3 a, FDGVector3 b)
    {
      return !(a == b);
    }

    public override AbstractVector Add(AbstractVector v2)
    {
      FDGVector3 v32 = v2 as FDGVector3;
      X = X + v32.X;
      Y = Y + v32.Y;
      Z = Z + v32.Z;

      return this;
    }

    public override AbstractVector Subtract(AbstractVector v2)
    {
      FDGVector3 v32 = v2 as FDGVector3;
      X = X - v32.X;
      Y = Y - v32.Y;
      Z = Z - v32.Z;

      return this;
    }

    public override AbstractVector Multiply(float n)
    {
      X = X * n;
      Y = Y * n;
      Z = Z * n;

      return this;
    }

    public override AbstractVector Divide(float n)
    {
      if (n == 0.0f)
      {
        X = 0.0f;
        Y = 0.0f;
        Z = 0.0f;
      }
      else
      {
        X = X / n;
        Y = Y / n;
        Z = Z / n;
      }

      return this;
    }

    public override float Magnitude()
    {
      return (float)Math.Sqrt((double)(X * X) + (double)(Y * Y) + (double)(Z * Z));
    }

    public override AbstractVector Normalize()
    {
      return this / Magnitude();
    }

    public override AbstractVector SetZero()
    {
      X = 0.0f;
      Y = 0.0f;
      Z = 0.0f;

      return this;
    }

    public override AbstractVector SetIdentity()
    {
      X = 1.0f;
      Y = 1.0f;
      Z = 1.0f;

      return this;
    }

    public static AbstractVector Zero()
    {
      return new FDGVector3(0.0f, 0.0f, 0.0f);
    }

    public static AbstractVector Identity()
    {
      return new FDGVector3(1.0f, 1.0f, 1.0f);
    }

    public static AbstractVector Random()
    {
      return new FDGVector3(10.0f * (Util.Random() - 0.5f), 10.0f * (Util.Random() - 0.5f), 10.0f * (Util.Random() - 0.5f));
    }

    public static FDGVector3 operator +(FDGVector3 a, FDGVector3 b)
    {
      FDGVector3 temp = new FDGVector3(a.X, a.Y, a.Z);
      temp.Add(b);

      return temp;
    }

    public static FDGVector3 operator -(FDGVector3 a, FDGVector3 b)
    {
      FDGVector3 temp = new FDGVector3(a.X, a.Y, a.Z);
      temp.Subtract(b);

      return temp;
    }

    public static FDGVector3 operator *(FDGVector3 a, float b)
    {
      FDGVector3 temp = new FDGVector3(a.X, a.Y, a.Z);
      temp.Multiply(b);

      return temp;
    }

    public static FDGVector3 operator *(float a, FDGVector3 b)
    {
      FDGVector3 temp = new FDGVector3(b.X, b.Y, b.Z);
      temp.Multiply(a);

      return temp;
    }

    public static FDGVector3 operator /(FDGVector3 a, float b)
    {
      FDGVector3 temp = new FDGVector3(a.X, a.Y, a.Z);
      temp.Divide(b);

      return temp;
    }
  }
}
