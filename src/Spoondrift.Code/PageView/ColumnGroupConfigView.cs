using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView
{
    public class ColumnGroupConfigView
    {
        public int ShowType { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<ColumnConfigView> Columns { get; set; }

        public bool IsHidden { get; set; }


    }
}
