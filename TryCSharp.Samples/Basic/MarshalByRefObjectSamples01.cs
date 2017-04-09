using System;
using System.Reflection;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     MarshalByRefObjectクラスに関するサンプルです。
    /// </summary>
    [Sample]
    public class MarshalByRefObjectSamples01 : IExecutable
    {
        public void Execute()
        {
            var obj1 = new CanNotMarshalByRef();
            obj1.PrintDomain();

            var newDomain = AppDomain.CreateDomain("new domain");

            /* ** ERROR **  "Sazare.Samples.MarshalByRefObjectSamples01+CanNotMarshalByRef"はシリアル化可能として設定されていません。
      CanNotMarshalByRef obj2 =
          (CanNotMarshalByRef) newDomain.CreateInstanceAndUnwrap(
              Assembly.GetExecutingAssembly().FullName,
              typeof(CanNotMarshalByRef).FullName
          );

      obj2.PrintDomain();
      */

            var obj3 =
                (CanMarshalByRef) newDomain.CreateInstanceAndUnwrap(
                    Assembly.GetExecutingAssembly().FullName,
                    typeof(CanMarshalByRef).FullName
                );

            obj3.PrintDomain();

            //
            // Serializable属性を付加しただけでは、実行は行えるが、別のAppDomain内からの
            // 実行ではなくて、呼び元のAppDomainでの実行となる。
            // (つまり、AppDomainの境界を越えていない。)
            //
            var obj4 =
                (CanSerialize) newDomain.CreateInstanceAndUnwrap(
                    Assembly.GetExecutingAssembly().FullName,
                    typeof(CanSerialize).FullName
                );

            obj4.PrintDomain();
        }

        private class CanNotMarshalByRef
        {
            public void PrintDomain()
            {
                Output.WriteLine("Object is executing in AppDomain \"{0}\"", AppDomain.CurrentDomain.FriendlyName);
            }
        }

        private class CanMarshalByRef : MarshalByRefObject
        {
            public void PrintDomain()
            {
                Output.WriteLine("Object is executing in AppDomain \"{0}\"", AppDomain.CurrentDomain.FriendlyName);
            }
        }

        [Serializable]
        private class CanSerialize
        {
            public void PrintDomain()
            {
                Output.WriteLine("Object is executing in AppDomain \"{0}\"", AppDomain.CurrentDomain.FriendlyName);
            }
        }
    }
}