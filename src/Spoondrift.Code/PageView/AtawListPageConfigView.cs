using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView
{
    public class AtawListPageConfigView : AtawPageConfigView
    {
        public string SearchFormName { get; set; }

        public string ListFormName { get; set; }

        public PageSelector PageSelector { get; set; }
        //public bool HasPager { get; set; }
    }
}
