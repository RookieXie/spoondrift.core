using Spoondrift.Code.Data;
using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Config
{
    public interface IButtonRight:IRegName
    {
        string GetButtons(ObjectData data, IEnumerable<ObjectData> listData);
    }
}
