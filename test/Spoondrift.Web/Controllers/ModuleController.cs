using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Spoondrift.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        public void Module(string ds, string xml, string pageStyle, string keyValue)
        {
            //try
            //{
            //    xml = Xml(xml);
            //    AtawDebug.AssertNotNullOrEmpty(xml, "亲 ,modulexml 注册名不可以为空的", this);
            //    ModuleConfig mc = xml.SingletonByPage<ModuleConfig>();
            //    if (mc.Mode == ModuleMode.None)
            //    {
            //        throw new AtawException("ModuleXml的Mode节点不能为空", this);
            //    }

            //    var tool = GetPageViewTool(mc);
            //    tool.BeginModuleInterceptor(ref ds, ref xml, ref pageStyle, ref keyValue, ref mc);

            //    //if (!AtawAppContext.Current.IsAuthenticated)
            //    //{
            //    //    JsResponseResult<object> ree = new JsResponseResult<object>()
            //    //    {
            //    //        ActionType = JsActionType.Alert,
            //    //        Content = "请登录，匿名暂不开放...."
            //    //    };
            //    //    return FastJson(ree);
            //    //}
            //    mc = xml.SingletonByPage<ModuleConfig>();
            //    if (!AtawBasePageViewCreator.IsSupportPage(mc.SupportPage, pageStyle.Value<PageStyle>()))
            //    {
            //        JsResponseResult<object> ree = new JsResponseResult<object>()
            //        {
            //            ActionType = JsActionType.Alert,
            //            Content = "无权访问该页面"
            //        };
            //        return FastJson(ree);
            //    }
            //    bool isXml2Db = AtawAppContext.Current.ApplicationXml.IsMigration && !mc.IsNoDb;
            //    if (isXml2Db && mc.DataBase == null)
            //    {
            //        mc.Forms.Cast<FormConfig>().ToList().ForEach(a =>
            //        {
            //            var dataForm = a.File.XmlConfig<DataFormConfig>();
            //            AtawAppContext.Current.Xml2Db.Migrations(dataForm);
            //        }
            //            );
            //    }

            //    if (isXml2Db)
            //    {
            //        AtawTrace.WriteFile(LogType.DatabaseStructure, AtawAppContext.Current.Xml2Db.GetLogMessage());
            //        var dbContext = AtawAppContext.Current.UnitOfData;
            //        if (dbContext != null)
            //        {
            //            AtawAppContext.Current.UnitOfData.Submit();
            //            AtawAppContext.Current.UnitOfData = null;
            //        }
            //    }

            //    AtawBasePageViewCreator pageCreator = (pageStyle + "PageView").SingletonByPage<AtawBasePageViewCreator>();
            //    pageCreator.Initialize(mc, JsonConvert.DeserializeObject<DataSet>(ds ?? ""), keyValue, "", false);
            //    var apcv = pageCreator.Create();
            //    apcv.RegName = xml;


            //    return tool.EndModuleInterceptor(apcv);
            //}
            //catch (Exception ex)
            //{
            //    RecoredException(ex);
            //    AtawPageConfigView apcv = new AtawPageConfigView();
            //    apcv.Header = new PageHeader();
            //    apcv.Header.IsValid = false;
            //    apcv.Header.Message = "<h2>系统出现异常，请跟管理员联系！</h2><p>异常信息是:{0}</p>".AkFormat(ex.Message);
            //    return ReturnJson(apcv);
            //}
        }
    }
}