using Spoondrift.Code.Config.Form;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.PageView
{
    public class ColumnRightConfig
    {

        public ColumnRightConfig()
        {
            this.Override = new List<OverrideColumnConfig>();
            this.Override1 = new List<OverrideColumnConfig>();

            this.Add = new List<ColumnConfig>();
            this.Add1 = new List<ColumnConfig>();

            this.Delete = new List<DeleteColumnConfig>();
            this.Delete1 = new List<DeleteColumnConfig>();
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlElement("OverrideItem")]
        public List<OverrideColumnConfig> Override1 { get; set; }

        [XmlElement("AddItem")]
        public List<ColumnConfig> Add1 { get; set; }

        [XmlElement("DeleteItem")]
        public List<DeleteColumnConfig> Delete1 { get; set; }


        [XmlArrayItem("Column")]
        public List<OverrideColumnConfig> Override { get; set; }

        [XmlArrayItem("Column")]
        public List<ColumnConfig> Add { get; set; }

        [XmlArrayItem("Column")]
        public List<DeleteColumnConfig> Delete { get; set; }

    }
}
