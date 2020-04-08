using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spoondrift.Code.Config;
using Spoondrift.Code.PlugIn;
using Spoondrift.Code.Util;
using System;
using System.IO;
using System.Reflection;

namespace Spoondrift.Code.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            dynamic type = this.GetType();
            string currentDirectory = Path.GetDirectoryName(type.Assembly.Location);
            Console.WriteLine(currentDirectory);
            foreach (string dllPath in Directory.GetFiles(currentDirectory, "*.dll"))
            {
                var an = AssemblyName.GetAssemblyName(dllPath);
                var assembly = AppDomain.CurrentDomain.Load(an);
                var types = assembly.GetTypes();
                foreach (var t in types)
                {
                    var code = Attribute.GetCustomAttribute(t, typeof(CodePlugAttribute)) as CodePlugAttribute;
                    if (code != null)
                    {
                        if (t.IsEnum)
                        {
                            EnumCodeTable(t);
                        }
                        else
                        {
                            try
                            {




                            }
                            catch (Exception ex)
                            {

                            }

                        }

                    }
                }
            }
        }


        public void EnumCodeTable(Type type)
        {

            //string key = "SELECTOR_ENUM";
            // HasCache = true;

            var arrs = Enum.GetValues(type);
            int count = arrs.Length;
            foreach (var en in arrs)
            {
                if (en.Equals(arrs.GetValue(count - 1)))
                {
                    Console.WriteLine($"ODE_VALUE={en.Value<int>().ToString()},CODE_TEXT={en.GetDescription()},CODE_NAME={ en.ToString()}");


                }
                else
                {
                    if (en.Equals(arrs.GetValue(0)))
                    {
                        Console.WriteLine($"ODE_VALUE={en.Value<int>().ToString()},CODE_TEXT={en.GetDescription()},CODE_NAME={ en.ToString()}");
                    }
                    else
                    {
                        Console.WriteLine($"ODE_VALUE={en.Value<int>().ToString()},CODE_TEXT={en.GetDescription()},CODE_NAME={ en.ToString()}");
                    }
                }

            }
        }
    }
}
