using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView
{
    public class PageHeader
    {
        /// <summary>
        /// 页面验证是否通过
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }

    }
}
