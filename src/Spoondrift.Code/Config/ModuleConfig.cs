using Spoondrift.Code.Config.Module;
using Spoondrift.Code.Util;
using Spoondrift.Code.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config
{
    public class ModuleConfig : FileXmlConfigBase, IReadXmlCallback
    {

        public ModuleConfig()
        {
        }

        public string Interceptor { get; set; }

        public bool IsNoDb { get; set; }

        public ModuleMode Mode { get; set; }

        public string Title { get; set; }

        public string Layout { get; set; }

        public string SupportPage { get; set; }

       // [XmlArrayItem("Script")]
        //public List<ScriptConfig> Scripts { get; set; }

       // [XmlArrayItem(Type = typeof(MvcFormConfig), ElementName = "MvcForm")]
        [XmlArrayItem(Type = typeof(FormConfig), ElementName = "Form")]
        //[XmlArrayItem(Type = typeof(SeaFormConfig), ElementName = "SeaForm")]
       // [XmlArrayItem(Type = typeof(ScriptFormConfig), ElementName = "ScriptForm")]
        public List<BaseFormConfig> Forms { get; set; }

        [XmlArrayItem("Relation")]
        public List<RelationConfig> Relations { get; set; }

        [XmlArrayItem("Button")]
        public List<CustomButtonConfig> Buttons { get; set; }

        public RightConfig Right { get; set; }

        public string Route { get; set; }//默认为Default

        public string ReturnUrl { get; set; }

        public string UpdateReturnUrl { get; set; }
        public string InsertReturnUrl { get; set; }

        public bool HasReview { get; set; }

        public bool IsPart { get; set; }
        //  public string Product { get; set; }

        public string BeforeHook { get; set; }
        public string AfterHook { get; set; }

        public string TsHook { get; set; }
        /// <summary>
        /// 数据库连接标识
        /// </summary>
       // public DataBaseConfig DataBase { get; set; }

        //[XmlArrayItem("ColumnRight")]
        //public List<ColumnRightConfig> ColumnRights { get; set; }

        //[XmlArrayItem("RightUnit")]
        //public List<RightUnitConfig> RightUnits { get; set; }

        //[XmlArrayItem("PageStyleRight")]
        //public List<PageStyleRightConfig> PageStyleRights { get; set; }


        //public bool HasPager { get; set; }

        //public int PageSize { get; set; }

        void IReadXmlCallback.OnReadXml()
        {
            if (Forms != null && Forms.Count > 0)
            {
                Forms.ForEach(a =>
                {
                    //Debug.AssertArgumentNullOrEmpty(a.Name, " 表单名称不能为空  ", this);
                    if (a is FormConfig)
                    {
                        var b = (FormConfig)a;
                        if ((b.DataPlug.IsEmpty()))
                        {
                            b.DataPlug = "EmptyDataTableSource";
                        }
                        //if (b.VerticalTab.ToString().IsEmpty())
                        //{
                        //    b.VerticalTab = false;
                        //}
                        if (b.Group.InternalShowPage.IsEmpty())
                            b.Group.ShowPage = PageStyle.All;
                        else
                        {
                            var arr = b.Group.InternalShowPage.Split('|');
                            arr.ToList().ForEach(str =>
                            {
                                b.Group.ShowPage = b.Group.ShowPage | str.ToEnum<PageStyle>();
                            });
                        }

                    }
                    if (a.Title != null && System.Text.Encoding.Default.GetBytes(a.Title.ToCharArray()).Length > 50)
                    {
                        throw new Exception("Title长度设置不要超过25个中文或者50个英文");
                    }
                });
            }
            Forms = Forms.OrderBy(a => a.Order).ToList();


            //Scripts.ForEach(a =>
            //{
            //    if (a.InternalStyle.IsEmpty())
            //        a.Style = PageStyle.All;
            //    else
            //    {
            //        var arr = a.InternalStyle.Split('|');

            //        arr.ToList().ForEach(str =>
            //        {
            //            a.Style = a.Style | str.ToEnum<PageStyle>();
            //        });
            //    }
            //});
            if (Right != null)
                (Right as IReadXmlCallback).OnReadXml();


        }
    }
}
