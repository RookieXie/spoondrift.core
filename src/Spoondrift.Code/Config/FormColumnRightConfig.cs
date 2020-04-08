using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config
{
    public class FormColumnRightConfig
    {
        [XmlAttribute]
        public string Name { get; set; }

        public string RegName { get; set; }

    }
}
