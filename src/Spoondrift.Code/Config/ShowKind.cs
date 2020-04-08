using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Spoondrift.Code.Config
{
    /// <summary>
    /// 表单展现方式
    /// </summary>
    public enum ShowKind
    {
        /// <summary>
        /// 未知
        /// </summary>
        None = 0,
        /// <summary>
        /// 标签
        /// </summary>
        [Description("标签")]
        Tab = 1,
        /// <summary>
        /// 平铺
        /// </summary>
        [Description("平铺")]
        Tile = 2



    }
}
