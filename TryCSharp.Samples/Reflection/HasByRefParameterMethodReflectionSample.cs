using System;
using System.Reflection;
using TryCSharp.Common;

namespace TryCSharp.Samples.Reflection
{
    /// <summary>
    ///     ByRefの引数を持つメソッドをリフレクションで取得するサンプルです。
    /// </summary>
    [Sample]
    public class HasByRefParameterMethodReflectionSample : IExecutable
    {
        public void Execute()
        {
            var type = typeof(HasByRefParameterMethodReflectionSample);
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            Type[] paramTypes = {typeof(string), Type.GetType("System.Int32&"), typeof(int)};

            var methodInfo = type.GetMethod("SetPropertyValue", flags, null, paramTypes, null);
            Output.WriteLine(methodInfo);
        }

        // <summary>
        // Dummy Method.
        // </summary>
        protected void SetPropertyValue(string propName, ref int refVal, int val)
        {
            //
            // nop.
            //
        }
    }
}