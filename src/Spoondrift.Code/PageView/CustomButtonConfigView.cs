using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView
{
    public class CustomButtonConfigView
    {
        public string Name { get; set; }

        public string Text { get; set; }

        public bool Unbatchable { get; set; }

        public ClientConfig Client { get; set; }

        public ServerConfig Server { get; set; }

        public string Icon { get; set; }
        public string BtnCss { get; set; }
        //public string RegName
        //{
        //    get { return Name; }
        //}
    }
}
