using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView
{
    public class BaseOptions
    {
        public string RegName { get; set; }

        public JsDataValue DataValue { get; set; }

        //public string DataObject { get; set; }

        public bool IsKey { get; set; }

        public bool IsParentColumn { get; set; }

        public string Id { get; set; }

        public ControlLegal Legal { get; set; }

        public PostSetting PostSetting { get; set; }

        public bool IsReadOnly { get; set; }

        public string DetialFormatFun { get; set; }

        public bool IsLike { get; set; }

        public string DisplayName { get; set; }
        public string Prompt { get; set; }
        public string ValPrompt { get; set; }

        public bool IsOpenByDefault { get; set; }
        //public UploadConfig Upload { get; set; }

    }
}
