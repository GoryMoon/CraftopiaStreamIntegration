using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace CraftopiaPatcher
{
    public static class Helper
    {
        public static PropertyDefinition EmitCreatePropertyForField(this FieldDefinition field, string name) {
            var prop = new PropertyDefinition( name, PropertyAttributes.None, field.FieldType)
            {
                HasThis = !field.IsStatic
            };

            ParameterDefinition valueArg;

            var attributes = MethodAttributes.Private |
            MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            if (field.IsStatic) attributes |= MethodAttributes.Static;

            var getMethod = new MethodDefinition("get_" + prop.Name, attributes, field.FieldType)
            {
                SemanticsAttributes = MethodSemanticsAttributes.Getter
            };
            var setMethod = new MethodDefinition("set_" + prop.Name, attributes, field.Module.TypeSystem.Void)
            {
                SemanticsAttributes = MethodSemanticsAttributes.Setter
            };
            setMethod.Parameters.Add(valueArg = new ParameterDefinition(field.FieldType));

            var getter = getMethod.Body.GetILProcessor();
            var setter = setMethod.Body.GetILProcessor();

            if(field.IsStatic)
            {
                    getter.Emit(OpCodes.Ldsfld, field);
                    getter.Emit(OpCodes.Ret);
                    
                    setter.Emit(OpCodes.Ldarg_0);
                    setter.Emit(OpCodes.Stsfld, field);
                    setter.Emit(OpCodes.Ret);
            }
            else
            {
                    getter.Emit(OpCodes.Ldarg_0);
                    getter.Emit(OpCodes.Ldfld, field);
                    getter.Emit(OpCodes.Ret);
                    
                    setter.Emit(OpCodes.Ldarg_0);
                    setter.Emit(OpCodes.Ldarg, valueArg);
                    setter.Emit(OpCodes.Stfld, field);
                    setter.Emit(OpCodes.Ret);
            }

            field.DeclaringType.Methods.Add(getMethod);
            field.DeclaringType.Methods.Add(setMethod);
            prop.GetMethod = getMethod;
            prop.SetMethod = setMethod;
            field.DeclaringType.Properties.Add(prop);

            return prop;
        }
    }
}