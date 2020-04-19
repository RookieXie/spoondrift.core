using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Config.Module
{
    public class RelationConfig
    {
        public string MasterForm { get; set; }
        public string DetailForm { get; set; }

        public string MasterField { get; set; }
        public string DetailField { get; set; }
    }
}
