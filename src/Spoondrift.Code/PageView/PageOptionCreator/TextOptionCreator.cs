using Spoondrift.Code.PageView.FormViewCreator;
using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView
{
    [CodePlug("Text", BaseClass = typeof(OptionCreator),
        CreateDate = "2012-11-16", Author = "sj", Description = "Text控件参数创建插件")]
    public class TextOptionCreator : DecodeOptionCreator
    {
        private BaseOptions fTextOptions;

        public TextOptionCreator(IServiceProvider provider) :base(provider)
        {
            fTextOptions = new BaseOptions();
            BaseOptions = fTextOptions;
        }
    }
}
