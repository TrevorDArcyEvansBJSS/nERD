// NClass - Free class diagram editor
// Copyright (C) 2006-2009 Balazs Tihanyi
// 
// This program is free software; you can redistribute it and/or modify it under 
// the terms of the GNU General Public License as published by the Free Software 
// Foundation; either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT 
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
// FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with 
// this program; if not, write to the Free Software Foundation, Inc., 
// 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using NClass.Core;
using NClass.DiagramEditor.ClassDiagram.Shapes;
using System;

namespace NClass.DiagramEditor.ClassDiagram.Connections
{
  internal sealed class EntityRelationshipConnection : Connection
  {
    private readonly EntityRelationship _relationship;

    /// <exception cref="ArgumentNullException">
    /// <paramref name="relationship"/> is null.-or-
    /// <paramref name="startShape"/> is null.-or-
    /// <paramref name="endShape"/> is null.
    /// </exception>
    public EntityRelationshipConnection(EntityRelationship relationship, Shape startShape, Shape endShape)
      : base(relationship, startShape, endShape)
    {
      _relationship = relationship;
    }

    protected internal override Relationship Relationship
    {
      get { return _relationship; }
    }

    protected override bool IsDashed
    {
      get { return true; }
    }

    protected override bool CloneRelationship(Diagram diagram, Shape first, Shape second)
    {
      TypeBase firstType = first.Entity as TypeBase;
      TypeBase secondType = second.Entity as TypeBase;

      if (firstType != null && secondType != null)
      {
        var clone = _relationship.Clone(firstType, secondType);
        return diagram.InsertEntityRelationship(clone);
      }
      else
      {
        return false;
      }
    }
  }
}
