using System;
using System.Linq;
using TryCSharp.Common;

namespace TryCSharp.Samples.Basic
{
    /// <summary>
    ///     Enumについてのサンプルです。
    /// </summary>
    [Sample]
    public class EnumSamples001 : IExecutable
    {
        public void Execute()
        {
            //
            // FlagsAttributeを付与している場合は
            // 単体の値としても利用できるが、AND OR XORの
            // 演算も行えるようになる。
            //
            var enum1 = SampleEnum.Value2;
            var enum2 = SampleEnum.Value1 | SampleEnum.Value3;

            Output.WriteLine(enum1);
            Output.WriteLine(enum2);

            Output.WriteLine("enum2 has Value3? == {0}", (enum2 & SampleEnum.Value3) == SampleEnum.Value3);
            Output.WriteLine("enum2 has Value2? == {0}", (enum2 & SampleEnum.Value2) == SampleEnum.Value2);

            /////////////////////////////////////////////////////////////
            //
            // System.Enumクラスには、列挙型を扱う上で便利なメソッドが
            // いくつか用意されている。
            //
            // ■Formatメソッド
            // ■GetNameメソッド
            // ■GetNamesメソッド
            // ■GetUnderlyingTypeメソッド
            // ■GetValuesメソッド
            // ■IsDefinedメソッド
            // ■Parseメソッド
            // ■ToObjectメソッド
            // ■ToStringメソッド
            //
            Output.WriteLine(string.Empty);

            //
            // Formatメソッド.
            //
            // 対象となる列挙値を特定のフォーマットにして取得する。
            // 指定出来るオプションは以下の通り。
            //
            // ■G or g: 名前を取得（但し、値が存在しない場合、１０進数でその値が返される）
            // ■X or x: １６進数で値を取得 (但し、0xは先頭に付与されない）
            // ■D or d: １０進数で値を取得
            // ■F or f: Gとほぼ同じ。
            //
            Output.WriteLine("============ {0} ============", "Format");
            Output.WriteLine(Enum.Format(typeof(SampleEnum), 2, "G"));
            Output.WriteLine(Enum.Format(typeof(SampleEnum), 2 | 3, "G"));
            Output.WriteLine(Enum.Format(typeof(SampleEnum), SampleEnum.Value1 | SampleEnum.Value3, "G"));
            Output.WriteLine(Enum.Format(typeof(SampleEnum), SampleEnum.Value4, "X"));
            Output.WriteLine(Enum.Format(typeof(SampleEnum), SampleEnum.Value4, "D"));
            Output.WriteLine(Enum.Format(typeof(SampleEnum), SampleEnum.Value4, "F"));
            Output.WriteLine(Enum.Format(typeof(SampleEnum), SampleEnum.Value1 | SampleEnum.Value4, "F"));

            //
            // GetNameメソッド
            //
            // 対象となる値から、対応する列挙値の名前を取得する.
            // 対応する列挙値が存在しない場合は、nullとなる。
            //
            Output.WriteLine("============ {0} ============", "GetName");
            var targetValue = 4;
            Output.WriteLine(Enum.GetName(typeof(SampleEnum), targetValue));
            Output.WriteLine(Enum.GetName(typeof(SampleEnum), -1) == null ? "null" : string.Empty);

            //
            // GetNamesメソッド
            //
            // 対象となる列挙型に定義されている値の名称を一気に取得する.
            //
            Output.WriteLine("============ {0} ============", "GetNames");
            var names = Enum.GetNames(typeof(SampleEnum));
            names.ToList().ForEach(Output.WriteLine);

            //
            // GetUnderlyingTypeメソッド
            //
            // 特定の列挙値が属する列挙型を取得する。
            //
            Output.WriteLine("============ {0} ============", "GetUnderlyingType");
            Enum enumVal = SampleEnum.Value2;
            var enumType = enumVal.GetType();
            var underlyingType = Enum.GetUnderlyingType(enumType);

            Output.WriteLine(enumType.Name);
            Output.WriteLine(underlyingType.Name);

            //
            // GetValuesメソッド
            //
            // 対象となる列挙型に設定されている値を一気に取得.
            //
            Output.WriteLine("============ {0} ============", "GetValues");
            var valueArray = Enum.GetValues(typeof(SampleEnum));
            foreach (var element in valueArray)
            {
                Output.WriteLine(element);
            }

            //
            // IsDefinedメソッド
            //
            // 指定した値が、対象となる列挙型に存在するか否かを調査する。
            //
            Output.WriteLine("============ {0} ============", "IsDefined");
            Output.WriteLine("値{0}がSampleEnumに存在するか？ {1}", 2, Enum.IsDefined(typeof(SampleEnum), 2));
            Output.WriteLine("値{0}がSampleEnumに存在するか？ {1}", 10, Enum.IsDefined(typeof(SampleEnum), 10));

            //
            // Parseメソッド.
            //
            // 文字列から対応する列挙値を取得する。
            // 尚、該当文字列に対応する列挙値が存在しない場合はnullでなく
            // ArgumentExceptionが発生する。
            //
            // Parseメソッドには、以下のパターンのデータを指定することが出来る。
            // ■単一の値
            // ■列挙値の名前
            // ■名前をコンマで繋いだリスト
            //
            // 名前をコンマで繋いだリストを指定した場合は、該当する列挙値の
            // OR演算された結果が取得できる。
            //
            Output.WriteLine("============ {0} ============", "Parse");
            var testVal = "Value4";
            Output.WriteLine(Enum.Parse(typeof(SampleEnum), testVal));

            try
            {
                // 存在しない値を指定.
                Output.WriteLine(Enum.Parse(typeof(SampleEnum), "not_found"));
            }
            catch (ArgumentException)
            {
                Output.WriteLine("文字列 not_found に対応する列挙値が存在しない。");
            }

            testVal = "4";
            Output.WriteLine(Enum.Parse(typeof(SampleEnum), testVal));

            testVal = "Value1,Value2,Value4";
            Output.WriteLine(Enum.Parse(typeof(SampleEnum), testVal));

            //
            // ToObjectメソッド.
            //
            // 指定された値を対応する列挙値に変換する。
            // 各型に対応するためのオーバーロードメソッドが存在する。
            //
            Output.WriteLine("============ {0} ============", "ToObject");
            var v = 1;
            Output.WriteLine(Enum.ToObject(typeof(SampleEnum), v));

            //
            // ToStringメソッド.
            //
            // 対応する列挙値の文字列表現を取得する。
            // これまでに上述した各処理は全てEnumクラスのstaticメソッドで
            // あったが、このメソッドはインスタンスメソッドとなる。
            //
            // 基本的に、Enum.Formatメソッドに"G"を適用した結果となる。
            // （IFormatProviderを指定した場合はカスタム書式となる。）
            //
            Output.WriteLine("============ {0} ============", "ToString");
            var e1 = SampleEnum.Value4;
            Output.WriteLine(e1.ToString());
        }

        //
        // Enumを定義.
        //
        // フラグ値としても利用する場合はFlagAttributeを付ける.
        //
        // 基になる型は明示的に指定しない場合はintとなる。
        // 列挙定数は２の累乗で定義する方がいい模様。（MSDNより）
        //
        [Flags]
        private enum SampleEnum
        {
            Value1 = 1,
            Value2 = 2,
            Value3 = 4,
            Value4 = 16
        }
    }
}