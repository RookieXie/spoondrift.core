using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Data
{
    public class UniqueRow
    {
        public List<string> ColumnList
        {
            get;
            set;
        }

        public UniqueRow()
        {
            ColumnList = new List<string>();
        }

        public UniqueRow(params string[] colStr)
            : this()
        {
            foreach (string str in colStr)
                ColumnList.Add(str);
        }

        public bool EquelCheck(UniqueRow row)
        {
            int _thisLength = ColumnList.Count;
            int _thatLength = row.ColumnList.Count;

            int _min = _thisLength < _thatLength ? _thisLength : _thatLength;
            for (int i = 0; i < _min; i++)
            {
                if (ColumnList[i].Trim() != row.ColumnList[i].Trim())
                    return false;
            }
            return true;

        }
    }
}
