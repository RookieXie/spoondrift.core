using Spoondrift.Code.Config;
using Spoondrift.Code.Config.Form;
using Spoondrift.Code.Config.Module;
using Spoondrift.Code.Data;
using Spoondrift.Code.PageView.FormViewCreator;
using Spoondrift.Code.PlugIn;
using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Spoondrift.Code.PageView.PagePlug
{
    public abstract class AtawBasePageViewCreator : IRegName
    {
        public string CodePlugName { get; set; }
        /// <summary>
        /// 请求上下文 获取当前登录人信息使用
        /// </summary>
        //protected IHttpContextAccessor httpContextAccessor { get; }
        //private IUnitOfDapper fDbContext;
        protected IServiceProvider provider;
        public AtawBasePageViewCreator(IServiceProvider serviceProvider)
        {
            provider = serviceProvider;
            //httpContextAccessor = provider.GetService<IHttpContextAccessor>();

            //PageItems = AtawAppContext.Current.PageFlyweight.PageItems;
            //if (!DataXmlPath.IsEmpty())
            //    DataFormConfig = DataXmlPath.PlugInPageGet<DataFormConfig>();
        }
        protected AtawPageConfigView BasePageView { get; set; }

        public ModuleConfig ModuleConfig { get; set; }

        protected Dictionary<string, AtawFormConfigView> FormViews { get; set; }

        protected DataSet PostDataSet { get; set; }

        protected string KeyValue { get; set; }

        //public string ForeignKeyValue { get; set; }

        protected IEnumerable<FormConfigInfo> FormInfoList { get; set; }

        protected PageStyle PageStyle { get; set; }

        protected string FormName { get; set; }

        private IEnumerable<FormConfig> FormConfigs { get; set; }

        //private IEnumerable<MvcFormConfig> MvcFormConfigs { get; set; }

        //private IEnumerable<SeaFormConfig> SeaFormConfigs { get; set; }

        // private IEnumerable<ScriptFormConfig> ScriptFormConfigs { get; set; }

        private bool IsFillEmpty { get; set; }

        public virtual void Initialize(ModuleConfig moduleConfig, DataSet postDataSet, string keyValue, string formName, bool isFillEmpty)
        {
            FormName = formName;
            PostDataSet = postDataSet;
            KeyValue = keyValue;
            IsFillEmpty = isFillEmpty;
            //ForeignKeyValue = foreignKeyValue;
            ModuleConfig = AnalysisModule(moduleConfig);
            this.BasePageView.Data = new DataSet();
        }

        public bool IsSupportPage(string supportPage, PageStyle pagestyle)
        {
            if (!supportPage.IsEmpty() && supportPage != "All")
            {
                var pages = supportPage.Split('|').ToList();
                return pages.Contains(pagestyle.ToString());
            }
            return true;
        }


        /// <summary>
        /// 对ModuleConfig进行再处理,FormName不为空时则只对单个Form处理(例如，多表中对某张表进行查询)
        /// </summary>
        /// <param name="moduleConfig"></param>
        /// <returns></returns>
        private ModuleConfig AnalysisModule(ModuleConfig moduleConfig)
        {
            FormConfigs = moduleConfig.Forms.Where(a => a is FormConfig)
                .Cast<FormConfig>()
                .Where(a => IsSupportPage(a.SupportPage, this.PageStyle));
            //MvcFormConfigs = moduleConfig.Forms.Where(a => a is MvcFormConfig) == null ? null :
            //    moduleConfig.Forms.Where(a => a is MvcFormConfig).Cast<MvcFormConfig>().ToList();
            //SeaFormConfigs = moduleConfig.Forms.Where(a => a is SeaFormConfig) == null ? null :
            //    moduleConfig.Forms.Where(a => a is SeaFormConfig).Cast<SeaFormConfig>().ToList();

            //ScriptFormConfigs = moduleConfig.Forms.Where(a => a is ScriptFormConfig) == null ? null :
            // moduleConfig.Forms.Where(a => a is ScriptFormConfig).Cast<ScriptFormConfig>().ToList();

            //if (moduleConfig.Mode == ModuleMode.MasterDetail)
            //{
            // AtawDebug.AssertArgumentNull(moduleConfig.Relations, "主从表必须配置Relation", this);
            //BasePageView.KeyValue = KeyValue;  //主从表修改时，主表主键需要传递给前台
            // }
            #region 非空属性赋值
            //Form配置中Name或TableName或Title可能没有值，此时需要赋值
            moduleConfig.Forms.ForEach(a =>
            {
                if (a is FormConfig)
                {
                    var form = a as FormConfig;
                    var dataForm = XmlUtil.PlugGetXml<DataFormConfig>(form.File);

                    if (form.TableName.IsEmpty())
                        form.TableName = dataForm.TableName;
                    if (form.TableName.IsEmpty())
                        form.TableName = provider.GetCodePlugService<IListDataTable>(form.DataPlug).CodePlugName; //form.DataPlug.InstanceByPage<IListDataTable>(a.Name).RegName;
                    if (a.Name.IsEmpty())
                    {
                        a.Name = dataForm.Name;
                    }
                    if (a.Name.IsEmpty())
                    {
                        a.Name = form.TableName;
                    }
                    a.Width = form.Width;
                    string msg = string.Format("数据源为{0}插件的form的名称不能为空", form.DataPlug);
                    //AtawDebug.AssertNotNullOrEmpty(form.Name, msg, this);

                    if (a.Title.IsEmpty())
                        a.Title = dataForm.Title;
                    if (a.Title.IsEmpty())
                        a.Title = moduleConfig.Title;
                }
                //if (a is MvcFormConfig)
                //{
                //    var mvcForm = a as MvcFormConfig;
                //    AtawDebug.AssertNotNull(mvcForm.DataRoute, "MvcForm需要配置DataRoute", this);
                //    var dataRoute = AtawAppContext.Current.MvcConfigXml.DataRoutes.FirstOrDefault(route => route.Name == mvcForm.DataRoute.Name);
                //    if (dataRoute != null)
                //    {
                //        mvcForm.DataRoute.ActionName = dataRoute.ActionName;
                //        mvcForm.DataRoute.ControlName = dataRoute.ControlName;
                //        mvcForm.DataRoute.AreaName = dataRoute.AreaName;
                //        mvcForm.DataRoute.NameSpace = dataRoute.NameSpace;
                //    }
                //    else
                //    {
                //        if (mvcForm.DataRoute.ControlName.IsEmpty())
                //        {
                //            AtawDebug.AssertNotNullOrEmpty(mvcForm.DataRoute.ControlName, "DataRoute的ControlName不能为空", this);
                //        }
                //        if (mvcForm.DataRoute.ActionName.IsEmpty())
                //        {
                //            AtawDebug.AssertNotNullOrEmpty(mvcForm.DataRoute.ActionName, "DataRoute的ActionName不能为空", this);
                //        }
                //        if (mvcForm.DataRoute.AreaName.IsEmpty())
                //        {
                //            AtawDebug.AssertNotNullOrEmpty(mvcForm.DataRoute.AreaName, "DataRoute的AreaName不能为空", this);
                //        }
                //        if (mvcForm.DataRoute.NameSpace.IsEmpty())
                //        {
                //            AtawDebug.AssertNotNullOrEmpty(mvcForm.DataRoute.NameSpace, "DataRoute的NameSpace不能为空", this);
                //        }
                //    }
                //}
            });
            #endregion

            #region 主从表批量修改或显示明细时，现只针对主表操作
            if (PostDataSet != null && PostDataSet.Tables.Count > 0)
            {
                DataTable dt = PostDataSet.Tables["_KEY"];
                if (dt != null)
                {
                    if (dt.Rows.Count > 1)
                        moduleConfig.Mode = ModuleMode.Single;
                    //else if (dt.Rows.Count == 1)
                    //    BasePageView.KeyValue = dt.Rows[0][0].ToString();


                    var keyValueList = new List<string>();
                    foreach (DataRow row in dt.Rows)
                    {
                        if (dt.Columns.Contains("KeyValue"))
                        {
                            string _key = row["KeyValue"].ToString();
                            keyValueList.Add(_key);
                        }
                    }
                    BasePageView.KeyValue = String.Join<string>(",", keyValueList);
                }
            }
            #endregion

            if (FormName.IsEmpty())
            {
                if (moduleConfig.Mode == ModuleMode.Single || moduleConfig.Mode == ModuleMode.MasterDetail ||
                    moduleConfig.Mode == ModuleMode.SingleToSingle)
                {
                    var form = FormConfigs.FirstOrDefault(a => a.IsMainTable);
                    var mainForm = form == null ? FormConfigs.FirstOrDefault() : form;//若没有配置主表，则指定第一个为主表
                    mainForm.IsMainTable = true;
                    switch (PageStyle)
                    {
                        case PageStyle.List:
                            moduleConfig.Forms.Clear();
                            mainForm.HasSearch = true;
                            moduleConfig.Forms.Add(mainForm);
                            moduleConfig.HasReview = mainForm.HasReview;
                            break;
                        case PageStyle.Insert:
                        case PageStyle.Update:
                        case PageStyle.Review:
                        case PageStyle.Detail:
                            mainForm.FormType = FormType.Normal;
                            if (moduleConfig.Mode == ModuleMode.Single)
                            {
                                moduleConfig.Forms.Clear();
                                moduleConfig.Forms.Add(mainForm);
                                moduleConfig.HasReview = mainForm.HasReview;
                            }
                            else
                            {
                                //var detailForms = FormConfigs.Where(a => !a.IsMainTable);
                                //foreach (FormConfig item in detailForms)
                                //{
                                //    item.FormType = FormType.Grid;
                                //}
                            }
                            break;
                    }
                }
            }
            else
            {
                var form = moduleConfig.Forms.Find(a => a.Name == FormName);
                //AtawDebug.AssertNotNull(form, string.Format(ObjectUtil.SysCulture, "请求的formname:  {0}不存在,请检查xml文件是否有问题", FormName), this);
                //var mainForm = form == null ? moduleConfig.Forms.First() : form;//若没有配置主表，则指定第一个为主表
                //if (PageStyle == PageStyle.Insert || PageStyle == PageStyle.Detail || PageStyle == PageStyle.Update)
                //{
                //    mainForm.FormType = FormType.Normal;
                //}
                //if (moduleConfig.Mode == ModuleMode.Single)
                //{
                moduleConfig.Forms.Clear();
                moduleConfig.Forms.Add(form);
                // }
            }

            return moduleConfig;
        }

        public virtual void SetReturnUrl()
        {
            BasePageView.ReturnUrl = ModuleConfig.ReturnUrl;
        }

        public virtual AtawPageConfigView Create()
        {
            //BasePageView.Title = ModuleConfig.Title;
            #region 验证
            PageHeader header = ValidHeader();
            if (!header.IsValid)
                return BasePageView;
            #endregion

            #region 按钮初始化



            SetDefaultButton();
            ModuleConfig.Buttons.ToList().ForEach(
                a =>
                {
                    bool isData = a.IsData;
                    var bt = new CustomButtonConfigView()
                    {
                        Client = a.Client,
                        Name = a.Name,
                        Server = a.Server,
                        Text = a.Text,
                        Unbatchable = a.Unbatchable,
                        BtnCss = a.BtnCss,
                        Icon = a.Icon

                    };

                    #region 按钮验证
                    bool IsAuthenticated = true;
                    if (ModuleConfig.Right != null && ModuleConfig.Right.FunctionRights != null)
                    {
                        //var buttonRight = ModuleConfig.Right.FunctionRights.ButtonRights.FirstOrDefault(b => b.ButtonName == a.Name);
                        //if (buttonRight != null)
                        //{
                        //    var rightUnit = ModuleConfig.Right.FunctionRights.RightUnits.FirstOrDefault(b => b.Name == buttonRight.Name);
                        //    //AtawDebug.AssertNotNull(rightUnit, string.Format("需要配置名为{0}的RightUnit", buttonRight.Name), this);
                        //    if (rightUnit.RightType == RightType.MvcFilter)
                        //    {
                        //        var type = RightUtil.RightVerification(rightUnit.RegName);
                        //        IsAuthenticated = type == RightFilterType.Success;
                        //    }
                        //}
                    }
                    #endregion
                    if (IsAuthenticated)
                    {
                        if (isData)
                        {
                            if (!BasePageView.DataButtons.Keys.Contains(a.Name))
                            {
                                BasePageView.DataButtons.Add(a.Name, bt);
                            }
                        }
                        else
                        {
                            if (!BasePageView.PageButtons.Keys.Contains(a.Name))
                            {
                                BasePageView.PageButtons.Add(a.Name, bt);
                            }
                        }
                    }

                }
                );
            #endregion

            Dictionary<string, AtawFormConfigView> formViewDict = new Dictionary<string, AtawFormConfigView>();
            //创建Form
            FormConfigs.ToList().ForEach(form =>
            {
                FillDataSet(form);
                AtawBaseFormViewCreator formViewCreator = null;
                if (PageStyle != PageStyle.None)
                    formViewCreator = provider.GetCodePlugService<AtawBaseFormViewCreator>(PageStyle.ToString() + "Form");// (PageStyle.ToString() + "Form").InstanceByPage<AtawBaseFormViewCreator>(form.Name);
                else
                {
                    form.Action = form.Action == PageStyle.None ? PageStyle.List : form.Action;
                    if (form.Action == PageStyle.Insert)  //ModulePage中如果Form的Action为Insert，则视为Update状态下的批量新增
                    {
                        formViewCreator = provider.GetCodePlugService<AtawBaseFormViewCreator>("UpdateForm"); //("UpdateForm").InstanceByPage<AtawBaseFormViewCreator>(form.Name);
                    }
                    else
                        formViewCreator = provider.GetCodePlugService<AtawBaseFormViewCreator>(form.Action.ToString() + "Form");// (form.Action.ToString() + "Form").InstanceByPage<AtawBaseFormViewCreator>(form.Name);
                }
                formViewCreator.Initialize(ModuleConfig, form, BasePageView);
                var formViews = formViewCreator.Create();
                formViews.ToList().ForEach(view =>
                {
                    formViewDict.Add(view.Name, view);
                });
                //viewDict.Add(formView.Name, formView);
                //var info = new FormConfigInfo();
                //info.DataForm = dataForm;
                //info.FormConfig = a;
                //info.FormView = formView;
                //infoList.Add(info);
            });

            //Dictionary<string, AtawMvcFormConfigView> mvcFormViewDict = new Dictionary<string, AtawMvcFormConfigView>();
            ////创建MvcForm
            //if (MvcFormConfigs != null)
            //{
            //    MvcFormConfigs.Cast<MvcFormConfig>().ToList().ForEach(a =>
            //    {
            //        AtawMvcFormConfigView mvcFormView = new AtawMvcFormConfigView();
            //        mvcFormView.Title = a.Title;
            //        mvcFormView.Name = a.Name;
            //        mvcFormView.ShowType = a.ShowType;
            //        mvcFormView.ShowKind = a.ShowKind;
            //        mvcFormView.DataRoute = a.DataRoute;
            //        mvcFormViewDict.Add(mvcFormView.Name, mvcFormView);
            //    });
            //}

            //Dictionary<string, AtawSeaFormConfigView> seaFormViewDict = new Dictionary<string, AtawSeaFormConfigView>();
            ////创建MvcForm
            //if (SeaFormConfigs != null)
            //{
            //    SeaFormConfigs.Cast<SeaFormConfig>().ToList().ForEach(a =>
            //    {
            //        if (a.Plug.IsAkEmpty())
            //        {
            //            a.Plug = "BaseSeaCreator";
            //        }
            //        var _creator = a.Plug.CodePlugIn<ISeaCreator>();
            //        _creator.postDataSet = PostDataSet;
            //        AtawSeaFormConfigView seaFormView = _creator.create(a);
            //        seaFormViewDict.Add(seaFormView.Name, seaFormView);

            //        //seaFormView.P1 = this.PostDataSet.Tables[0].Compute
            //    });
            //}


            //Dictionary<string, AtawScriptFormConfigView> scriptFormViewDict = new Dictionary<string, AtawScriptFormConfigView>();
            ////创建MvcForm
            //if (ScriptFormConfigs != null)
            //{
            //    ScriptFormConfigs.Cast<ScriptFormConfig>().ToList().ForEach(a =>
            //    {
            //        AtawScriptFormConfigView scriptFormView = new AtawScriptFormConfigView();
            //        scriptFormView.Title = a.Title;
            //        scriptFormView.Name = a.Name;
            //        scriptFormView.ShowKind = a.ShowKind;
            //        if (a.ScriptFunName.IsEmpty())
            //        {
            //            scriptFormView.ScriptFormFunName = a.Name;
            //        }
            //        else
            //        {
            //            scriptFormView.ScriptFormFunName = a.ScriptFunName;
            //        }
            //        scriptFormViewDict.Add(scriptFormView.Name, scriptFormView);
            //    });
            //}




            //设置布局:当form不以tab连续布局时，则默认tile布局
            #region Layout
            BasePageView.Layout = new List<PanelList>();
            var previousKind = ShowKind.None;
            int i = 0;
            //int count = 0;
            ModuleConfig.Forms.ForEach(a =>
            {

                PanelList panelList = null;
                if (ModuleConfig.Forms.Count > 1)
                {
                    if (i == 0)
                    {

                        if (a.ShowKind == ShowKind.Tab && ModuleConfig.Forms[i + 1].ShowKind == ShowKind.Tile)
                        {
                            a.ShowKind = ShowKind.Tile;
                        }
                    }
                    else if (i == ModuleConfig.Forms.Count - 1)
                    {
                        if (a.ShowKind == ShowKind.Tab && ModuleConfig.Forms[i - 1].ShowKind == ShowKind.Tile)
                        {
                            a.ShowKind = ShowKind.Tile;
                        }
                    }
                    else if (a.ShowKind == ShowKind.Tab && ModuleConfig.Forms[i - 1].ShowKind == ShowKind.Tile &&
                        ModuleConfig.Forms[i + 1].ShowKind == ShowKind.Tile)
                    {
                        a.ShowKind = ShowKind.Tile;
                    }
                    i++;
                }
                else
                    a.ShowKind = ShowKind.Tile;

                if (previousKind != a.ShowKind)
                {
                    panelList = new PanelList();

                    //panelList.VerticalTab = a.VerticalTab;
                    //if (panelList.VerticalTab)
                    //{
                    //    count++;

                    //}
                    //else if (count > 0)
                    //{
                    //    panelList.VerticalTab = false ? true : true;
                    //}
                    panelList.ShowKind = a.ShowKind;
                    previousKind = a.ShowKind;
                    List<Panel> panels = new List<Panel>();
                    Panel panel = new Panel();
                    panel.FormName = a.Name;
                    panels.Add(panel);
                    panelList.Panels = panels;
                    BasePageView.Layout.Add(panelList);
                }
                else
                {
                    Panel panel = new Panel();
                    panel.FormName = a.Name;
                    //BasePageView.Layout[BasePageView.Layout.Count - 1].VerticalTab = a.VerticalTab;
                    //if (BasePageView.Layout[BasePageView.Layout.Count - 1].VerticalTab)
                    //{
                    //    count++;
                    //}
                    //if (count > 0)
                    //{
                    //    BasePageView.Layout[BasePageView.Layout.Count - 1].VerticalTab = false ? true : true;
                    //}

                    BasePageView.Layout[BasePageView.Layout.Count - 1].Panels.Add(panel);

                }
            });

            BasePageView.Layout.ForEach(a =>
            {
                if (a.ShowKind == ShowKind.Tab)
                {
                    var form = ModuleConfig.Forms.FirstOrDefault(b => b.VerticalTab);
                    if (form != null)
                        a.VerticalTab = true;
                }
            });
            #endregion

            //FormViews = viewDict;
            //FormInfoList = infoList;
            BasePageView.Title = ModuleConfig.Title;
            BasePageView.PageLayout = ModuleConfig.Layout;
            BasePageView.Route = ModuleConfig.Route;
            BasePageView.IsPart = ModuleConfig.IsPart;

            BasePageView.BeforeHook = ModuleConfig.BeforeHook;
            BasePageView.AfterHook = ModuleConfig.AfterHook;
            BasePageView.TsHook = ModuleConfig.TsHook;
            //BasePageView.TsSkinName = ModuleConfig.TsSkinName;
            //BasePageView.Forms = FormViews;
            SetReturnUrl();
            BasePageView.Forms = formViewDict;
            //BasePageView.MvcForms = mvcFormViewDict;
            //BasePageView.SeaForms = seaFormViewDict;
            //BasePageView.ScriptForms = scriptFormViewDict;
            //动态加载的js赋值
            if (ModuleConfig.Mode == ModuleMode.None)
            {
                //BasePageView.Scripts = ModuleConfig.Scripts.Where(a => a.Style == Core.PageStyle.All || a.Style == Core.PageStyle.None).ToList();
            }
            else
            {
                //var scripts = ModuleConfig.Scripts.Where(a => a.Style == Core.PageStyle.All || a.Style == Core.PageStyle.None || a.Style == PageStyle).ToList();
                //FirstOrDefault(a => (a.Style & PageStyle) == PageStyle);
                //BasePageView.Scripts = scripts;
            }
            BasePageView.Scripts.ForEach(a =>
            {
                if (a != null && !a.Path.IsEmpty() && a.Path.IndexOf("/") != 0) //相对路径
                {
                    a.Path = string.Format("/Scripts/{0}", a);
                }
            });

            // BasePageView.PageSourceData = AtawAppContext.Current.PageFlyweight.PageItems["PageSourceData"].Value<string>();
            //BasePageView.PageCustomerSourceData = AtawAppContext.Current.PageFlyweight.PageItems["PageCustomerSourceData"];
            return BasePageView;
        }

        private PageHeader ValidHeader()
        {
            PageHeader header = new PageHeader();
            header.IsValid = true;
            //if (!AtawAppContext.Current.IsAuthenticated)
            //{
            //    header.IsValid = false;
            //    header.Message = "未登录";
            //}
            //else
            //{
            //    if (PageStyle != PageStyle.None && ModuleConfig.Right != null && ModuleConfig.Right.FunctionRights != null)
            //    {
            //        var pageStyleRight = ModuleConfig.Right.FunctionRights.PageStyleRights.FirstOrDefault(a => a.PageStyle == PageStyle);
            //        if (pageStyleRight != null)
            //        {
            //            var rightUnit = ModuleConfig.Right.FunctionRights.RightUnits.FirstOrDefault(a => a.Name == pageStyleRight.Name);
            //            AtawDebug.AssertNotNull(rightUnit, string.Format("需要配置名为{0}的RightUnit", pageStyleRight.Name), this);
            //            //if (!AtawAppContext.Current.IsAuthenticated)
            //            //{
            //            //    header.IsValid = false;
            //            //    header.Message = "未登录";
            //            //}
            //            //else
            //            //{
            //            if (rightUnit.RightType == RightType.MvcFilter)
            //            {
            //                var type = RightUtil.RightVerification(rightUnit.RegName);
            //                switch (type)
            //                {
            //                    case RightFilterType.UnRenew:
            //                        header.IsValid = false;
            //                        header.Message = "未续费";
            //                        break;
            //                    case RightFilterType.DenyPermission:
            //                        header.IsValid = false;
            //                        header.Message = "没有权限";
            //                        break;
            //                }
            //            }
            //            // }
            //        }
            //    }
            //}
            BasePageView.Header = header;
            return header;
        }

        #region 给source 用的 dataXMl
        public void MergeColumns(FormConfig form, DataFormConfig dataForm, ModuleConfig moduleConfig)
        {
            if (form.FormColumnRight != null)
            {
                ColumnRightConfig columnRight = null;
                string columnRightname = form.FormColumnRight.Name;
                if (form.FormColumnRight.RegName.IsEmpty())
                {
                    //AtawDebug.AssertArgumentNullOrEmpty(columnRightname, "FormColumnRight中若没指定RegName,Name属性不能为空", moduleConfig);
                    columnRight = moduleConfig.Right.ColumnRights.FirstOrDefault(a => a.Name == columnRightname);
                    //AtawDebug.AssertArgumentNull(columnRight, string.Format("ModuleXml中必须配置名为'{0}'的ColumnRight", columnRightname), moduleConfig);
                }
                else
                {
                    IColumnRight columnRightPlug = provider.GetCodePlugService<IColumnRight>(form.FormColumnRight.RegName); //form.FormColumnRight.RegName.CodePlugIn<IColumnRight>();
                    columnRightname = columnRightPlug.GetColumnRightName();
                    columnRight = moduleConfig.Right.ColumnRights.FirstOrDefault(a => a.Name == columnRightname);
                    //AtawDebug.AssertArgumentNull(columnRight, string.Format("ModuleXml中必须配置名为'{0}'的ColumnRight", columnRightname), moduleConfig);
                }
                //if (columnRight.Delete != null)
                //{
                //    columnRight.Delete.ForEach(dCol =>
                //    {
                //        var col = dataForm.Columns.FirstOrDefault(baseCol => baseCol.Name.Equals(dCol.Name, StringComparison.OrdinalIgnoreCase));
                //        if (col != null)
                //            dataForm.Columns.Remove(col);
                //    });
                //}
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
        }

        public void AssignColumn(OverrideColumnConfig oCol, ColumnConfig baseCol)
        {
            if (!oCol.DisplayName.IsEmpty())
                baseCol.DisplayName = oCol.DisplayName;
            if (!oCol.Prompt.IsEmpty())
                baseCol.Prompt = oCol.Prompt;

            //if (!oCol.TplContainerHtml.IsAkEmpty())
            //{
            //    baseCol.TplContainerHtml = oCol.TplContainerHtml;
            //}
            if (!oCol.ValPrompt.IsEmpty())
            {
                baseCol.ValPrompt = oCol.ValPrompt;
            }
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
            if (oCol.ControlLegal != null)
            {
                baseCol.ControlLegal = oCol.ControlLegal;
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
        }
        #endregion


        private string SetDataBtnFilterButtonConfig(string btnStr)
        {
            string result = "";
            string[] btnList = btnStr.Split('|');
            foreach (string btn in btnList)
            {
                if (this.BasePageView.DataButtons.Keys.Contains(btn))
                {
                    result = "{0}{1}{2}".SFormat(result, "|", btn);
                }
            }
            return result;
            // this.BasePageView.DataButtons
        }

        protected virtual void FillDataSet(FormConfig fc)
        {
            IListDataTable dt = provider.GetCodePlugService<IListDataTable>(fc.DataPlug); //fc.DataPlug.InstanceByPage<IListDataTable>(fc.Name);
            dt.PageStyle = PageStyle;
            var dataForm = XmlUtil.PlugGetXml<DataFormConfig>(fc.File); //fc.File.InstanceByPage<DataFormConfig>(fc.Name);
            var keyCols = dataForm.Columns.Where(a => a.IsKey);
            if (keyCols != null)
                //AtawDebug.Assert(keyCols.ToList().Count == 1, string.Format("{0}只能指定一个主键", fc.File), dataForm);
                if (dataForm.PrimaryKey.IsEmpty())
                {
                    dataForm.PrimaryKey = dataForm.Columns.First(col => col.IsKey).Name;
                }
            MergeColumns(fc, dataForm, ModuleConfig);
            //dt.Initialize(this.PostDataSet, ModuleConfig.PageSize, KeyValue, ForeignKeyValue, fc.TableName, dataForm.PrimaryKey);
            int pageSize = 0;
            if (fc.Pager != null)
                pageSize = fc.Pager.PageSize;
            string foreignKey = GetForeignKey(fc);
            if (!foreignKey.IsEmpty())
            {
                string _foreignKeyValue = KeyValue;
                var _relations1 = this.ModuleConfig.Relations.Where(a => a.DetailForm == fc.Name).ToList();
                if (_relations1.Count > 0)
                {
                    var _ra = _relations1.First();
                    string __relationKey = "_foreignkey_{0}_{1}".SFormat(_ra.MasterForm, _ra.MasterField);
                    //if (AtawAppContext.Current.GetItem(__relationKey) != null)
                    //{
                    //    _foreignKeyValue = AtawAppContext.Current.GetItem(__relationKey).ToString();
                    //}
                    //else
                    //{
                    _foreignKeyValue = KeyValue;
                    //}

                }

                dt.Initialize(new ModuleFormInfo(this.PostDataSet, pageSize, KeyValue, _foreignKeyValue,
                    fc.TableName, dataForm.PrimaryKey, foreignKey, IsFillEmpty, dataForm, fc.OrderSql, fc));
            }
            else
                dt.Initialize(new ModuleFormInfo(this.PostDataSet, pageSize, KeyValue, "", fc.TableName,
                    dataForm.PrimaryKey, "", IsFillEmpty, dataForm, fc.OrderSql, fc));

            //  if(this.ModuleConfig.Relations.Find(a=>a.MasterForm == fc.Name))
            var _relations = this.ModuleConfig.Relations.Where(a => a.MasterForm == fc.Name).ToList();
            if (dt.List.Count() > 0)
            {
                var row = dt.List.ToList()[0];
                if (_relations.Count > 0)
                {
                    _relations.ForEach((_relation) =>
                    {

                        string _key = _relation.MasterField;
                        object _value = row.Row[_key];
                        string __relationKey = "_foreignkey_{0}_{1}".SFormat(_relation.MasterForm, _relation.MasterField);
                       // AtawAppContext.Current.SetItem(__relationKey, _value);

                    });
                }
            }

            if (PageStyle != PageStyle.Insert)
            {
                var _list = dt.List;
                var __list = _list.ToList();
                __list.ForEach(a =>
                {
                    if (!fc.ButtonRightPlug.IsEmpty())
                    {
                        var gh = provider.GetCodePlugService<IButtonRight>(fc.ButtonRightPlug);// fc.ButtonRightPlug.CodePlugIn<IButtonRight>();
                        var buttons = gh.GetButtons(a, dt.List);
                        a.BUTTON_RIGHT = SetDataBtnFilterButtonConfig(buttons);
                    }
                    if (fc.HasReview)
                    {
                        a.BUTTON_RIGHT = a.BUTTON_RIGHT + "|Review";
                    }
                });

                dt.AppendTo(this.BasePageView.Data);
                //给数据源添加虚拟字段
                var dataTable = this.BasePageView.Data.Tables[dt.CodePlugName];
                if (dataTable != null)
                {
                    var virtualCols = dataForm.Columns.Where(a => a.Kind == ColumnKind.Virtual && !a.SourceName.IsEmpty());
                    if (virtualCols != null)
                    {
                        foreach (var col in virtualCols)
                        {
                            dataTable.Columns.Add(col.Name);
                            foreach (DataRow row in dataTable.Rows)
                            {
                                row[col.Name] = row[col.SourceName].ToString();
                            }
                        }
                    }
                }

            }
        }

        private string GetForeignKey(FormConfig fc)
        {
            string foreignKey = "";
            if (this.ModuleConfig.Relations != null)
            {
                var relation = this.ModuleConfig.Relations.FirstOrDefault(a => a.DetailForm == fc.Name);
                if (relation != null)
                {

                    foreignKey = relation.DetailField;
                    if (relation.DetailField != relation.MasterField)
                    {
                        fc.IsDetailForm = true;
                    }
                }
            }
            return foreignKey;
        }

        private void CreatePageStyleButton(string[] pages, PageStyle pageStyle, bool isData)
        {
            if (pages == null || pages.Length == 0 || pages.Contains(PageStyle.All.ToString()) || pages.Contains(pageStyle.ToString()))
            {
                CreateButton(pageStyle, isData);
            }
        }

        private void SetDefaultButton()
        {
            List<CustomButtonConfig> list = new List<CustomButtonConfig>();
            if (ModuleConfig.SupportPage.IsEmpty())
                ModuleConfig.SupportPage = PageStyle.All.ToString();
            //if (!ModuleConfig.SupportPage.IsEmpty())
            //{
            var pages = ModuleConfig.SupportPage.Split('|');
            // if(pages )

            CreatePageStyleButton(pages, PageStyle.Insert, false);

            CreatePageStyleButton(pages, PageStyle.Delete, true);

            CreatePageStyleButton(pages, PageStyle.Detail, true);

            CreatePageStyleButton(pages, PageStyle.Update, true);

            if (ModuleConfig.HasReview == true)
            {
                CreatePageStyleButton(pages, PageStyle.Review, true);
            }

            //if (AtawAppContext.Current.ApplicationXml.IsSupportMobile)
            //{
            //    CustomButtonConfig bt = new CustomButtonConfig();
            //    bt.IsData = false;
            //    bt.Name = "btMobilePage";
            //    bt.Text = "打开手机版";
            //    bt.Client = new ClientConfig() { Function = "GotoMobilePage" };
            //    ModuleConfig.Buttons.Add(bt);
            //}

            //if (AtawAppContext.Current.ApplicationXml.IsSupportReport)
            //{
            //    CustomButtonConfig bt = new CustomButtonConfig();
            //    bt.IsData = false;
            //    bt.Name = "btReportPage";
            //    bt.Text = "打开报表";
            //    bt.Client = new ClientConfig() { Function = "GotoReportPage" };
            //    ModuleConfig.Buttons.Add(bt);
            //}

            //if (AtawAppContext.Current.ApplicationXml.IsSupportReport)
            //{
            //    CustomButtonConfig bt = new CustomButtonConfig();
            //    bt.IsData = false;
            //    bt.Name = "btQing";
            //    bt.Text = "轻办公";
            //    bt.Client = new ClientConfig() { Function = "GotoQing" };
            //    ModuleConfig.Buttons.Add(bt);
            //}
            //}

            // return list;

        }

        private void CreateButton(PageStyle pageStyle, bool isData)
        {
            var bts = ModuleConfig.Buttons.Where(a => a.Name == pageStyle.ToString());
            bool isBt = bts != null && bts.Count() > 0;
            if (!isBt)
            {
                CustomButtonConfig bt = new CustomButtonConfig();
                bt.IsData = isData;
                bt.Name = pageStyle.ToString();
                bt.Text = pageStyle.GetDescription();
                bt.Client = new ClientConfig() { Function = pageStyle.ToString() };
                ModuleConfig.Buttons.Add(bt);
            }
            else
            {
                //AtawDebug.AssertArgument(bts.Count() == 1, pageStyle.ToString(), string.Format(ObjectUtil.SysCulture,
                
                var bt = bts.FirstOrDefault();
                if (!bt.IsData && isData)
                {
                    bt.IsData = true;
                }
                if (bt.Text.IsEmpty())
                {
                    bt.Text = pageStyle.GetDescription();
                }
                if (bt.Client == null)
                {
                    bt.Client = new ClientConfig() { Function = pageStyle.ToString() };
                }
            }

            // return bt;
        }

    }
    public class FormConfigInfo
    {
        public DataFormConfig DataForm { get; set; }

        public FormConfig FormConfig { get; set; }

        public AtawFormConfigView FormView { get; set; }
    }
}
