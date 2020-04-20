using Spoondrift.Code.Config;
using Spoondrift.Code.Config.Form;
using Spoondrift.Code.PlugIn;
using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spoondrift.Code.PageView.FormViewCreator
{
    [CodePlug("ListForm", BaseClass = typeof(BaseFormViewCreator),
     CreateDate = "2012-12-13", Author = "sj", Description = "ListForm创建插件")]
    public class ListFormViewCreator : BaseFormViewCreator
    {
        public ListFormViewCreator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.PageStyle = Config.PageStyle.List;
        }

        public override IEnumerable<FormConfigView> Create()
        {
            var formViews = base.Create();
            var formView = formViews.First();
            var list = DataFormConfig.Columns.Where(a => a.Search != null);
            if (FormConfig.Pager != null)
                formView.HasPager = true;
            //if (FormConfig.HasSearch)
            //{
            //    Debug.AssertEnumerableArgumentNull(list, string.Format("{0}查询字段配置", FormConfig.File), this);
            //    formView.HasSearch = true;
            //    var searchFormView = CreateSearchFormView(formView, list.ToList());
            //    var viewList = formViews.ToList();
            //    viewList.Add(searchFormView);
            //    formViews = viewList;
            //}
            if (FormConfig.HasSearch)
            {
                if (list.Count() > 0)
                {
                    formView.HasSearch = true;
                    var searchFormView = CreateSearchFormView(formView, list.ToList());
                    var viewList = formViews.ToList();
                    viewList.Add(searchFormView);
                    formViews = viewList;
                }
                else
                    formView.HasSearch = false;
            }
            return formViews;
        }

        private FormConfigView CreateSearchFormView(FormConfigView formView, List<ColumnConfig> columns)
        {
            FormConfigView searchFormView = new FormConfigView();
            searchFormView.FormType = FormType.Normal;
            searchFormView.ShowKind = ShowKind.Tile;
            searchFormView.TableName = formView.TableName + "_SEARCH";
            searchFormView.PrimaryKey = formView.PrimaryKey;
            searchFormView.Title = formView.Title + "查询";
            searchFormView.Name = formView.Name + "_SEARCH";
            searchFormView.Columns = new List<ColumnConfigView>();
            foreach (var column in columns)
            {
                CreateSearchColumn(searchFormView, column);
            }
            return searchFormView;
        }

        private void CreateSearchColumn(FormConfigView formView, ColumnConfig column)
        {
            ColumnConfigView colView = new ColumnConfigView();
            if (column.IsReadOnly)
            {
                column.IsReadOnly = false;
            }

            if (column.ControlType == ControlType.TreeMultiSelector)
            {
                colView.ControlType = ControlType.TreeSingleSelector;
            }
            else if (column.ControlType == ControlType.Detail || column.ControlType == ControlType.TextArea || (column.Search != null && column.Search.IsLike == true))//添加如果islike为true
                colView.ControlType = ControlType.Text;
            //else if (column.Search.ControlType != ControlType.None)
            //{
            //    colView.ControlType = column.Search.ControlType;
            //}
            else if (column.ControlType == ControlType.SingleCheckBox)
            {
                colView.ControlType = ControlType.Combo;
                column.RegName = "SingleCheckBoxIsOrNo";
            }
            else
                colView.ControlType = column.ControlType;
            //搜索区的控件类型当为搜索字段配置的控件类型
            if (column.Search.ControlType != ControlType.None)
            {
                colView.ControlType = column.Search.ControlType;
            }
            if (column.Search.DateSpan)
            {
                colView.DisplayName = column.DisplayName + "开始";
            }
            else
                colView.DisplayName = column.DisplayName;
            colView.DisplayName = column.DisplayName;
            colView.Name = column.Name;

            string controlRegname = colView.ControlType.ToString();
            // to.Options 
            var optionCreator = provider.GetCodePlugService<OptionCreator>(controlRegname);// controlRegname.CodePlugIn<OptionCreator>();
            //初始化
            optionCreator.Initialize(BasePageView, formView, column, PageStyle.None);
            //方法调用
            colView.Options = optionCreator.Create();

            if (!column.DetailRegName.IsEmpty())
            {
                colView.Options.RegName = column.DetailRegName;
            }
            if (column.Search != null && column.Search.IsOpenByDefault == true)
            {
                colView.Options.IsOpenByDefault = true;
            }
            colView.Options.DataValue = null;
            if (column.ControlType == ControlType.Text) //Text字段，模糊查询
                colView.Options.PostSetting.ColumnName = colView.Name + "_LIKE";
            //如果islike为true则
            if (column.Search != null && column.Search.IsLike == true)
            {
                colView.Options.PostSetting.ColumnName = colView.Name + "_LIKE";
                colView.Options.IsLike = true;
            }
            if (column.Search.IsExtension) //查询扩展
                colView.Options.PostSetting.ColumnName = colView.Name + "_IN";

            //查询区日期控件需要扩展成2个控件
            if (column.Search.DateSpan)
            {
                colView.DisplayName = column.DisplayName + "开始";
                formView.Columns.Add(colView);
                PageStyle = PageStyle.None;//避免创建选择器控件或多选控件的时候重复解码CodeTable
                var col = CreateColumn(formView, column);
                PageStyle = PageStyle.List;
                col.DisplayName = column.DisplayName + "结束";
                col.Name = column.Name + "_END";
                col.ControlType = colView.ControlType;
                col.Options.DataValue = null;
                col.Options.PostSetting.ColumnName = col.Name;
                formView.Columns.Add(col);
            }
            else
                formView.Columns.Add(colView);
        }


        protected override ColumnConfigView CreateColumn(FormConfigView formView, ColumnConfig column)
        {
            var col = base.CreateColumn(formView, column);

            switch (column.ControlType)
            {
                case ControlType.Hidden:
                    col.ControlType = ControlType.Hidden;
                    break;
                case ControlType.Date:
                    col.ControlType = ControlType.DetailDate;
                    break;
                case ControlType.MultiFileUpload:
                case ControlType.SingleFileUpload:
                    col.ControlType = ControlType.FileDetail;
                    break;
                case ControlType.SingleImageUpload:
                case ControlType.MultiImageUpload:
                    col.ControlType = ControlType.ImageDetail;
                    break;
                case ControlType.Editor:
                    col.ControlType = ControlType.EditorDetail;
                    break;
                case ControlType.InnerPage:
                    col.ControlType = ControlType.InnerPage;
                    break;
                case ControlType.InnerForm:
                    col.ControlType = ControlType.InnerForm;
                    break;
                //case ControlType.MaskCode:
                //    col.ControlType = ControlType.MaskCode;
                //    break;
                case ControlType.Amount:
                    col.ControlType = ControlType.AmountDetail;
                    break;
                case ControlType.AmountDetail:
                    col.ControlType = ControlType.AmountDetail;
                    break;
                case ControlType.TwoColumns:
                    col.ControlType = ControlType.TwoColumnsDetail;
                    break;
                case ControlType.TwoColumnsDetail:
                    col.ControlType = ControlType.TwoColumnsDetail;
                    break;
                case ControlType.TextArea:
                    col.ControlType = ControlType.DetailArea;
                    break;
                case ControlType.DetailArea:
                    col.ControlType = ControlType.DetailArea;
                    break;
                case ControlType.JsonForm:
                    col.ControlType = ControlType.JsonForm;
                    break;
                case ControlType.XFormCol:
                    col.ControlType = ControlType.XFormColDetail;
                    break;

                case ControlType.AllImageShow:
                    col.ControlType = ControlType.AllImageShow;
                    break;

                case ControlType.ImageDetail:
                    col.ControlType = ControlType.ImageDetail;
                    break;

                default:

                    col.ControlType = ControlType.Detail;
                    break;

            }

            //if (column.ControlType != ControlType.Hidden)
            //    col.ControlType = ControlType.Detail;
            //if (column.ControlType == ControlType.Date)
            //    col.ControlType = ControlType.DetailDate;
            //if (column.ControlType == ControlType.SingleFileUpload)
            //    col.ControlType = ControlType.DownLink;
            //if (column.ControlType == ControlType.SingleImageUpload || column.ControlType == ControlType.MultiImageUpload)
            //    col.ControlType = ControlType.ImageDetail;
            ////case ControlType.Editor:
            ////     col.ControlType = ControlType.EditorDetail;
            ////     break;
            //if (column.ControlType == ControlType.Editor)
            //    col.ControlType = ControlType.EditorDetail;
            return col;
        }
    }
}
