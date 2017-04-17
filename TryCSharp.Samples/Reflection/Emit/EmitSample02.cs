using System;
using System.Reflection;
using System.Reflection.Emit;
using TryCSharp.Common;

namespace TryCSharp.Samples.Reflection.Emit
{
    /// <summary>
    ///     Emitのサンプル２です。
    /// </summary>
    /// <remarks>
    ///     プロパティを持つクラスを動的生成します。
    /// </remarks>
    [Sample]
    public class EmitSample02 : IExecutable
    {
        public void Execute()
        {
            //////////////////////////////////////////////////////////////////
            //
            // プロパティ付きの型を作成.
            //
            //
            // 0.これから作成する型を格納するアセンブリ名作成.
            //
            var asmName = new AssemblyName
            {
                Name = "DynamicTypes"
            };

            //
            // 1.AssemlbyBuilderの生成
            //
            var domain = AppDomain.CurrentDomain;
            var asmBuilder = domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            //
            // 2.ModuleBuilderの生成.
            //
            var modBuilder = asmBuilder.DefineDynamicModule(asmName.Name, string.Format("{0}.dll", asmName.Name));
            //
            // 3.TypeBuilderの生成.
            //
            var typeBuilder = modBuilder.DefineType("WithPropClass", TypeAttributes.Public, typeof(object), Type.EmptyTypes);
            //
            // 4.FieldBuilderの生成.
            //
            var fieldBuilder = typeBuilder.DefineField("_message", typeof(string), FieldAttributes.Private);
            //
            // 5.PropertyBuilderの生成.
            //
            var propBuilder = typeBuilder.DefineProperty("Message", PropertyAttributes.HasDefault, typeof(string), Type.EmptyTypes);
            //
            // 6.プロパティは実際にはGetter/Setterメソッドの呼び出しとなる為、それらのメソッドを作成する必要がある。
            //   それらのメソッドに付加するメソッド属性を定義.
            //
            var propAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            //
            // 7.Getメソッドの生成.
            //
            var getterMethodBuilder = typeBuilder.DefineMethod("get_Message", propAttr, typeof(string), Type.EmptyTypes);
            //
            // 8.ILGeneratorを生成し、Getter用のILコードを設定.
            //
            var il = getterMethodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fieldBuilder);
            il.Emit(OpCodes.Ret);
            //
            // 9.Setメソッドを生成
            //
            var setterMethodBuilder = typeBuilder.DefineMethod("set_Message", propAttr, null, new[] {typeof(string)});
            //
            // 10.ILGeneratorを生成し、Setter用のILコードを設定.
            //
            il = setterMethodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, fieldBuilder);
            il.Emit(OpCodes.Ret);
            //
            // 11.PropertyBuilderにGetter/Setterを紐付ける.
            //
            propBuilder.SetGetMethod(getterMethodBuilder);
            propBuilder.SetSetMethod(setterMethodBuilder);
            //
            // 12.作成した型を取得.
            //
            var type = typeBuilder.CreateType();
            //
            // 13.型を具現化.
            //
            var withPropObj = Activator.CreateInstance(type);
            //
            // 14.実行.
            //
            var propInfo = type.GetProperty("Message");
            propInfo.SetValue(withPropObj, "HelloWorld", null);
            Output.WriteLine(propInfo.GetValue(withPropObj, null));
            //
            // 15.(option) 作成したアセンブリを保存.
            //
            asmBuilder.Save($"{asmName.Name}.dll");
        }
    }
}