using System;
using System.Collections.Generic;
using System.Text;

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
