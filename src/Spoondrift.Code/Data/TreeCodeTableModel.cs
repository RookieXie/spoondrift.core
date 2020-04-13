using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Data
{
    public class TreeCodeTableModel : CodeDataModel
    {
        public bool IsParent { get; set; }
        private bool isLeaf;
        public bool IsLeaf
        {
            get { return isLeaf; }
            set
            {
                this.isLeaf = value;
                this.Nocheck = !value;
                //this.chkDisabled = !value;
            }
        }
        //public bool IsLeaf { get; set; }
        public IEnumerable<TreeCodeTableModel> Children;
        public string ParentId { get; set; }
        public bool Open { get; set; }
        public string Arrange { get; set; }
        public object ExtData { get; set; }
        public int Order { get; set; }
        public int LayerLevel { get; set; }
        //public bool chkDisabled { get; set; }
        public bool? Nocheck { get; set; }
        // public bool IsSelect { get; set; }
        public bool IsHidden { get; set; }
    }
}
