using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config
{
    public class GroupConfig
    {
        public string IsDisabled { get; set; }

        [XmlIgnore]
        public PageStyle ShowPage { get; set; }

        [XmlElement("ShowPage")]
        public string InternalShowPage { get; set; }
    }
}
