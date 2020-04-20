
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Spoondrift.Code.PageView
{
    public class PageConfigView
    {

        public PageConfigView()
        {
            DataButtons = new Dictionary<string, CustomButtonConfigView>();
            PageButtons = new Dictionary<string, CustomButtonConfigView>();
        }
        /// <summary>
        /// 输出PageSourceData的享元字符串
        /// </summary>
        public string PageSourceData { get; set; }

        public object PageCustomerSourceData { get; set; }

        public string RegName { get; set; }

        public string Title { get; set; }
        public string PageLayout { get; set; }

        public string ReturnUrl { get; set; }
        public string Route { get; set; }

        public bool IsPart { get; set; }
        public PageHeader Header { get; set; }
        /// <summary>
        /// 动态加载的js
        /// </summary>
        public List<ScriptConfig> Scripts { get; set; }


        public DataSet Data { get; set; }
        public IDictionary<string, object> ExtData { get; set; }
        //主从表的主表主键值
        public string KeyValue { get; set; }

        public Dictionary<string, FormConfigView> Forms { get; set; }
        //public Dictionary<string, SeaFormConfigView> SeaForms { get; set; }
        //public Dictionary<string, MvcFormConfigView> MvcForms { get; set; }
       // public Dictionary<string, ScriptFormConfigView> ScriptForms { get; set; }
        public List<PanelList> Layout { get; set; }

        public IDictionary<string, CustomButtonConfigView> DataButtons { get; set; }
        public IDictionary<string, CustomButtonConfigView> PageButtons { get; set; }

        public string BeforeHook { get; set; }
        public string AfterHook { get; set; }

        public string TsHook { get; set; }
        /// <summary>
        /// 自定义渲染页面
        /// </summary>
        public string TsSkinName { get; set; }


    }
}
