using System.Collections.Generic;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     各型のデフォルト値についてのサンプルです。
    /// </summary>
    [Sample]
    public class DefaultValuesSamples01 : IExecutable
    {
        public void Execute()
        {
            Output.WriteLine("byte   のデフォルト:    {0}", default(byte));
            Output.WriteLine("char   のデフォルト:    {0}", default(char) == 0x00);
            Output.WriteLine("short  のデフォルト:    {0}", default(short));
            Output.WriteLine("ushort のデフォルト:    {0}", default(ushort));
            Output.WriteLine("int  のデフォルト:    {0}", default(int));
            Output.WriteLine("uint   のデフォルト:    {0}", default(uint));
            Output.WriteLine("long   のデフォルト:    {0}", default(long));
            Output.WriteLine("ulong  のデフォルト:    {0}", default(ulong));
            Output.WriteLine("float  のデフォルト:    {0}", default(float));
            Output.WriteLine("double のデフォルト:    {0}", default(double));
            Output.WriteLine("decimalのデフォルト:    {0}", default(decimal));
            Output.WriteLine("string のデフォルト:    NULL = {0}", default(string) == null);
            Output.WriteLine("byte[] のデフォルト:    NULL = {0}", default(byte[]) == null);
            Output.WriteLine("List<string>のデフォルト: NULL = {0}", default(List<string>) == null);
            Output.WriteLine("自前クラスのデフォルト:   NULL = {0}", default(SampleClass) == null);
            Output.WriteLine("自前構造体のデフォルト:   {0}", default(SampleStruct));
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class SampleClass
        {
        }

        private struct SampleStruct
        {
        }
    }
}