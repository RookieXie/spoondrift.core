using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config.Form
{
    public class DeleteColumnConfig
    {
        [XmlAttribute]
        public string Name { get; set; }
    }
}
