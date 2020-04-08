using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Config
{
    [CodePlug("FormType", Description = " 表单类型")]
    public enum FormType
    {
        None = 0,
        Normal = 1,
        Grid = 2,
        Album = 3,
        Activity = 4,
        File = 5,
        Comment = 6,
        Forward = 7,
        Calendar = 8,
        Tpl = 10,
        Custom = 11
    }
}
