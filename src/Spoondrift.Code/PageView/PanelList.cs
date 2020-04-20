using Spoondrift.Code.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView
{
    /// <summary>
    /// 布局面板列表
    /// </summary>
    public class PanelList
    {
        public ShowKind ShowKind { get; set; }

        public bool VerticalTab { get; set; }

        public List<Panel> Panels { get; set; }
    }
}
