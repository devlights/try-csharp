using System.Collections.Generic;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Linq
{
    /// <summary>
    ///     Linqのサンプルです。
    /// </summary>
    [Sample]
    public class LinqSamples48 : IExecutable
    {
        public void Execute()
        {
            //
            // SequenceEqual拡張メソッドは、2つのシーケンスが等しいか否かを判別するメソッド。
            //
            // IEqualityComparerを指定できるオーバーロードが存在する.
            //
            var numbers1 = Enumerable.Range(1, 10);
            var numbers2 = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

            Output.WriteLine("numbers1 eq numbers2 == {0}", numbers1.SequenceEqual(numbers2));

            //
            // IEqualityComparer<T>を指定するバージョン.
            //
            var languages1 = new[]
            {
                Language.Create("csharp"),
                Language.Create("visualbasic"),
                Language.Create("java"),
                Language.Create("python"),
                Language.Create("ruby"),
                Language.Create("php")
            };

            var languages2 = new[]
            {
                Language.Create("csharp"),
                Language.Create("visualbasic"),
                Language.Create("java"),
                Language.Create("python"),
                Language.Create("ruby"),
                Language.Create("php")
            };

            var languages3 = new[]
            {
                Language.Create("csharp"),
                Language.Create("visualbasic"),
                Language.Create("java")
            };

            var comparer = new LanguageNameComparer();

            Output.WriteLine(
                "languages1 eq languages2 == {0}",
                languages1.SequenceEqual(languages2, comparer)
            );

            Output.WriteLine(
                "languages1 eq languages3 == {0}",
                languages1.SequenceEqual(languages3, comparer)
            );
        }

        private class Language
        {
            public string? Name { get; set; }

            public static Language Create(string name)
            {
                return new Language {Name = name};
            }
        }

        private class LanguageNameComparer : EqualityComparer<Language>
        {
            public override bool Equals(Language? l1, Language? l2)
            {
                return l1!.Name == l2!.Name;
            }

            public override int GetHashCode(Language l)
            {
                return l?.Name?.GetHashCode() ?? 0;
            }
        }
    }
}