using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config.Form
{
    public class SelectorConfig
    {
        //public string RegName { get; set; }
        public string DataText { get; set; }
        [XmlAttribute]
        public bool Descendant { get; set; }

        public string ModuleXml { get; set; }
    }
}
