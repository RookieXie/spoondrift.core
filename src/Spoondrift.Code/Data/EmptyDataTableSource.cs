using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Data
{
    [CodePlug("EmptyDataTableSource", BaseClass = typeof(IListDataTable),
     CreateDate = "2020-04-15", Author = "xbg", Description = "默认数据源对象")]
    public class EmptyDataTableSource : BaseDataTableSource
    {
        public EmptyDataTableSource(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
