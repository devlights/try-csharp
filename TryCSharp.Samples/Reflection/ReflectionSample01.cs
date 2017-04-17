using System;
using System.Collections.Generic;
using TryCSharp.Common;

namespace TryCSharp.Samples.Reflection
{
    /// <summary>
    ///     リフレクションのサンプル1です。
    /// </summary>
    [Sample]
    public class ReflectionSample01 : IExecutable
    {
        public void Execute()
        {
            //
            // Typeオブジェクトの取得.
            //
            // 1.typeofを使用.
            var type1 = typeof(string);

            //
            // 2.型名から取得.
            //
            var typeName = "System.String";
            var type2 = Type.GetType(typeName);

            //
            // 3.ジェネリック型をtypeofで取得.
            //
            var type3 = typeof(List<string>);

            //
            // 4.ジェネリック型を型名から取得.
            //
            typeName = "System.Collections.Generic.List`1[System.String]";
            var type4 = Type.GetType(typeName);

            //
            // 5.型引数が1つ以上の場合.
            //
            typeName = "System.Collections.Generic.Dictionary`2[System.String, System.Int32]";
            var type5 = Type.GetType(typeName);

            //
            // 6.ジェネリック型を型引数無しでTypeオブジェクトとして取得し、後から型引数を与える場合.
            //
            typeName = "System.Collections.Generic.List`1";
            var type6 = Type.GetType(typeName);

            Type type7 = null;
            if (type6.IsGenericType && type6.IsGenericTypeDefinition)
            {
                type7 = type6.MakeGenericType(typeof(string));
            }

            Output.WriteLine(type1);
            Output.WriteLine(type2);
            Output.WriteLine(type3);
            Output.WriteLine(type4);
            Output.WriteLine(type5);
            Output.WriteLine(type6);
            Output.WriteLine(type7);
        }
    }
}