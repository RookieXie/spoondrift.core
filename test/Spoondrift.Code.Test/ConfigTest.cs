using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spoondrift.Code.Config;
using Spoondrift.Code.Config.Form;
using Spoondrift.Code.Data;
using Spoondrift.Code.PlugIn;
using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
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
            var provide = services.BuildServiceProvider();
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modules", "RC_Role.xml"); 
            var config = XmlUtil.ReadFromFile<ModuleConfig>(filePath);

            config.Forms.Cast<FormConfig>().ToList().ForEach(a =>
                {
                    IListDataTable dt = provide.GetCodePlugService<IListDataTable>(a.DataPlug);
                    var formPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "forms", a.File);
                    var dataForm = XmlUtil.ReadFromFile<DataFormConfig>(formPath);
                    //Console.WriteLine(dataForm.Name);
                });
            Assert.AreEqual("角色", config.Title);
        }
    }
}
