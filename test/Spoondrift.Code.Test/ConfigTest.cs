using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spoondrift.Code.Config;
using Spoondrift.Code.Config.Form;
using Spoondrift.Code.Dapper;
using Spoondrift.Code.Data;
using Spoondrift.Code.PlugIn;
using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Spoondrift.Code.Test
{
    [TestClass]
    public class ConfigTest
    {
        [TestMethod]
        public void ReadFormConfig_Test()
        {
            var services = new ServiceCollection();
            services.AddCodePlugService();
            services.AddScoped<IUnitOfDapper>(p =>
            {
                return new DapperContext("server=118.24.146.107;port=3306;uid=root;pwd=fengyan1992;database=Spoondrift;Min Pool Size=0;Pooling=true;connect timeout=120;CharSet=utf8mb4;SslMode=none;");
            });
            services.AddScoped<Xml2DB>();
            var provide = services.BuildServiceProvider();
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modules", "RC_Role.xml");
            var config = XmlUtil.ReadFromFile<ModuleConfig>(filePath);

            config.Forms.Cast<FormConfig>().ToList().ForEach(a =>
                {
                    IListDataTable dt = provide.GetCodePlugService<IListDataTable>(a.DataPlug);
                    var formPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "forms", a.File);
                    var dataForm = XmlUtil.ReadFromFile<DataFormConfig>(formPath);
                    var xml2Db = services.BuildServiceProvider().GetService<Xml2DB>();
                    xml2Db.Migrations(dataForm);//创建表               //dt.Initialize();
                    var postDataSet = new DataSet();
                    dt.Initialize(new ModuleFormInfo(postDataSet, 10, "", "",
                     a.TableName, dataForm.PrimaryKey, "", false, dataForm, a.OrderSql, a));
                    var list = dt.List;
                    var data = new DataSet();
                    dt.AppendTo(data);
                    var dataTable = data.Tables[a.Name];
                    foreach (DataRow item in dataTable.Rows)
                    {
                        // Console.WriteLine(item["FID"]);
                        var fid = item["FID"];
                    }
                    //Console.WriteLine(dataForm.Name);
                });
            Assert.AreEqual("角色", config.Title);
        }
    }
}
