using Spoondrift.Code.PageView;
using Spoondrift.Code.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config.Module
{
    public class RightConfig : IReadXmlCallback
    {
        public FunctionRightConfig FunctionRights { get; set; }

        [XmlArrayItem("ColumnRight")]
        public List<ColumnRightConfig> ColumnRights { get; set; }


        void IReadXmlCallback.OnReadXml()
        {
            
        }
    }
}
