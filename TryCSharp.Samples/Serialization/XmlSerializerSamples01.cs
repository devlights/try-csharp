using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TryCSharp.Common;

namespace TryCSharp.Samples.Serialization
{
    /// <summary>
    ///     System.Xml.Serialization.XmlSerializerクラスのサンプルです。
    /// </summary>
    /// <remarks>
    ///     XmlSerializerクラスは、.NETの初期から存在するシリアライザクラスです。
    ///     リストや配列、列挙型をシリアライズする際にクセがあるので注意が必要です。
    ///     また、ディクショナリをシリアライズすることは出来ないので注意が必要です。
    ///     （カスタムディクショナリを作成すれば可能となります。)
    /// </remarks>
    [Sample]
    public class XmlSerializerSamples01 : IExecutable
    {
        public void Execute()
        {
            var serializer = new XmlSerializer(typeof(XmlSerializerSamples01_Data));
            var original = new XmlSerializerSamples01_Data();

            original.Initialize();
            using (var mem = new MemoryStream())
            {
                serializer.Serialize(mem, original);

                mem.Position = 0;
                mem.CopyTo(Output.OutStream);
            }
        }
    }

    [XmlRoot(ElementName = "Data")]
    public class XmlSerializerSamples01_Data
    {
        internal string _stringProperty1 = string.Empty;

        public XmlSerializerSamples01_Data()
        {
            StringProperty1 = string.Empty;
            StringProperty2 = string.Empty;
            ObjectProperty = new XmlSerializerSamples01_Data2();
            ListProperty = new List<XmlSerializerSamples01_Data2>();
            EnumProperty = XmlSerializerSamples01_Enum.Element1;
        }

        /// <summary>
        ///     通常のプロパティ
        /// </summary>
        public string StringProperty1
        {
            get { return _stringProperty1; }
            set { _stringProperty1 = value; }
        }

        /// <summary>
        ///     自動プロパティ
        /// </summary>
        public string StringProperty2 { get; set; }

        /// <summary>
        ///     カスタムオブジェクト
        /// </summary>
        public XmlSerializerSamples01_Data2 ObjectProperty { get; set; }

        /// <summary>
        ///     リスト
        /// </summary>
        [XmlArray(ElementName = "GenericListProperty")]
        [XmlArrayItem(ElementName = "Data2")]
        public List<XmlSerializerSamples01_Data2> ListProperty { get; set; }

        /// <summary>
        ///     ディクショナリ
        ///     XmlSerializerは、ディクショナリをサポートしていない。
        ///     そのままでは、シリアライズできないが、カスタムディクショナリを
        ///     作成すれば可能となる。
        ///     <para>
        ///         http://qiita.com/rohinomiya/items/b88a5da3965a1c5bed0d
        ///     </para>
        ///     DataContractSerilizerなら普通にシリアライズできるのでそっちを利用するのもアリ。
        /// </summary>
        /// <summary>
        ///     列挙型
        /// </summary>
        public XmlSerializerSamples01_Enum EnumProperty { get; set; }

        public void Initialize()
        {
            StringProperty1 = "Property1";
            StringProperty2 = "Property2";
            ObjectProperty = new XmlSerializerSamples01_Data2
            {
                IntValue = 100,
                DateTimeValue = DateTime.Now
            };
            ListProperty = new List<XmlSerializerSamples01_Data2>
            {
                new XmlSerializerSamples01_Data2
                {
                    IntValue = 200,
                    DateTimeValue = DateTime.Now
                },
                new XmlSerializerSamples01_Data2
                {
                    IntValue = 300,
                    DateTimeValue = DateTime.Now
                }
            };
            //DictionaryProperty = new Dictionary<int, XmlSerializerSamples01_Data2>
            //{
            //  {
            //    1,
            //    new XmlSerializerSamples01_Data2
            //    {
            //      IntValue = 400,
            //      DateTimeValue = DateTime.MinValue
            //    }
            //  },
            //  {
            //    2,
            //    new XmlSerializerSamples01_Data2
            //    {
            //      IntValue = 500,
            //      DateTimeValue = DateTime.MaxValue
            //    }
            //  }
            //};
            EnumProperty = XmlSerializerSamples01_Enum.Element3;
        }
    }

    public class XmlSerializerSamples01_Data2
    {
        public int IntValue { get; set; }

        public DateTime DateTimeValue { get; set; }
    }

    public enum XmlSerializerSamples01_Enum
    {
        [XmlEnum] Element1,
        [XmlEnum] Element2,
        [XmlEnum] Element3
    }
}