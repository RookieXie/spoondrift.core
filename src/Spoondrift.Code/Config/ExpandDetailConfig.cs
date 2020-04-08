using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config
{
    public class ExpandDetailConfig
    {
        [XmlAttribute]
        public ExpandDetailType Type { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}
