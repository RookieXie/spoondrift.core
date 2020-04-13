using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Spoondrift.Code.Data
{
    public class ObjectData 
    {
        public HashSet<string> MODEFY_COLUMNS
        {
            get;
            set;
        }
        public DataRow Row
        {
            get;
            set;
        }

        public string BUTTON_RIGHT
        {
            get;
            set;
        }
    }
}
