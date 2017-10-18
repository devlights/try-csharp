using System;

namespace TryCSharp.DotNetCore2.Tutorial
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var me = new Program();
            me.Execute();            
        }

        private void Execute()
        {
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("please enter class name. exit....");
                return;
            }

            var fullTypeName = string.Format("TryCSharp.DotNetCore2.Tutorial.{0}", input);
            var type = Type.GetType(fullTypeName);
            if (type == null)
            {
                Console.WriteLine("[ERROR] target type does not found. exit....");
                return;
            }
            
            var instance = Activator.CreateInstance(type);
            if (instance == null)
            {
                Console.WriteLine("[ERROR] error has occured exit....");
                return;
            }

            if (!(instance is IExecutable executable))
            {
                Console.WriteLine("[ERROR] can't cast to IExecutable. exit...");
                return;
            }
            
            executable.Execute();

            Console.WriteLine("exit");
        }
    }
}