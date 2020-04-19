using Spoondrift.Code.PageView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config.Module
{
    public class CustomButtonConfig
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Text { get; set; }

        [XmlAttribute]
        public bool IsData { get; set; }
        [XmlAttribute]
        public string BtnCss { get; set; }
        [XmlAttribute]
        public string Icon { get; set; }
        /// <summary>
        /// 是否不允许批量操作
        /// </summary>
        [XmlAttribute]
        public bool Unbatchable { get; set; }

        public ClientConfig Client { get; set; }

        public ServerConfig Server { get; set; }

    }
}
