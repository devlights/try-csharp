using System.Linq;
using System.Collections.Generic;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples42 : IExecutable
    {
        public void Execute()
        {
            //
            // Containsメソッドは、指定された要素がシーケンス内に存在するか否かを返す。
            //
            // IEqualityComparer<T>を指定するオーバーロードも存在する。
            //
            var names = new[] {"csharp", "visualbasic", "java", "python", "ruby", "php"};
            Output.WriteLine("要素[python]は存在する? = {0}", names.Contains("python"));

            //
            // IEqualityComparer<T>を指定するバージョン.
            //
            var languages = new[]
            {
                Language.Create("csharp"),
                Language.Create("visualbasic"),
                Language.Create("java"),
                Language.Create("python"),
                Language.Create("ruby"),
                Language.Create("php")
            };

            Output.WriteLine(
                "要素[python]は存在する? = {0}",
                languages.Contains(Language.Create("python"), new LanguageNameComparer())
            );
        }

        private class Language
        {
            public string Name { get; set; }

            public static Language Create(string name)
            {
                return new Language {Name = name};
            }
        }

        private class LanguageNameComparer : EqualityComparer<Language>
        {
            public override bool Equals(Language l1, Language l2)
            {
                return l1.Name == l2.Name;
            }

            public override int GetHashCode(Language l)
            {
                return l?.Name?.GetHashCode() ?? 0;
            }
        }
    }
}