using System;
using System.Reflection;
using TryCSharp.Common;

namespace TryCSharp.Samples.Reflection
{
    /// <summary>
    ///     ジェネリックメソッドをリフレクションで取得するサンプルです。
    /// </summary>
    [Sample]
    public class GenericMethodReflectionSample : IExecutable
    {
        public void Execute()
        {
            var type = typeof(GenericMethodReflectionSample);
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            //
            // ジェネリックメソッドが一つしかない場合は以下のようにして取得できる。
            //
            // ジェネリック定義されている状態のメソッド情報を取得.
            // MethodInfo mi = type.GetMethod("SetPropertyValue", flags);
            // 型引数を設定して、実メソッド情報を取得
            // MethodInfo genericMi = mi.MakeGenericMethod(new Type[]{ typeof(DateTime) });
            //
            // しかし、同名メソッドのオーバーロードが複数存在する場合は一旦GetMethodsにて
            // ループさせ、該当するメソッドを見つける作業が必要となる。
            //
            // [参照URL]
            // http://www.codeproject.com/KB/dotnet/InvokeGenericMethods.aspx
            //
            var methodName = "SetPropertyValue";
            Type[] paramTypes = {typeof(string), typeof(DateTime), typeof(DateTime)};
            foreach (var mi in type.GetMethods(flags))
            {
                if (mi.IsGenericMethod && mi.IsGenericMethodDefinition && mi.ContainsGenericParameters)
                {
                    if ((mi.Name == methodName) && (mi.GetParameters().Length == paramTypes.Length))
                    {
                        var genericMi = mi.MakeGenericMethod(typeof(DateTime));
                        Output.WriteLine(genericMi);
                    }
                }
            }
        }

        protected void SetPropertyValue(string propName, ref int refVal, int val)
        {
            //
            // nop.
            //
        }

        protected void SetPropertyValue<T>(string propName, ref T refVal, T val)
        {
            //
            // nop.
            //
        }
    }
}