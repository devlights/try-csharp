using System;
using System.Linq;
using System.Reflection;
using TryCSharp.Common;
using TryCSharp.Samples;

// ReSharper disable InconsistentNaming

namespace TryCSharp.Tools.Cui
{
    internal static class Program
    {
        private static readonly Type DummyType;
        private static readonly string ClassName;

        static Program()
        {
            DummyType = typeof(Dummy);
            ClassName = string.Empty;
        }

        private static void Main()
        {
            try
            {
                Input.InputManager = new CuiInputManager();
                Output.OutputManager = new CuiOutputManager();

                var emptyValidator = new EmptyInputValidator();
                var exitValidator = new ExitPhaseValidator();

                var typeFullNameList = GetAssembly().GetExportedTypes().Select(x => x.FullName).ToList();
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

                        var filtered = typeFullNameList.Where(x =>
                        {
                            var fqdn = x.ToLower();
                            var inp = userInput.ToLower();

                            return fqdn.Contains(inp);
                        }).ToList();

                        if (filtered.Count == 0)
                        {
                            Output.WriteLine("指定されたサンプルが見つかりません...[{0}]", userInput);
                            continue;
                        }

                        if (filtered.Count > 1)
                        {
                            Output.WriteLine("候補が複数存在します。");
                            foreach (var item in filtered)
                            {
                                Output.WriteLine("**** {0}", item);
                            }

                            continue;
                        }

                        var handle = Activator.CreateInstance(GetAssembly().FullName, filtered.First());

                        var clazz = handle?.Unwrap();
                        if (clazz == null)
                        {
                            continue;
                        }

                        var executor = new CuiAppProcessExecutor();
                        executor.Execute(clazz as IExecutable);
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
    }
}