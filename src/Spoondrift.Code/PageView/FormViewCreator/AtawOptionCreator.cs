using Spoondrift.Code.Config;
using Spoondrift.Code.Config.Form;
using Spoondrift.Code.Data;
using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView.FormViewCreator
{
    public abstract class AtawOptionCreator : IRegName
    {
        public string CodePlugName { get; set; }
        public AtawPageConfigView PageView { get; set; }

        public IListDataTable FormData { get; set; }

        public AtawFormConfigView FormView { get; set; }

        public ColumnConfig Config { get; set; }

        public PageStyle PageStyle { get; set; }

        public abstract BaseOptions Create();

        public void Initialize(AtawPageConfigView pageView, AtawFormConfigView formView, ColumnConfig columnConfig, PageStyle style)
        {
            this.PageView = pageView;
            this.FormView = formView;
            this.Config = columnConfig;
            this.PageStyle = style;
        }
    }
}
