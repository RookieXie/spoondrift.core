using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spoondrift.Code.Config;
using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Test
{
    [TestClass]
    public class ConfigTest
    {
        [TestMethod]
        public void ReadFormConfig_Test()
        {
            var filePath = "I:\\GitHub\\spoondrift.core\\test\\Spoondrift.Code.Test\\modules\\RC_Role.xml";
            var config = XmlUtil.ReadFromFile<ModuleConfig>(filePath);
            config.Forms.ForEach(a => { 
            
            });
            Assert.AreEqual("角色", config.Title);
        }
    }
}
