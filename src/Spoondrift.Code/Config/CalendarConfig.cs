using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config
{
    public class CalendarConfig
    {
        [XmlAttribute]
        public CalendarView DefaultView { get; set; }

        [XmlAttribute]
        public string DisplayViews { get; set; }

        public string TitleField { get; set; }

        public string TitleFormatFun { get; set; }

        public string StartDateField { get; set; }

        public string EndDateField { get; set; }
    }
}
