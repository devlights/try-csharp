using System;
using System.Collections.Generic;
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

                        var optInfo = new Dictionary<string, bool>
                        {
                            {"fullMatched", false}
                        };

                        var filtered = typeFullNameList.Where(x =>
                        {
                            var fqdn = x.ToLower();
                            var inp = userInput.ToLower();

                            var fullMatch = fqdn.Split('.').Last() == inp;
                            if (fullMatch)
                            {
                                optInfo["fullMatched"] = true;
                            }

                            return fqdn.Contains(inp);
                        }).ToList();

                        if (filtered.Count == 0)
                        {
                            Output.WriteLine("not found...[{0}]", userInput);
                            continue;
                        }

                        if (!optInfo["fullMatched"])
                        {
                            if (filtered.Count > 1)
                            {
                                Output.WriteLine("There are multiple candidates.");
                                foreach (var item in filtered)
                                {
                                    Output.WriteLine("**** {0}", item);
                                }

                                continue;
                            }
                        }

                        var clazz = Activator.CreateInstance(GetAssembly().GetType(), filtered.First());
                        if (clazz == null)
                        {
                            continue;
                        }

                        // FIXME: 以下の処理がダサい。そのうち直す。
                        var executor = new CuiAppProcessExecutor();
                        var target = clazz as IExecutable;
                        if (target != null)
                        {
                            executor.Execute(target);                            
                        }
                        else
                        {
                            var asyncTarget = clazz as IAsyncExecutable;
                            executor.Execute(asyncTarget);
                        }
                    }
                    catch (TypeLoadException)
                    {
                        Output.WriteLine("not found...[{0}]", ClassName);
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