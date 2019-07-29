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
using NClass.CSharp;
using System;
using System.Collections.Generic;

namespace NClass.EntityRelationshipDiagram
{
  public sealed class ErdLanguage : Language
  {
    private ErdLanguage()
    {
    }

    public static ErdLanguage Instance { get; } = new ErdLanguage();

    public override string Name => "Entity Relationship Diagram";

    public override string AssemblyName => "EntityRelationshipDiagram";

    public override Dictionary<AccessModifier, string> ValidAccessModifiers => throw new NotImplementedException();

    public override Dictionary<ClassModifier, string> ValidClassModifiers => throw new NotImplementedException();

    public override Dictionary<FieldModifier, string> ValidFieldModifiers => throw new NotImplementedException();

    public override Dictionary<OperationModifier, string> ValidOperationModifiers => throw new NotImplementedException();

    public override bool SupportsAssemblyImport => false;

    public override bool SupportsInterfaces => false;

    public override bool SupportsStructures => false;

    public override bool SupportsEnums => false;

    public override bool SupportsDelegates => false;

    public override bool SupportsExplicitImplementation => false;

    public override bool SupportsStates => false;

    public override bool ExplicitVirtualMethods => false;

    public override string DefaultFileExtension => "*.erd";

    protected override string[] ReservedNames => throw new NotImplementedException();

    protected override string[] TypeKeywords => throw new NotImplementedException();

    public override string GetAccessString(AccessModifier access, bool forCode)
    {
      throw new NotImplementedException();
    }

    public override string GetClassModifierString(ClassModifier modifier, bool forCode)
    {
      throw new NotImplementedException();
    }

    public override string GetFieldModifierString(FieldModifier modifier, bool forCode)
    {
      throw new NotImplementedException();
    }

    public override string GetOperationModifierString(OperationModifier modifier, bool forCode)
    {
      throw new NotImplementedException();
    }

    public override bool IsValidModifier(FieldModifier modifier)
    {
      throw new NotImplementedException();
    }

    public override bool IsValidModifier(OperationModifier modifier)
    {
      throw new NotImplementedException();
    }

    public override bool IsValidModifier(AccessModifier modifier)
    {
      throw new NotImplementedException();
    }

    protected override ClassType CreateClass()
    {
      return new CSharpClass();
    }

    protected override DelegateType CreateDelegate()
    {
      throw new NotImplementedException();
    }

    protected override EnumType CreateEnum()
    {
      throw new NotImplementedException();
    }

    protected override InterfaceType CreateInterface()
    {
      throw new NotImplementedException();
    }

    protected override ArgumentList CreateParameterCollection()
    {
      throw new NotImplementedException();
    }

    protected override StructureType CreateStructure()
    {
      throw new NotImplementedException();
    }

    protected override Operation Implement(Operation operation, CompositeType newParent, bool explicitly)
    {
      throw new NotImplementedException();
    }

    protected override Operation Override(Operation operation, CompositeType newParent)
    {
      throw new NotImplementedException();
    }

    protected override void ValidateField(Field field)
    {
      throw new NotImplementedException();
    }

    protected override void ValidateOperation(Operation operation)
    {
      throw new NotImplementedException();
    }
  }
}
