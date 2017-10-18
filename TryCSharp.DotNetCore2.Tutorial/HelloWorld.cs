using System;

namespace TryCSharp.DotNetCore2.Tutorial
{
    internal class HelloWorld : IExecutable
    {
        public void Execute()
        {
            Console.WriteLine("Hello world");
        }
    }
}