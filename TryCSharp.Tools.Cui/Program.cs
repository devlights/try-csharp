using System;
using System.Linq;
using System.Reflection;
using TryCSharp.Common;
using TryCSharp.Samples;

// ReSharper disable InconsistentNaming

namespace TryCSharp.Tools.Cui
{
    static class Program
    {
        private static readonly Type DummyType;
        private static string ClassName;

        static Program()
        {
            DummyType = typeof(Dummy);
            ClassName = string.Empty;
        }

        private static void Main()
        {
            try
            {
                Input.InputManager   = new CuiInputManager();
                Output.OutputManager = new CuiOutputManager();

                var emptyValidator = new EmptyInputValidator();
                var exitValidator  = new ExitPhaseValidator();

                for (;;)
                {
                    try
                    {
                        Output.Write("\nENTER CLASS NAME: ");

                        var userInput = Input.ReadLine().ToString();
                        if (emptyValidator.Validate(userInput))
                        {
                            continue;
                        }

                        if (exitValidator.Validate(userInput))
                        {
                            break;
                        }

                        var handle = Activator.CreateInstance(GetAssembly().FullName, GetFqdnName(userInput));
                        if (handle != null)
                        {
                            var clazz = handle.Unwrap();
                            if (clazz != null)
                            {
                                var executor = new CuiAppProcessExecutor();
                                executor.Execute(clazz as IExecutable);
                            }
                        }
                    }
                    catch (TypeLoadException)
                    {
                        Output.WriteLine("指定されたサンプルが見つかりません...[{0}]", ClassName);
                    }
                    catch (Exception ex)
                    {
                        Output.WriteLine(ex.ToString());
                    }
                }
            }
            finally
            {
                Output.WriteLine("\n\nPress any key to exit...");
                Input.Read();
            }
        }

        private static Assembly GetAssembly()
        {
            return DummyType.Assembly;
        }

        private static string GetInitialClassName()
        {
            return Environment.GetCommandLineArgs().Skip(1).FirstOrDefault();
        }

        private static string WithNamespace(string className)
        {
            var parts = className.Split(new char[] { '.' });
            if (parts.Length > 1)
            {
                return className;
            }

            return string.Format("{0}.{1}", DummyType.Namespace, className);
        }

        private static string GetFqdnName(string value)
        {
            var className = GetInitialClassName();
            if (!string.IsNullOrWhiteSpace(className))
            {
                return WithNamespace((ClassName = className));
            }

            return WithNamespace((ClassName = value));
        }
    }
}