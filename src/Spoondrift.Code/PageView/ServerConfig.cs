using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.PageView
{
    public class ServerConfig
    {
        [XmlAttribute]
        public bool IsSubmit { get; set; }

        public string PlugName { get; set; }

    }
}
