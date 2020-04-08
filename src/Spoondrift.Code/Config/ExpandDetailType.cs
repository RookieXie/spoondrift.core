using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Config
{
    [CodePlug("ExpandDetailType", Description = "行扩展详情类型")]
    public enum ExpandDetailType
    {
        none = 0,
        Custom = 1,
        Tpl = 2
    }
}
