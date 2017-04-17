using System;
using System.Collections.Generic;
using System.Reflection;
using TryCSharp.Common;

namespace TryCSharp.Samples.Reflection
{
    /// <summary>
    ///     リフレクションのサンプル2です。
    /// </summary>
    [Sample]
    public class ReflectionSample02 : IExecutable
    {
        public void Execute()
        {
            var type = typeof(List<string>);

            //
            // Activatorを利用してインスタンス化.（Typeのみの指定）
            //
            var obj = Activator.CreateInstance(type);
            Output.WriteLine(obj.GetType());

            //
            // Activatorを利用してインスタンス化.(Assembly名と型名)
            // この場合、戻り値はSystem.Runtime.Remoting.ObjectHandleになります。
            //
            var objHandle = Activator.CreateInstance(Assembly.GetAssembly(type).FullName, type.FullName);
            obj = objHandle.Unwrap();
            Output.WriteLine(objHandle.GetType());
            Output.WriteLine(obj.GetType());

            //
            // Assemblyを利用してインスタンス化.
            // 以下が使えるのは、対象となるクラスが既に読み込み済みのアセンブリの場合です。
            // まだ読み込まれていないアセンブリに含まれるクラスの場合は先にLoadしてから使います。
            //
            obj = Assembly.GetAssembly(type).CreateInstance(type.FullName);
            Output.WriteLine(obj?.GetType());
        }
    }
}