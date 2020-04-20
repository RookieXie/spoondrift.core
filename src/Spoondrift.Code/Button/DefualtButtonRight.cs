using Spoondrift.Code.Config;
using Spoondrift.Code.Data;
using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Button
{
    [CodePlug("DefualtButtonRight", BaseClass = typeof(IButtonRight),
       CreateDate = "2020-04-20", Author = "xbg", Description = "权限插件")]
    public class DefualtButtonRight : IButtonRight
    {
        public string CodePlugName { get; set; }
        private IServiceProvider provider;
        public DefualtButtonRight(IServiceProvider serviceProvider)
        {
            provider = serviceProvider;
        }

        public string GetButtons(ObjectData data, IEnumerable<ObjectData> listData)
        {
            return "Update|Detail|Delete|List|Add";

        }
    }
}
