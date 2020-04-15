using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config.Form
{
    public class NavigationConfig
    {
        [XmlAttribute]
        public bool IsAvailable { get; set; }
        [XmlAttribute]
        public bool IsRefrech { get; set; }
        [XmlAttribute]
        public bool IsExpand { get; set; }
        public ControlType ControlType { get; set; }
        public string RegName { get; set; }

    }
}
