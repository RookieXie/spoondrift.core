using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spoondrift.Code.Config;
using Spoondrift.Code.Config.Form;
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
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modules", "RC_Role.xml"); 
            var config = XmlUtil.ReadFromFile<ModuleConfig>(filePath);

            config.Forms.Cast<FormConfig>().ToList().ForEach(a =>
                {
                    var formPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "forms", a.File);
                    var dataForm = XmlUtil.ReadFromFile<DataFormConfig>(formPath);
                    //Console.WriteLine(dataForm.Name);
                });
            Assert.AreEqual("角色", config.Title);
        }
    }
}
