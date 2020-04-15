using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Data
{
    public class UniqueRowArrange : List<UniqueRow>
    {

        public bool CheckInsert(UniqueRow row)
        {
            foreach (UniqueRow r in this)
            {
                if (r.EquelCheck(row))
                {
                    return false;
                }
            }

            this.Add(row);
            return true;
        }
    }
}
