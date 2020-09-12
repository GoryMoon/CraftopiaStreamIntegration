using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable IdentifierTypo

namespace CraftopiaPatcher
{
    public class CraftopiaPatcher
    {
        public static IEnumerable<string> TargetDLLs { get; } = new[] {"Assembly-CSharp.dll"};

        public static void Patch(ref AssemblyDefinition assembly)
        {
            try
            {
                var module = assembly.MainModule;
                
                var emType = module.Types.First(definition => definition.Name == "OcEm");
                var customNameField = new FieldDefinition("_customName", FieldAttributes.Private, module.TypeSystem.String);
                emType.Fields.Add(customNameField);
                var customNameProperty = customNameField.EmitCreatePropertyForField("CustomName");

                var headerType = module.Types.First(definition => definition.Name == "OcEnemyHeader");
                var nameProperty = headerType.Methods.First(prop => prop.Name == "GetNameString");
                var ownerField = headerType.Fields.First(f => f.Name == "_owner");
                
                var first = nameProperty.Body.Instructions.First(i => i.OpCode == OpCodes.Box).Next;
                var formatMethod = nameProperty.Body.Instructions.Last(i => i.OpCode == OpCodes.Call);

                var processor = nameProperty.Body.GetILProcessor();
                processor.InsertBefore(first, processor.Create(OpCodes.Ldarg_0));
                processor.InsertBefore(first, processor.Create(OpCodes.Ldfld, ownerField));
                processor.InsertBefore(first, processor.Create(OpCodes.Callvirt, customNameProperty.GetMethod));
                processor.InsertBefore(first, processor.Create(OpCodes.Dup));
                processor.InsertBefore(first, processor.Create(OpCodes.Brtrue_S, formatMethod));
                processor.InsertBefore(first, processor.Create(OpCodes.Pop));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}