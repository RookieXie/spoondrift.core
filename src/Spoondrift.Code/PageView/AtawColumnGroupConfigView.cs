using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView
{
    public class AtawColumnGroupConfigView
    {
        public int ShowType { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<AtawColumnConfigView> Columns { get; set; }

        public bool IsHidden { get; set; }


    }
}
