using Spoondrift.Code.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView
{
    public class FormConfigView : BaseFormConfigView
    {
        //public string Name { get; set; }

        //public string Title { get; set; }

        public string TableName { get; set; }

        public string PrimaryKey { get; set; }

        public string ParentKey { get; set; }

        public FormType FormType { get; set; }


        public bool VerticalTab { get; set; }
        //public ShowKind ShowKind { get; set; }

        /// <summary>
        ///修改时，是否批量新增
        /// </summary>
        public bool HasBatchInsert { get; set; }

        /// <summary>
        /// 列表是否支持查询
        /// </summary>
        public bool HasSearch { get; set; }

        /// <summary>
        /// 是否分页
        /// </summary>
        public bool HasPager { get; set; }

        /// <summary>
        /// 是否带导航
        /// </summary>
        public bool HasNavigation { get; set; }

        public List<ColumnConfigView> Columns { get; set; }

       public List<ColumnGroupConfigView> ColumnGroups { get; set; }

        public List<NaviColumnConfigView> NavigationColumns { get; set; }

        public PageStyle Action { get; set; }

        /// <summary>
        /// 是否子表
        /// </summary>
        public bool IsDetailForm { get; set; }

        public string AfterInitFunName { get; set; }
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

        public string CodeValueFieldName { get; set; }

        public string CodeTextFieldName { get; set; }

        public bool IsSafeMode { get; set; }

        /// <summary>
        /// 列表是否支持评论
        /// </summary>
        public bool HasReview { get; set; }

        public bool IsInner { get; set; }
        public bool HaveNoSwitchForm { get; set; }
        public bool HaveNoSortBar { get; set; }

        public string Tpl { get; set; }

        public CalendarConfig Calendar { get; set; }

        public bool DisableColumnGroup { get; set; }

        public ExpandDetailConfig ExpandDetailPlug { get; set; }

        public string RowTpl { get; set; }

        public string ContentTpl { get; set; }

    }
}
