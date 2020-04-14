using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PlugIn
{
    public class PlugInModel
    {
        public PlugInModel()
        {
            Tags = new List<PlugInTag>();
        }

        public string Author
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string CreateDate
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string RegName
        {
            get;
            set;
        }

        public Type BaseType { get; set; }
        public Type InstanceType { get; set; }
        public List<PlugInTag> Tags { get; set; }

    }
}
