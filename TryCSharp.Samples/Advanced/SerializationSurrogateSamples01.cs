using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TryCSharp.Common;

namespace TryCSharp.Samples.Advanced
{
    /// <summary>
    ///     シリアライズに関するサンプルです。
    /// </summary>
    /// <remarks>
    /// シリアル化サロゲートについて。 (ISerializationSurrogate)
    ///
    /// 本サンプルは .NET 5.0 では使用できません。（BinaryFormatterが非推奨扱いになっているため)
    /// 詳細については、以下に記載があります。
    ///   - https://docs.microsoft.com/ja-jp/dotnet/standard/serialization/binaryformatter-security-guide
    /// </remarks>
    [Sample]
    public class SerializationSurrogateSamples01 : IExecutable
    {
        public void Execute()
        {
#if ENABLE_OLD_NET_FEATURE
            //
            // 普通のシリアライズ処理.
            //
            var obj = MakeSerializableObject();
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                // 成功する.
                formatter.Serialize(stream, obj);

                stream.Position = 0;
                Output.WriteLine(formatter.Deserialize(stream));
            }

            //
            // シリアライズ不可 (Serializable属性をつけていない)
            //
            var obj2 = MakeNotSerializableObject();
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                try
                {
                    // 対象クラスにSerializable属性が付与されていないので
                    // 以下を実行すると例外が発生する.
                    formatter.Serialize(stream, obj2);

                    stream.Position = 0;
                    Output.WriteLine(formatter.Deserialize(stream));
                }
                catch (SerializationException ex)
                {
                    Output.WriteLine("[ERROR]: {0}", ex.Message);
                }
            }

            //
            // シリアル化サロゲート. (SerializationSurrogate)
            //
            var obj3 = MakeNotSerializableObject();
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                //
                // シリアル化サロゲートを行うために、以下の手順で設定を行う.
                //
                // 1.SurrogateSelectorオブジェクトを用意.
                // 2.自作Surrogateクラスを用意.
                // 3.SurrogateSelector.AddSurrogateでSurrogateオブジェクトを設定
                // 4.SurrogateSelectorをFormatterに設定.
                //
                // これにより、シリアライズ不可なオブジェクトをFormatterにてシリアライズ/デシリアライズ
                // する際にシリアル化サロゲートが行われるようになる。
                //
                var selector = new SurrogateSelector();
                var surrogate = new CanNotSerializeSurrogate();
                var context = new StreamingContext(StreamingContextStates.All);

                selector.AddSurrogate(typeof(CanNotSerialize), context, surrogate);

                formatter.SurrogateSelector = selector;

                try
                {
                    // 通常、以下を実行すると例外が発生するが
                    // シリアル化サロゲートを行うので、エラーとならずシリアライズが成功する.
                    formatter.Serialize(stream, obj3);

                    stream.Position = 0;
                    Output.WriteLine(formatter.Deserialize(stream));
                }
                catch (SerializationException ex)
                {
                    Output.WriteLine("[ERROR]: {0}", ex.Message);
                }
            }
#endif
        }

        private IHasNameAndAge MakeSerializableObject()
        {
            return new CanSerialize
            {
                Name = "hoge"
                ,
                Age = 99
            };
        }

        private IHasNameAndAge MakeNotSerializableObject()
        {
            return new CanNotSerialize
            {
                Name = "hehe"
                ,
                Age = 98
            };
        }

        #region SampleInterfaceAndClasses

        private interface IHasNameAndAge
        {
            string Name { get; set; }
            int Age { get; set; }
        }

        // シリアライズ可能なクラス
        [Serializable]
        private class CanSerialize : IHasNameAndAge
        {
            private int _age;
            private string _name;

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public int Age
            {
                get { return _age; }
                set { _age = value; }
            }

            public override string ToString()
            {
                return string.Format("[CanSerialize] Name={0}, Age={1}", Name, Age);
            }
        }

        // シリアライズ不可なクラス
        private class CanNotSerialize : IHasNameAndAge
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public override string ToString()
            {
                return string.Format("[CanNotSerialize] Name={0}, Age={1}", Name, Age);
            }
        }

        // CanNotSerializeクラスのためのサロゲートクラス.
        private class CanNotSerializeSurrogate : ISerializationSurrogate
        {
            // シリアライズ時に呼び出されるメソッド
            public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
            {
                var targetObj = obj as CanNotSerialize;
                if (targetObj == null)
                {
                    return;
                }

                //
                // シリアライズする項目と値を以下のようにinfoに設定していく.
                //
                info.AddValue("Name", targetObj.Name);
                info.AddValue("Age", targetObj.Age);
            }

            // デシリアライズ時に呼び出されるメソッド.
            public object SetObjectData(object obj, SerializationInfo info, StreamingContext context,
                ISurrogateSelector selector)
            {
                var targetObj = obj as CanNotSerialize;
                if (targetObj == null)
                {
                    return null;
                }

                //
                // infoから値を取得し、対象となるオブジェクトに設定.
                //
                targetObj.Name = info.GetString("Name");
                targetObj.Age = info.GetInt32("Age");

                // Formatterは, この戻り値を無視するので戻り値はnullで良い.
                return null;
            }
        }

        #endregion
    }
}