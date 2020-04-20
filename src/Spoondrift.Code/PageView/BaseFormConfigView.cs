using Spoondrift.Code.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView
{
    public class BaseFormConfigView
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public ShowKind ShowKind { get; set; }

        public int ShowType { get; set; }

        public string Width { get; set; }
    }
}
