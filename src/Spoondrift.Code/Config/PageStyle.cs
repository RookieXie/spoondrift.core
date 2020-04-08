using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Spoondrift.Code.Config
{
    [Flags]
    public enum PageStyle
    {
        [Description("未知")]
        None = 0,
        [Description("列表")]
        List = 1,
        [Description("详情")]
        Detail = 2,
        [Description("编辑")]
        Update = 4,
        [Description("新增")]
        Insert = 8,
        [Description("删除")]
        Delete = 10,
        [Description("评论")]
        Review = 12,
        [Description("所有")]
        All = 15
    }
}
