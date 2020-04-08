using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Config.Form
{
    [CodePlug("XmlDataType", Description = "数据类型")]
    public enum XmlDataType
    {
        String = 1,
        Int,
        Date,
        DateTime,
        Double,
        Text,
        Money,
        Binary,
        Bit,
        Byte,
        Short,
        Long,
        Decimal
    }
}
