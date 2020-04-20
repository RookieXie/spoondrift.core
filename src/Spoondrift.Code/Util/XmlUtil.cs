using Spoondrift.Code.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Spoondrift.Code.Util
{
    public static class XmlUtil
    {
        #region XmlReadUtil

        internal static readonly XmlReaderSettings ReaderSetting = InitReading();

        private static XmlReaderSettings InitReading()
        {
            XmlReaderSettings result = new XmlReaderSettings { CloseInput = true };
            return result;
        }

        public static XmlReader GetXmlReader(string path)
        {


            XmlReader reader = XmlReader.Create(new Uri(path).ToString(), ReaderSetting);
            return reader;
        }

        public static XmlReader GetXmlReader(Stream stream)
        {


            XmlReader reader = XmlReader.Create(stream, ReaderSetting);
            return reader;
        }

        internal static readonly XmlWriterSettings WriterSetting = InitWriting();

        internal static readonly XmlWriterSettings WriterSetting2 = InitWriting2();

        private static XmlWriterSettings InitWriting()
        {
            XmlWriterSettings result = new XmlWriterSettings
            {
                CloseOutput = true
            };
            return result;
        }

        private static XmlWriterSettings InitWriting2()
        {
            XmlWriterSettings result = new XmlWriterSettings
            {
                CloseOutput = false
            };
            return result;
        }

        public static string ToString(object value)
        {
            if (value == null)
                return string.Empty;

            if (value is bool)
                return value.ToString().ToLower();
            TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
            if (converter == null)
                return value.ToString();
            else
                return converter.ConvertToString(value);
        }

        internal static void WriteBinaryNullable(BinaryWriter writer, bool nullable)
        {
            writer.Write(nullable);
        }
        #endregion
        #region 序列化

        private static void CallBackSerialed(Object obj)
        {
            var callBack = obj as IReadXmlCallback;
            if (callBack != null)
            {
                callBack.OnReadXml();
            }
        }

        public static T ReadFromString<T>(string xmlString)
            where T : XmlConfigBase, new()
        {
            using (Stream xmlStream = new MemoryStream(Encoding.Unicode.GetBytes(xmlString)))
            {
                using (XmlReader xmlReader = XmlReader.Create(xmlStream))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    Object obj = xmlSerializer.Deserialize(xmlReader);
                    CallBackSerialed(obj);
                    return obj as T;
                }
            }
        }

        public static object ReadFromFile(string filePath, Type type)
        {


            XmlReader xmlReader = XmlUtil.GetXmlReader(filePath);
            using (xmlReader)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                Object obj = xmlSerializer.Deserialize(xmlReader);
                CallBackSerialed(obj);
                return obj;
            }
        }
        public static T PlugGetXml<T>(this string name)
        {
            name = name.ToUpper();


            if (name.IndexOf("@") >= 0)
            {
                name = name.Replace(".XML", "");
                string[] arr = name.Split('@');
                string xml = arr[0];
                name = xml + ".XML";
            }

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"forms", name);
            object obj = XmlUtil.ReadFromFile(path, typeof(T));
            return (T)obj;

        }

        public static T ReadFromFile<T>(string filePath)
             where T : XmlConfigBase, new()
        {
            XmlReader xmlReader = XmlUtil.GetXmlReader(filePath);
            using (xmlReader)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                Object obj = xmlSerializer.Deserialize(xmlReader);
                CallBackSerialed(obj);
                return obj as T;
            }
        }

        public static byte[] SaveBinaryFormat<T>(T obj)
        {
            MemoryStream ms = new MemoryStream();
            using (ms)
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T SaveToFile<T>(string filePath, T XMLObj)
             where T : XmlConfigBase, new()
        {
            XmlSerializer x = new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(filePath);
            using (writer)
            {
                x.Serialize(writer, XMLObj as T);
                return null;
            }
        }
        #endregion

    }
}
