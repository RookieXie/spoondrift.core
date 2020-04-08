using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config
{
    public class BaseFormConfig
    {

        [XmlAttribute]
        public string Title { get; set; }

        /// <summary>
        /// 必填的
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public ShowKind ShowKind { get; set; }

        [XmlAttribute]
        public int ShowType { get; set; }

        [XmlAttribute]
        public string Order { get; set; }

        public bool VerticalTab { get; set; }

        public string AfterInitFunName { get; set; }
        [XmlAttribute]
        public string Width { get; set; }

    }
}
