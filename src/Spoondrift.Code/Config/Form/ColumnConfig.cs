using Spoondrift.Code.Data;
using Spoondrift.Code.PlugIn;
using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config.Form
{
    public class ColumnConfig
    {
        public ColumnConfig()
        {
            //Report = new ReportConfig();
            //Score = new ScoreConfig();
            //Editor = new EditorConfig();

        }

        //public EditorConfig Editor
        //{
        //    get;
        //    set;
        //}

        //public ReportConfig Report
        //{
        //    get;
        //    set;
        //}

        [XmlAttribute]
        public bool IsUniqueKey { get; set; }

        [XmlAttribute]
        public bool IsKey { get; set; }

        [XmlAttribute]
        public bool IsParentColumn { get; set; }

        [XmlAttribute]
        public ColumnKind Kind { get; set; }

        [XmlAttribute]
        public bool IsReadOnly { get; set; }

        //[XmlAttribute]
        //public bool IsExtension { get; set; }



        public string Name { get; set; }

        public string DisplayName { get; set; }
        public string Prompt { get; set; }
        public string ValPrompt { get; set; }

        /// <summary>
        /// 虚拟字段的数据源字段
        /// </summary>
        public string SourceName { get; set; }

        public ControlType ControlType { get; set; }

        /// <summary>
        /// 选择器数据源的代码表
        /// </summary>
        public string RegName { get; set; }

        /// <summary>
        /// 选择器解码的代码表
        /// </summary>
        public string DetailRegName { get; set; }

        public int Order { get; set; }
        public int Length { get; set; }
        public XmlDataType DataType { get; set; }

        /// <summary>
        /// 显示格式,指字段占几个单元格,值为0,1,2
        /// </summary>
        [XmlAttribute]
        public int ShowType { get; set; }

        public ChangerConfig Changer { get; set; }
        /// <summary>
        /// 判断是否排序
        /// </summary>
        [XmlAttribute]
        public bool Sortable { set; get; }
        [XmlAttribute]
        public bool IsDetailLink { get; set; }
        [XmlIgnore]
        public PageStyle ShowPage { get; set; }

        [XmlElement("ShowPage")]
        public string InternalShowPage { get; set; }
        public bool IsHiddenCol { get; set; }
        public string ChangeEventFun { get; set; }
        [XmlIgnore]
        public string DefaultValueStr { get; set; }

        public string InnerFormName { get; set; }

        //public MacroConfig DefaultValue { get; set; }

        public SelectorConfig Selector { get; set; }


       // public MarkDownConfig MarkDown { get; set; }


        //public AmountConfig Amount { get; set; }

        public SearchConfig Search { get; set; }

        public ControlLegalConfig ControlLegal { get; set; }

        public string DetialFormatFun { get; set; }

        //public UploadConfig Upload { get; set; }

        //public TreeConfig TreeConfig { get; set; }

        public NavigationConfig Navigation { get; set; }

        //public ScoreConfig Score { get; set; }

        public string GetValue(DataRow row, IServiceProvider provider)
        {
            string _val = row[Name].ToString();
            if (!_val.IsEmpty())
            {
                if (!RegName.IsEmpty())
                {
                    var ioc = provider.GetCodePlugService<CodeTable<CodeDataModel>>(RegName);
                    if (ioc != null)
                    {
                        return ioc[_val].CodeText;
                    }
                }
            }
            return _val;
        }

        //public InnerPageConfig InnerPage { get; set; }
        public string LinkFormat { get; set; }

        public string NormalStyle { get; set; }

        public string Width { get; set; }
        public string Height { get; set; }
        public string TdStyle { get; set; }

        /// <summary>
        /// 快速搜索名，用于前端搜索
        /// </summary>
        public string ShortCutName { get; set; }

        /// <summary>
        /// 自定义控件
        /// </summary>
        //public CustomControlConfig CustomControl { get; set; }

        /// <summary>
        /// PCAS区域选择控件
        /// </summary>
       // public PCASConfig PCAS { get; set; }
    }
}
