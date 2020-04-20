using Spoondrift.Code.Config;
using Spoondrift.Code.Config.Form;
using Spoondrift.Code.Config.Module;
using Spoondrift.Code.Data;
using Spoondrift.Code.PlugIn;
using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spoondrift.Code.PageView.FormViewCreator
{
    public abstract class BaseFormViewCreator:IRegName
    {
        public string CodePlugName { get; set; }
        /// <summary>
        /// 请求上下文 获取当前登录人信息使用
        /// </summary>
        //protected IHttpContextAccessor httpContextAccessor { get; }
        //private IUnitOfDapper fDbContext;
        protected IServiceProvider provider;
        public BaseFormViewCreator(IServiceProvider serviceProvider)
        {
            provider = serviceProvider;
            //httpContextAccessor = provider.GetService<IHttpContextAccessor>();

            //PageItems = AppContext.Current.PageFlyweight.PageItems;
            //if (!DataXmlPath.IsEmpty())
            //    DataFormConfig = DataXmlPath.PlugInPageGet<DataFormConfig>();
        }
        public FormConfig FormConfig { get; set; }

        protected DataFormConfig DataFormConfig { get; set; }

        public PageStyle PageStyle { get; set; }

        public PageConfigView BasePageView { get; set; }

        public ModuleConfig ModuleConfig { get; set; }

        protected List<FormConfigView> FormViews { get; set; }

        public void Initialize(ModuleConfig moduleConfig, FormConfig formConfig, PageConfigView basePageView)
        {
            ModuleConfig = moduleConfig;
            FormConfig = formConfig;
            BasePageView = basePageView;
            FormViews = new List<FormConfigView>();
        }

        public virtual IEnumerable<FormConfigView> Create()
        {
            DataFormConfig = XmlUtil.PlugGetXml<DataFormConfig>(FormConfig.File); //FormConfig.File.InstanceByPage<DataFormConfig>(FormConfig.Name);
            var dt = provider.GetCodePlugService<IListDataTable>(FormConfig.DataPlug);// FormConfig.DataPlug.InstanceByPage<IListDataTable>(FormConfig.Name);
            //合并重写后的所有字段
            MergeDelColumns(FormConfig, DataFormConfig, ModuleConfig, this.PageStyle);
            DataFormConfig.Columns = DataFormConfig.Columns.OrderBy(col => col.Order).ToList();

            //分组相关
            if (FormConfig.Group.IsDisabled == null)
                FormConfig.Group.IsDisabled = "false";
            var groupDisabled = FormConfig.Group.IsDisabled;
            if ((groupDisabled.ToLower() != "true" || groupDisabled != "1") && (FormConfig.Group.ShowPage & PageStyle) != PageStyle)
            {
                FormConfig.Group.IsDisabled = "true";
            }
            //if ((groupDisabled.ToLower() != "true" || groupDisabled != "1") && DataFormConfig.ColumnGroups.Count > 0)
            //{
            //    //所有字段按照分组重新排序
            //    DataFormConfig.Columns.Clear();
            //    DataFormConfig.ColumnGroups.ForEach(x =>
            //    {
            //        x.Columns = x.Columns.OrderBy(col => col.Order).ToList();
            //        DataFormConfig.Columns.AddRange(x.Columns);
            //    });
            //}

            DataFormConfig.Columns.ForEach(a =>
            {
                if (a.ControlLegal != null)
                {
                    DataFormConfig.ColumnLegals.Add(a.ControlLegal.Kind.ToString());
                }
            });

            var formView = CreateFormView(DataFormConfig, FormConfig);
            //主键赋值
            var keyCol = DataFormConfig.Columns.FirstOrDefault(col => col.IsKey);
            formView.PrimaryKey = keyCol == null ? "" : keyCol.Name;
            if (formView.PrimaryKey.IsEmpty())
                formView.PrimaryKey = dt.PrimaryKey;
            else
            {
                if (dt.PrimaryKey.IsEmpty())
                    dt.PrimaryKey = formView.PrimaryKey;
                DataFormConfig.PrimaryKey = formView.PrimaryKey;
            }
            var parentKeyCol = DataFormConfig.Columns.FirstOrDefault(col => col.IsParentColumn);
            formView.ParentKey = parentKeyCol == null ? "" : parentKeyCol.Name;
            formView.IsDetailForm = FormConfig.IsDetailForm;
            formView.Action = PageStyle;
            FormViews.Add(formView);
            return FormViews;
        }

        private  ColumnRightConfig GetColumnRight(FormConfig form, DataFormConfig dataForm, ModuleConfig moduleConfig, PageStyle pagestyle)
        {
            ColumnRightConfig columnRight = null;
            string columnRightname = form.FormColumnRight.Name;
            if (form.FormColumnRight.RegName.IsEmpty())
            {
                //Debug.AssertArgumentNullOrEmpty(columnRightname, "FormColumnRight中若没指定RegName,Name属性不能为空", moduleConfig);

                columnRight = moduleConfig.Right.ColumnRights.FirstOrDefault(a => a.Name == columnRightname);
                //Debug.AssertArgumentNull(columnRight, string.Format("ModuleXml中必须配置名为'{0}'的ColumnRight", columnRightname), moduleConfig);
            }
            else
            {


                IColumnRight columnRightPlug = provider.GetCodePlugService<IColumnRight>(form.FormColumnRight.RegName); //form.FormColumnRight.RegName.CodePlugIn<IColumnRight>();
                columnRightname = columnRightPlug.GetColumnRightName();
                columnRight = moduleConfig.Right.ColumnRights.FirstOrDefault(a => a.Name == columnRightname);
                //Debug.AssertArgumentNull(columnRight, string.Format("ModuleXml中必须配置名为'{0}'的ColumnRight", columnRightname), moduleConfig);

            }
            return columnRight;
        }

        private void MergeDelColumns(FormConfig form, DataFormConfig dataForm, ModuleConfig moduleConfig, PageStyle pagestyle)
        {
            if (form.FormColumnRight != null)
            {
                ColumnRightConfig columnRight = GetColumnRight(form, dataForm, moduleConfig, pagestyle);
                if (columnRight.Delete != null)
                {
                    columnRight.Delete.ForEach(dCol =>
                    {
                        var col = dataForm.Columns.FirstOrDefault(baseCol => baseCol.Name.Equals(dCol.Name, StringComparison.OrdinalIgnoreCase));
                        if (col != null)
                            dataForm.Columns.Remove(col);
                    });
                }
            }
        }

        public  void MergeColumns(FormConfig form, DataFormConfig dataForm, ModuleConfig moduleConfig, PageStyle pagestyle, out List<ColumnConfig> fullColumns)
        {
            if (form.FormColumnRight != null)
            {
                ColumnRightConfig columnRight = GetColumnRight(form, dataForm, moduleConfig, pagestyle);
                if (columnRight.Delete != null)
                {
                    columnRight.Delete.ForEach(dCol =>
                    {
                        var col = dataForm.Columns.FirstOrDefault(baseCol => baseCol.Name.Equals(dCol.Name, StringComparison.OrdinalIgnoreCase));
                        if (col != null)
                            dataForm.Columns.Remove(col);
                    });
                }
                if (columnRight.Override != null)
                {
                    columnRight.Override.ForEach(oCol =>
                    {
                        var col = dataForm.Columns.FirstOrDefault(baseCol => baseCol.Name.Equals(oCol.BaseColumnName, StringComparison.OrdinalIgnoreCase));
                        if (col != null)
                        {
                            AssignColumn(oCol, col);
                        }
                    });
                }
                if (columnRight.Add != null)
                    dataForm.Columns.AddRange(columnRight.Add);


            }

            fullColumns = new List<ColumnConfig>();

            fullColumns = dataForm.Columns;

            dataForm.Columns = dataForm.Columns.Where(a => a.ShowPage.HasFlag(pagestyle)).ToList();
        }

        public static void AssignColumn(OverrideColumnConfig oCol, ColumnConfig baseCol)
        {
            if (!oCol.ChangeEventFun.IsEmpty())
            {
                baseCol.ChangeEventFun = oCol.ChangeEventFun;
            }

            if (!oCol.Prompt.IsEmpty())
                baseCol.Prompt = oCol.Prompt;
            if (!oCol.ValPrompt.IsEmpty())
                baseCol.ValPrompt = oCol.ValPrompt;

            //if (!oCol.TplContainerHtml.IsAkEmpty())
            //{
            //    baseCol.TplContainerHtml = oCol.TplContainerHtml;
            //}

            if (!oCol.DisplayName.IsEmpty())
                baseCol.DisplayName = oCol.DisplayName;
            if (oCol.ControlType != ControlType.None)
                baseCol.ControlType = oCol.ControlType;
            if (oCol.Order != 0)
                baseCol.Order = oCol.Order;
            if (oCol.Search != null)
                baseCol.Search = oCol.Search;
            if (oCol.Selector != null)
                baseCol.Selector = oCol.Selector;
            if (oCol.Navigation != null)
                baseCol.Navigation = oCol.Navigation;
            //if (oCol.TreeConfig != null)
            //{
            //    baseCol.TreeConfig = oCol.TreeConfig;
            //}
            if (!oCol.InternalShowPage.IsEmpty())
            {
                var arr = oCol.InternalShowPage.Split('|');
                baseCol.ShowPage = PageStyle.None;
                arr.ToList().ForEach(str =>
                {
                    baseCol.ShowPage = baseCol.ShowPage | str.ToEnum<PageStyle>();
                });
            }
            //if (oCol.DefaultValue != null)
            //{
            //    if (oCol.DefaultValue.Value.IsEmpty())
            //    {
            //        baseCol.DefaultValueStr = string.Empty;
            //    }
            //    else if (!oCol.DefaultValue.NeedParse)
            //        oCol.DefaultValueStr = oCol.DefaultValue.Value;
            //    else
            //        oCol.DefaultValueStr = MacroExpression.Execute(oCol.DefaultValue.Value);
            //}
            if (!oCol.RegName.IsEmpty())
            {
                baseCol.RegName = oCol.RegName;
            }
            if (!oCol.DetailRegName.IsEmpty())
            {
                baseCol.DetailRegName = oCol.DetailRegName;
            }
            if (oCol.IsReadOnly)
            {
                baseCol.IsReadOnly = true;
            }
            else
            {
                baseCol.IsReadOnly = false;
            }
            if (!oCol.LinkFormat.IsEmpty())
            {
                baseCol.LinkFormat = oCol.LinkFormat;
            }
            if (oCol.IsDetailLink)
            {
                baseCol.IsDetailLink = true;
            }
            else
            {
                baseCol.IsDetailLink = false;
            }
            if (oCol.ControlLegal != null)
            {
                baseCol.ControlLegal = oCol.ControlLegal;
            }
        }

        protected FormConfigView CreateFormView(DataFormConfig dataForm, FormConfig formConfig)
        {
            FormConfigView formView = new FormConfigView();
            formView.FormType = formConfig.FormType;
            formView.ShowKind = formConfig.ShowKind;
            formView.AfterInitFunName = formConfig.AfterInitFunName;
            formView.VerticalTab = formConfig.VerticalTab; //布局方式
            formView.HasBatchInsert = false;
            formView.HasPager = false;
            formView.HasSearch = false;
            formView.ParentFieldName = formConfig.ParentFieldName;
            formView.TextFieldName = formConfig.TextFieldName;
            formView.IsParentFieldName = formConfig.IsParentFieldName;
            formView.IsSafeMode = formConfig.IsSafeMode;
            formView.HasReview = formConfig.HasReview;
            formView.Calendar = formConfig.Calendar;
            formView.ExpandDetailPlug = formConfig.ExpandDetailPlug;
            formView.RowTpl = formConfig.RowTpl;
            formView.ContentTpl = formConfig.ContentTpl;
            formView.DisableColumnGroup = formConfig.Group.IsDisabled.ToLower() == "true" || formConfig.Group.IsDisabled == "1" ? true : false;
            // formView.Tpl = formConfig.Tpl;
            //var _bean = formConfig.Tpls.FirstOrDefault(a => a.PageStyle == PageStyle);
            //if (_bean != null)
            //    formView.Tpl = _bean.Item;


            formView.IsInner = formConfig.IsInner;
            formView.HaveNoSwitchForm = formConfig.HaveNoSwitchForm;
            formView.HaveNoSortBar = formConfig.HaveNoSortBar;
            //var NavigationList = dataForm.Columns.Where(a => a.Navigation != null);
            //if (NavigationList.Count() > 0)
            //{
            //    formView.HasNavigation = true;
            //     List<ColumnConfig> columns = NavigationList.ToList();
            //     foreach (var column in columns)
            //     {
            //         ColumnConfigView colView = new ColumnConfigView();
            //         if (column.Navigation != null && column.Navigation.IsAvailable == true && (column.ControlType == ControlType.CheckBox || column.ControlType == ControlType.Combo || column.ControlType == ControlType.Radio || column.ControlType == ControlType.TreeSingleSelector || column.ControlType == ControlType.TreeMultiSelector))
            //         {
            //             if (column.ControlType == ControlType.CheckBox)
            //             {
            //                 colView.ControlType = ControlType.CheckBoxNavi;
            //             }
            //             else if (column.ControlType == ControlType.Radio || column.ControlType == ControlType.Combo)
            //             {
            //                 colView.ControlType = ControlType.RadioNavi;
            //             }
            //             else if (column.ControlType == ControlType.TreeSingleSelector)
            //             {
            //                 colView.ControlType = ControlType.TreeSingleNavi;
            //             }
            //             else
            //             {
            //                 colView.ControlType = ControlType.TreeMultiNavi;
            //             }
            //         }
            //     }
            //}
            formView.TableName = formConfig.TableName;
            //if (formView.TableName.IsEmpty())
            //    formView.TableName = dataForm.TableName;
            //if (formView.TableName.IsEmpty())
            //    formView.TableName = formConfig.DataPlug.PlugInPageGet<IListDataTable>().RegName;

            formView.Title = formConfig.Title;
            //if (formView.Title.IsEmpty())
            //    formView.Title = dataForm.Title;
            //if (formView.Title.IsEmpty())
            //    formView.Title = ModuleConfig.Title;
            if (ModuleConfig.Mode == ModuleMode.None)
            {
                formView.Title = formView.Title;
            }
            else
            {
                switch (PageStyle)
                {
                    case PageStyle.List:
                        formView.Title = formView.Title + "列表";
                        break;
                    case PageStyle.Detail:
                        formView.Title = formView.Title + "明细";
                        break;
                    case PageStyle.Update:
                        formView.Title = formView.Title + "修改";
                        break;
                    case PageStyle.Insert:
                        formView.Title = formView.Title + "新增";
                        break;
                }
            }

            formView.Name = formConfig.Name;
            //if (formView.Name.IsEmpty())
            //    formView.Name = dataForm.Name;
            //if (formView.Name.IsEmpty())
            //    formView.Name = formView.TableName;
            //string msg = string.Format("数据源为{0}插件的form的名称不能为空", formConfig.DataPlug);
            //Debug.AssertNotNullOrEmpty(formView.Name, msg, this);
            formView.Columns = new List<ColumnConfigView>();
            CreateColumns(formView, dataForm, formConfig);

            formView.ColumnGroups = new List<ColumnGroupConfigView>();
            CreateGroupColumns(formView, dataForm);

            formView.NavigationColumns = new List<NaviColumnConfigView>(); //可能有多个navi(目前只有一个) 
            if (PageStyle == PageStyle.List)
            {
                var navigation = dataForm.Columns.FindAll(a => a.Navigation != null && a.Navigation.IsAvailable);
                foreach (var navi in navigation)
                {
                    if (navigation != null)
                    {
                        var viewColumn = formView.Columns.FirstOrDefault(a => a.Name == navi.Name);
                        var viewNavi = CreateColumnNavigation(viewColumn, navi);
                        if (viewNavi != null)
                        {
                            formView.NavigationColumns.Add(viewNavi);
                        }
                    }
                }
            }
            return formView;
        }
        protected virtual NaviColumnConfigView CreateColumnNavigation(ColumnConfigView ColumnConfigView, ColumnConfig columnConfig)
        {
            if (ColumnConfigView == null || columnConfig == null) { return null; }
            try
            {
                ColumnConfigView viewColumn = new ColumnConfigView();
                ColumnConfigView.ObjectClone<ColumnConfigView>(viewColumn);
                // viewColumn = ColumnConfigView.c
                // viewColumn = ColumnConfigView;


                NaviColumnConfigView naviViewColumn = new NaviColumnConfigView();
                naviViewColumn.ChangeEventFun = viewColumn.ChangeEventFun;
                naviViewColumn.DisplayName = viewColumn.DisplayName;
                naviViewColumn.Prompt = viewColumn.Prompt;
                naviViewColumn.ValPrompt = viewColumn.ValPrompt;
                naviViewColumn.Kind = viewColumn.Kind;
                naviViewColumn.Name = viewColumn.Name;
                naviViewColumn.ShowType = viewColumn.ShowType;
                naviViewColumn.NormalStyle = viewColumn.NormalStyle;
                naviViewColumn.Width = viewColumn.Width;
                naviViewColumn.PxWidth = viewColumn.Width.lengToFloat();
                naviViewColumn.IsMulitText = viewColumn.IsMulitText;
                naviViewColumn.PxHeight = viewColumn.PxHeight;
                naviViewColumn.Border = viewColumn.Border;

                naviViewColumn.TdStyle = viewColumn.TdStyle;
                naviViewColumn.TdClass = viewColumn.TdClass;
                naviViewColumn.IsHiddenCol = viewColumn.IsHiddenCol;
                naviViewColumn.Sortable = viewColumn.Sortable;
                naviViewColumn.Options = new BaseOptions();
                viewColumn.Options.ObjectClone(naviViewColumn.Options);
                naviViewColumn.IsRefrech = columnConfig.Navigation.IsRefrech;
                naviViewColumn.IsAvailable = columnConfig.Navigation.IsAvailable;
                naviViewColumn.IsExpand = columnConfig.Navigation.IsExpand;
                if (!string.IsNullOrEmpty(columnConfig.Navigation.RegName))
                {
                    naviViewColumn.Options.RegName = columnConfig.Navigation.RegName;
                }
                ControlType controlType = columnConfig.Navigation.ControlType;
                if (controlType == ControlType.CheckBoxNavi ||
                    controlType == ControlType.RadioNavi ||
                    controlType == ControlType.TreeSingleNavi ||
                    controlType == ControlType.TreeMultiNavi ||
                    controlType == ControlType.SingleRadioNavi ||
                    controlType == ControlType.RadioNavi ||
                    controlType == ControlType.NaviFilter
                    )
                {
                    naviViewColumn.ControlType = controlType;
                }
                else
                {
                    switch (columnConfig.ControlType)
                    {
                        case ControlType.Combo:
                        case ControlType.Radio:
                            naviViewColumn.ControlType = ControlType.RadioNavi;
                            break;
                        case ControlType.CheckBox:
                            naviViewColumn.ControlType = ControlType.CheckBoxNavi;
                            break;
                        case ControlType.SingleCheckBox:
                            naviViewColumn.ControlType = ControlType.SingleRadioNavi;
                            break;
                        case ControlType.TreeSingleSelector:
                            naviViewColumn.ControlType = ControlType.TreeSingleNavi;
                            break;
                        case ControlType.TreeMultiSelector:
                            naviViewColumn.ControlType = ControlType.TreeMultiNavi;
                            break;
                        case ControlType.NaviFilter:
                            naviViewColumn.ControlType = ControlType.NaviFilter;
                            break;
                        default:
                            return null;
                    }
                }
                return naviViewColumn;
            }
            catch
            {
                return null;
            }
        }

        protected virtual void CreateColumns(FormConfigView formView, DataFormConfig dataformConfig, FormConfig formConfig)
        {
            foreach (var column in dataformConfig.Columns)
            {
                if ((column.ShowPage & PageStyle) != PageStyle)
                    continue;
                else
                {
                    //var col = CreateColumn(formView, column);
                    ////列表和明细页面的所有字段控件类型为Detail
                    //if (!column.IsKey && (PageStyle == PageStyle.Detail || PageStyle == PageStyle.List))
                    //    col.ControlType = ControlType.Detail;
                    //formView.Columns.Add(col);
                    formView.Columns.Add(CreateColumn(formView, column));

                    //if (column.Navigation != null && column.Navigation.IsAvailable == true)
                    //{
                    //    formView.NavigationColumns.Add(CreateColumnNavigation(formView, column));
                    //}
                }
            }
        }

        protected virtual void CreateGroupColumns(FormConfigView formView, DataFormConfig dataformConfig)
        {
            //dataformConfig.ColumnGroups.ForEach(group =>
            //{
            //    var columnGroup = new ColumnGroupConfigView();
            //    columnGroup.ShowType = group.ShowType == 0 ? "ColShowType".AppKv<int>(4) : group.ShowType;
            //    columnGroup.Name = group.Name;
            //    columnGroup.DisplayName = group.DisplayName;
            //    columnGroup.Columns = new List<ColumnConfigView>();
            //    var hiddenColCount = 0;
            //    group.Columns.ForEach(col =>
            //    {
            //        var colView = formView.Columns.FirstOrDefault(x => x.Name == col.Name);
            //        if (colView != null)
            //        {
            //            columnGroup.Columns.Add(colView);
            //            if (colView.ControlType == ControlType.Hidden)
            //            {
            //                hiddenColCount++;
            //            }
            //        }

            //    });
            //    //组内没有成员，该组移除，若组内所有字段都是隐藏字段，该组隐藏
            //    if (columnGroup.Columns.Count > 0)
            //    {
            //        formView.ColumnGroups.Add(columnGroup);
            //        if (columnGroup.Columns.Count == hiddenColCount)
            //            columnGroup.IsHidden = true;
            //    }
            //});

        }


        protected virtual ColumnConfigView CreateColumn(FormConfigView formView, ColumnConfig column)
        {
            ColumnConfigView colView = new ColumnConfigView();
            colView.ControlType = column.ControlType;
            colView.DisplayName = column.DisplayName;
            colView.Prompt = column.Prompt;
            colView.ValPrompt = column.ValPrompt;

            //if ((PageStyle == PageStyle.Update || PageStyle == PageStyle.Insert) && colView.ValPrompt != null && column.ShowType > 0)
            //{

            //    column.ShowType = column.ShowType >= 3?0:2;

            //}
            if (column.Changer != null)
            {
                colView.Changer = new ChangerViewConfig();
                colView.Changer.Expression = column.Changer.Expression;
                colView.Changer.DependColumns = column.Changer.DependColumns;
                colView.Changer.NotifyColumns = column.Changer.NotifyColumns;
            }

            if (column.ShowType == 0 ||
                 column.ControlType == ControlType.XFormCol ||
                column.ControlType == ControlType.AllImageShow ||
                column.ControlType == ControlType.Editor ||
                column.ControlType == ControlType.EditorDetail ||
                column.ControlType == ControlType.ImageDetail ||
                column.ControlType == ControlType.PCAS ||
                column.ControlType == ControlType.MultiSelector ||
                column.ControlType == ControlType.MultiImageUpload ||
                column.ControlType == ControlType.SingleImageUpload ||
                column.ControlType == ControlType.SingleFileUpload ||
                column.ControlType == ControlType.MultiFileUpload ||
                column.ControlType == ControlType.FormMultiSelector ||
                column.ControlType == ControlType.TextArea)
            {
                colView.ShowType = 4;// "ColShowType".AppKv<int>(4);
            }
            else
            {
                colView.ShowType = column.ShowType;
            }
            if (column.ControlType == ControlType.Hidden)
            {
                colView.ShowType = 0;
            }
            colView.Sortable = column.Sortable;
            colView.IsDetailLink = column.IsDetailLink;
            colView.Name = column.Name;
            colView.Kind = column.Kind;
            colView.NormalStyle = column.NormalStyle;
            colView.TdStyle = column.TdStyle;
            //colView.TdClass = column.TdClass;
            colView.IsHiddenCol = column.IsHiddenCol;
            colView.Width = column.Width;
            //colView.IsMulitText = column.IsMulitText;
            //colView.Border = column.Border;

            colView.PxWidth = column.Width.lengToFloat();
            colView.PxHeight = column.Height.lengToFloat();
            colView.LinkFormat = column.LinkFormat;
            colView.ChangeEventFun = column.ChangeEventFun;
            //colView.Report = column.Report;
            //colView.TreeConfig = column.TreeConfig;
            colView.ShortCutName = column.ShortCutName;
            //colView.Amount = column.Amount;
            //colView.TextRegName = column.TextRegName;
            //  colView.XForm = column.XForm;

            if (column.ControlType == ControlType.Custom)
            {
                //Debug.AssertArgumentNull(column.CustomControl, "Custom控件类型需要配置CustomControl", column);
                //Debug.AssertArgumentNullOrEmpty(column.CustomControl.ControlType, "CustomControl中必须指定自定义控件类型", column);
            }

            //colView.CustomControl = column.CustomControl;
            //if (column.ControlType == ControlType.Editor)
            //{
            //    colView.Editor = column.Editor;
            //}

            if (!column.SourceName.IsEmpty())
            {
                colView.QingColumnName = column.Name;
            }

            string controlRegname = column.ControlType.ToString();
            // to.Options 
            var optionCreator = provider.GetCodePlugService<OptionCreator>(controlRegname); //controlRegname.CodePlugIn<OptionCreator>();
            //初始化
            optionCreator.Initialize(BasePageView, formView, column, PageStyle);
            //方法调用
            colView.Options = optionCreator.Create();
            return colView;
        }

        //protected virtual ColumnConfigView CreateColumnNavigation(FormConfigView formView, ColumnConfig column)
        //{
        //    ColumnConfigView colView = new ColumnConfigView();
        //    if (column.Navigation != null && column.Navigation.IsAvailable == true && column.ControlType == ControlType.CheckBox)
        //    {
        //        colView.ControlType = ControlType.CheckBoxNavi;
        //    }
        //    else if (column.Navigation != null && column.Navigation.IsAvailable == true && (column.ControlType == ControlType.Radio || column.ControlType == ControlType.Combo))
        //    {
        //        colView.ControlType = ControlType.RadioNavi;
        //    }
        //    else if (column.Navigation != null && column.Navigation.IsAvailable == true && column.ControlType == ControlType.TreeSingleSelector)
        //    {
        //        colView.ControlType = ControlType.TreeSingleNavi;
        //    }
        //    else if (column.Navigation != null && column.Navigation.IsAvailable == true && column.ControlType == ControlType.TreeMultiSelector)
        //    {
        //        colView.ControlType = ControlType.TreeMultiNavi;
        //    }
        //    //else
        //    //{
        //    //    colView.ControlType = column.ControlType;
        //    //}
        //    colView.DisplayName = column.DisplayName;
        //    colView.ShowType = column.ShowType;
        //    colView.Sortable = column.Sortable;
        //    colView.Name = column.Name;
        //    colView.Kind = column.Kind;
        //    string controlRegname = column.ControlType.ToString();
        //    // to.Options 
        //    var optionCreator = controlRegname.CodePlugIn<OptionCreator>();
        //    //初始化
        //    optionCreator.Initialize(BasePageView, formView, column, PageStyle);
        //    //方法调用
        //    colView.Options = optionCreator.Create();
        //    return colView;
        //}

    }
}
