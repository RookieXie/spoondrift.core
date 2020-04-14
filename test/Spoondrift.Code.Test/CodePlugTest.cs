using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spoondrift.Code.Data;
using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Test
{
    [TestClass]
    public class CodePlugTest
    {
        [TestMethod]
        public void AddCodePlugTest()
        {
            var services = new ServiceCollection();
            services.AddCodePlugService();
            var codeTable=services.BuildServiceProvider().GetCodePlugService<CodeTable<CodeDataModel>>("FormType");
            Console.WriteLine(codeTable.RegName);
        }
    }
}
