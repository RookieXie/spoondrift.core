using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config.Form
{
    public class OverrideColumnConfig : ColumnConfig
    {
        [XmlAttribute("Name")]
        public string BaseColumnName { get; set; }
    }
}
