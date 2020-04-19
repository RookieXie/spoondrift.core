using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spoondrift.Code.PageView.PagePlug
{
    [CodePlug("ListPageView", BaseClass = typeof(AtawBasePageViewCreator),
         CreateDate = "2012-11-19", Author = "sj", Description = "ListPageView创建插件")]
    public class AtawListPageViewCreator : AtawBasePageViewCreator
    {
        private AtawListPageConfigView fListPageConfigView;
        public AtawListPageViewCreator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            fListPageConfigView = new AtawListPageConfigView();
            BasePageView = fListPageConfigView;
            PageStyle = Config.PageStyle.List;
        }

        public override AtawPageConfigView Create()
        {
            var pageView = base.Create();
            if (pageView.Header.IsValid)
            {
                var formView = pageView.Forms.FirstOrDefault(a => !a.Key.EndsWith("_SEARCH"));
                formView.Value.HasPager = true;
                //formView.Value.HasSearch = true;
                fListPageConfigView.ListFormName = formView.Key;
                if (formView.Value != null && formView.Value.HasSearch)
                    fListPageConfigView.SearchFormName = formView.Key + "_SEARCH";
            }
            return pageView;
        }
        //private AtawFormConfigView CreateSearchFormView()
        //{
        //    AtawFormConfigView formView = new AtawFormConfigView();
        //    formView.FormType = FormType.Normal;
        //    formView.ShowKind = ShowKind.Tile;
        //    formView.TableName = FormViews.First().Value.TableName;
        //    formView.PrimaryKey = FormViews.First().Value.PrimaryKey;
        //    formView.Title = FormViews.First().Value.Title + "查询";
        //    formView.Name = FormViews.First().Key + "_SEARCH";
        //    formView.Columns = new List<AtawColumnConfigView>();
        //    bool IsSearch = false;
        //    foreach (var column in FormInfoList.First().DataForm.Columns)
        //    {
        //        if (column.Search != null)
        //        {
        //            IsSearch = true;
        //            CreateSearchColumn(formView, column);
        //        }
        //    }
        //    return IsSearch ? formView : null;
        //}

        //private void CreateSearchColumn(AtawFormConfigView formView, ColumnConfig column)
        //{
        //    AtawColumnConfigView colView = new AtawColumnConfigView();
        //    if (column.ControlType == ControlType.Detail)
        //        colView.ControlType = ControlType.Text;
        //    else
        //        colView.ControlType = column.ControlType;
        //    if (column.Search.DateSpan)
        //    {
        //        colView.DisplayName = column.DisplayName + "开始";
        //    }
        //    else
        //        colView.DisplayName = column.DisplayName;
        //    colView.Name = column.Name;
        //    string controlRegname = column.ControlType.ToString();
        //    // to.Options 
        //    var optionCreator = controlRegname.PlugIn<AtawOptionCreator>();
        //    //初始化
        //    optionCreator.Initialize(BasePageView, formView, column, PageStyle.None);
        //    //方法调用
        //    colView.Options = optionCreator.Create();
        //    colView.Options.DataValue = null;
        //    //查询区日期控件需要扩展成2个控件
        //    if (column.Search.DateSpan)
        //    {
        //        colView.DisplayName = column.DisplayName + "开始";
        //        formView.Columns.Add(colView);
        //        PageStyle = PageStyle.None;//避免创建选择器控件或多选控件的时候重复解码CodeTable
        //        var col = CreateColumn(formView, column);
        //        PageStyle = PageStyle.List;
        //        col.DisplayName = column.DisplayName + "结束";
        //        col.Name = column.Name + "_END";
        //        col.Options.DataValue = null;
        //        col.Options.PostSetting.ColumnName = col.Name;
        //        formView.Columns.Add(col);
        //    }
        //    else
        //        formView.Columns.Add(colView);
        //}

        //public override AtawPageConfigView Create()
        //{
        //    var pageView = base.Create();
        //    fListPageConfigView.SearchFormName = FormViews.First().Key + "_SEARCH";
        //    fListPageConfigView.ListFormName = FormViews.First().Key;
        //    //fListPageConfigView.HasPager = ModuleConfig.HasPager;
        //    var searchForm = CreateSearchFormView();
        //    if (searchForm != null)
        //    {
        //        pageView.Forms.Add(searchForm.Name, searchForm);
        //    }
        //    return pageView;
        //}
    }
}
