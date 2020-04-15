using Spoondrift.Code.Data;
using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Config.Form
{
    public interface IServerLegal: IRegName
    {
        LegalObject CheckLegal(ColumnConfig colConfig, ObjectData data);
    }
}
