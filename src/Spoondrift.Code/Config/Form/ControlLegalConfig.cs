using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Config.Form
{
    public class ControlLegalConfig
    {
        public LegalKind Kind { get; set; }

        /// <summary>
        /// 自定义函数
        /// </summary>
        public string CustomLegalFun { get; set; }

        /// <summary>
        /// 正则表达式
        /// </summary>
        public string Reg { get; set; }

        /// <summary>
        /// 验证出错时的提示信息
        /// </summary>
        public string ErrMsg { get; set; }
        /// <summary>
        /// 验证表达式，为空表示验证通过，传入的参数为同一行的控件值
        /// </summary>
        public string LegalExpression { get; set; }

    }
}
