using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PlugIn
{
    public interface IRegName
    {
        /// <summary>
        /// 注册名 为了解析DI的时候使用
        /// </summary>
        string CodePlugName { get; set; }
    }
}
