using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Spoondrift.Code.Xml
{
    public abstract class XmlConfigBase : IXmlSerial
    {

        public void SaveStringBuilder(StringBuilder sb, Formatting formatting = Formatting.Indented)
        {
            StringWriter sw = new StringWriter(sb);

            XmlTextWriter xtw = new XmlTextWriter(sw);
            using (xtw)
            {
                xtw.Formatting = formatting;
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(xtw, this);
            }
        }

        public void SaveFile(string file)
        {
            ConfirmPath(Path.GetDirectoryName(file));
            FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
            using (fs)
            {
                XmlSerializer xs = new XmlSerializer(this.GetType());
                xs.Serialize(fs, this);
            }
        }
        private void ConfirmPath(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        public string SaveString(Formatting formatting = Formatting.Indented)
        {
            StringBuilder sb = new StringBuilder();
            SaveStringBuilder(sb, formatting);
            return sb.ToString();
        }

        public override string ToString()
        {
            return "配置类:  " + base.ToString();
        }
    }
}
