using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Spoondrift.Code.Xml
{
    public interface IXmlSerial
    {
        void SaveStringBuilder(StringBuilder sb, Formatting formatting);
        void SaveFile(string file);
        string SaveString(Formatting formatting);
    }
}
