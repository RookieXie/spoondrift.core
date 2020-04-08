using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Config.Module
{

    [CodePlug("ModuleMode", Description = " 模块模式")]
    public enum ModuleMode
    {
        None = 0,
        Single = 1,
        MasterDetail = 2,
        Multiple = 3,
        SingleToSingle = 4
    }
}
