using System;
using System.Reflection;
using System.Reflection.Emit;
using TryCSharp.Common;

namespace TryCSharp.Samples.Reflection.Emit
{
    /// <summary>
    ///     Emitのサンプル３です。
    /// </summary>
    /// <remarks>
    ///     カスタム属性を持つクラスを動的生成します。
    /// </remarks>
    [Sample]
    public class EmitSample03 : IExecutable
    {
        private static readonly string ASM_NAME = "DynamicTypes";
        private static readonly string MOD_NAME = $"{ASM_NAME}.dll";

        public void Execute()
        {
            var asmName = new AssemblyName
            {
                Name = ASM_NAME
            };

            var appDomain = AppDomain.CurrentDomain;
            var asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndCollect);
            var modBuilder = asmBuilder.DefineDynamicModule(MOD_NAME);
            var typeBuilder = modBuilder.DefineType("AttrTest", TypeAttributes.Public, typeof(object), Type.EmptyTypes);

            //
            // 型に対してカスタム属性を付加する。
            // 複数の属性を付ける場合は、AddAttributeメソッド内のようにSetCustomAttributeメソッドを
            // 複数呼びます。
            //
            AddAttribute(typeBuilder, typeof(IsDynamicTypeAttribute), Type.EmptyTypes, new object[] {});
            AddAttribute(typeBuilder, typeof(CreatorAttribute), new[] {typeof(string)}, new object[] {"gsf.zero1"});

            var type = typeBuilder.CreateType();
            Activator.CreateInstance(type);

            var attrs = type.GetCustomAttributes(true);
            if (attrs.Length > 0)
            {
                foreach (var attr in attrs)
                {
                    Output.WriteLine(attr);

                    if (attr is CreatorAttribute)
                    {
                        Output.WriteLine("\tName={0}", (attr as CreatorAttribute).CreatorName);
                    }
                }
            }
        }

        private void AddAttribute(TypeBuilder typeBuilder, Type attrType, Type[] attrCtorParamTypes, object[] attrCtorParams)
        {
            var ctorInfo = attrType.GetConstructor(attrCtorParamTypes);

            if (ctorInfo != null)
            {
                var attrBuilder = new CustomAttributeBuilder(ctorInfo, attrCtorParams);
                typeBuilder.SetCustomAttribute(attrBuilder);
            }
        }

        [AttributeUsage(AttributeTargets.Class)]
        public class IsDynamicTypeAttribute : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.Class)]
        public class CreatorAttribute : Attribute
        {
            public CreatorAttribute(string name)
            {
                CreatorName = name;
            }

            public string CreatorName { get; set; }
        }
    }
}