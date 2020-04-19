using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.PageView.FormViewCreator
{
    public class ChangerViewConfig
    {
        public string Expression { get; set; }
        [XmlElement("Depend")]
        public List<string> DependColumns { get; set; }

        [XmlElement("Notify")]
        public List<string> NotifyColumns { get; set; }
    }
}
