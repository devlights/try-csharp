using System;
using System.Reflection;
using System.Reflection.Emit;
using TryCSharp.Common;

namespace TryCSharp.Samples.Reflection.Emit
{
    /// <summary>
    ///     Emitのサンプルです。
    /// </summary>
    /// <remarks>
    ///     HelloWorldを出力するクラスを動的生成します。
    /// </remarks>
    [Sample]
    public class EmitSample01 : IExecutable
    {
        public void Execute()
        {
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
            var asmBuilder = domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            //
            // 2.ModuleBuilderの生成.
            //
            var modBuilder = asmBuilder.DefineDynamicModule("HelloWorld");
            //
            // 3.TypeBuilderの生成.
            //
            var typeBuilder = modBuilder.DefineType("SayHelloImpl", TypeAttributes.Public, typeof(object), new[] {typeof(ISayHello)});
            //
            // 4.MethodBuilderの生成
            //
            var methodAttr = MethodAttributes.Public | MethodAttributes.Virtual;
            var methodBuilder = typeBuilder.DefineMethod("SayHello", methodAttr, typeof(void), new Type[] {});
            typeBuilder.DefineMethodOverride(methodBuilder, typeof(ISayHello).GetMethod("SayHello"));
            //
            // 5.ILGeneratorを生成し、ILコードを設定.
            //
            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldstr, "Hello World");
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {typeof(string)}));
            il.Emit(OpCodes.Ret);
            //
            // 6.作成した型を取得.
            //
            var type = typeBuilder.CreateType();
            //
            // 7.型を具現化.
            //
            var hello = (ISayHello) Activator.CreateInstance(type);
            //
            // 8.実行.
            //
            hello.SayHello();
        }

        public interface ISayHello
        {
            void SayHello();
        }
    }
}