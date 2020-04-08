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
            var filePath = "";
            var config = XmlUtil.ReadFromFile<ModuleConfig>(filePath);
            Assert.Equals(config.FileName,"");
        }
    }
}
