using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        private static async Task Main(string[] args)
        {
            var onetime = args.Contains("--onetime");

            Input.InputManager = new CuiInputManager();
            Output.OutputManager = new CuiOutputManager();

            var emptyValidator = new EmptyInputValidator();
            var exitValidator = new ExitPhaseValidator();

            var typeFullNameList = GetAssembly().GetExportedTypes().Select(x => x.FullName).ToList();
            for (;;)
            {
                if (await Execute(emptyValidator, exitValidator, typeFullNameList, onetime))
                {
                    break;                        
                }
            }
        }

        private static async Task<bool> Execute(
            IHasValidation<string> emptyValidator, 
            IHasValidation<string> exitValidator,
            IEnumerable<string> typeFullNameList, 
            bool onetime)
        {
            try
            {
                Output.Write("\nENTER CLASS NAME: ");

                var userInput = Input.ReadLine().ToString();
                if (userInput == null)
                {
                    return false;
                }
                
                if (emptyValidator.Validate(userInput))
                {
                    return false;
                }

                if (exitValidator.Validate(userInput))
                {
                    return true;
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
                    return false;
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

                        return false;
                    }
                }

                var clazz = GetInstance(filtered.First());
                if (clazz == null)
                {
                    return false;
                }

                var executor = new CuiAppProcessExecutor();
                switch (clazz)
                {
                    case IExecutable target:
                    {
                        executor.Execute(target);
                        break;
                    }

                    case IAsyncExecutable asyncTarget:
                    {
                        await executor.Execute(asyncTarget);
                        break;
                    }

                    default:
                        Output.WriteLine($"**** INVALID SAMPLE TYPE **** [{clazz.GetType().FullName}]");
                        break;
                }

                if (onetime)
                {
                    return true;
                }
            }
            catch (TypeLoadException)
            {
                Output.WriteLine($"**** NOT FOUND **** [{ClassName}]");
            }
            catch (Exception ex)
            {
                Output.WriteLine(ex.ToString());
            }

            return false;
        }

        private static Assembly GetAssembly()
        {
            return DummyType.Assembly;
        }

        private static object GetInstance(string target)
        {
            return GetAssembly().CreateInstance(target);
        }
    }
}
