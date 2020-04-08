using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config
{
    public class FormConfig : BaseFormConfig
    {
        public FormConfig()
        {
            FormType = FormType.Grid;
           
            Group = new GroupConfig();
        }

        /// <summary>
        /// 是否是主表，整个module xml  只能有一个 序列化完成后要做判断
        /// </summary>
        [XmlAttribute]
        public bool IsMainTable { get; set; }

        //public bool HasPager { get; set; }

        /// <summary>
        /// 分页配置
        /// </summary>
        public PagerConfig Pager { get; set; }

        /// <summary>
        /// 布局配置
        /// </summary>
        //public bool VerticalTab { get; set; }

        /// <summary>
        /// 默认的页面状态
        /// </summary>
        [XmlAttribute]
        public PageStyle Action { get; set; }

        //[XmlAttribute]
        //public string Title { get; set; }

        [XmlAttribute]
        public string File { get; set; }

        [XmlAttribute]
        public string TableName { get; set; }

        [XmlAttribute]
        public FormType FormType { get; set; }

        //[XmlArrayItem("Tpl")]
        //public List<TplConfig> Tpls { get; set; }

        //[XmlAttribute]
        //public ShowKind ShowKind { get; set; }

        /// <summary>
        /// 修改时，是否支持批量新增
        /// </summary>
        [XmlAttribute]
        public bool HasBatchInsert { get; set; }

        /// <summary>
        /// 列表是否支持查询
        /// </summary>
        [XmlAttribute]
        public bool HasSearch { get; set; }

        /// <summary>
        /// 是否带导航
        /// </summary>
        [XmlAttribute]
        public bool HasNavigation { get; set; }

        public string SupportPage { get; set; }

        public string DataPlug { get; set; }


        public string ButtonRightPlug { get; set; }

        public string RowTpl { get; set; }

        public string ContentTpl { get; set; }

        //[XmlArrayItem("Column")]
        //public List<OverrideColumnConfig> Override { get; set; }

        //[XmlArrayItem("Column")]
        //public List<ColumnConfig> Add { get; set; }

        //[XmlArrayItem("Column")]
        //public List<DeleteColumnConfig> Delete { get; set; }

        public FormColumnRightConfig FormColumnRight { get; set; }

        //public MacroConfig AndFilterSql { get; set; }

        [XmlIgnore]
        public bool IsDetailForm { get; set; }

        public string OrderSql { get; set; }

        //public string AfterInitFunName { get; set; }
        public string ParentFieldName
        {
            get;
            set;
        }
        public string TextFieldName
        {
            get;
            set;
        }
        public string IsParentFieldName
        {
            get;
            set;
        }

       // public QingConfig QingConfig { get; set; }

        [XmlAttribute]
        public bool IsSafeMode { get; set; }


        /// <summary>
        /// 列表是否支持评论
        /// </summary>
        [XmlAttribute]
        public bool HasReview { get; set; }


        [XmlAttribute]
        public bool IsInner { get; set; }
        [XmlAttribute]
        public bool HaveNoSwitchForm { get; set; }
        [XmlAttribute]
        public bool HaveNoSortBar { get; set; }

        public CalendarConfig Calendar { get; set; }

        public GroupConfig Group { get; set; }

        public ExpandDetailConfig ExpandDetailPlug { get; set; }

    }
}
