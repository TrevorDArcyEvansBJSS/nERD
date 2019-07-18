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

using NClass.Translations;

namespace NClass.Core
{
  public sealed class DependencyRelationship : TypeRelationship
  {
    internal DependencyRelationship(TypeBase first, TypeBase second) :
      base(first, second)
    {
      RelationshipType = RelationshipType.Dependency;
      SupportsLabel = true;
      Attach();
    }

    public DependencyRelationship Clone(TypeBase first, TypeBase second)
    {
      DependencyRelationship dependency = new DependencyRelationship(first, second);
      dependency.CopyFrom(this);
      return dependency;
    }

    public override string ToString()
    {
      return string.Format("{0}: {1} --> {2}",
        Strings.Dependency, First.Name, Second.Name);
    }
  }
}
