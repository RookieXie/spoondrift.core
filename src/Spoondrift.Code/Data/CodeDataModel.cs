using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Data
{
    public class CodeDataModel: ObjectData
    {
        public string CODE_VALUE { get; set; }
        private string code_text;
        public string CODE_TEXT { get { return HtmlDecode(code_text); } set { code_text = value; } }
        public bool IsSelect { get; set; }
        private string HtmlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = str.Replace("&amp;", "&");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace(" &nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            str = str.Replace("&#39;", "\'");
            return str;
        }
    }
}
