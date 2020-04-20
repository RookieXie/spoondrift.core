using Spoondrift.Code.Config;
using Spoondrift.Code.Config.Form;
using Spoondrift.Code.Data;
using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView.FormViewCreator
{
    public abstract class OptionCreator : IRegName
    {
        public string CodePlugName { get; set; }
        public PageConfigView PageView { get; set; }

        public IListDataTable FormData { get; set; }

        public FormConfigView FormView { get; set; }

        public ColumnConfig Config { get; set; }

        public PageStyle ViewPageStyle { get; set; }

        public abstract BaseOptions Create();

        public void Initialize(PageConfigView pageView, FormConfigView formView, ColumnConfig columnConfig, PageStyle style)
        {
            this.PageView = pageView;
            this.FormView = formView;
            this.Config = columnConfig;
            this.ViewPageStyle = style;
        }
    }
}
