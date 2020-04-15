using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config.Form
{
    public class SearchConfig
    {
        public SearchConfig()
        {

            //  IsLike = false;
        }
        /// <summary>
        /// 是否扩展，主要用于多表查询
        /// </summary>
        [XmlAttribute]
        public bool IsExtension { get; set; }

        public bool DateSpan { get; set; }

        public ControlType ControlType { get; set; }
        [XmlAttribute]
        public bool IsLike { get; set; }
        [XmlAttribute]
        public bool IsOpenByDefault { get; set; }

    }
}
