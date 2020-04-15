using Microsoft.Extensions.DependencyInjection;
using Spoondrift.Code.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Spoondrift.Code.PlugIn
{
    public static class CodePlugServiceCollectionExtensions
    {
        public static void AddCodePlugService(this IServiceCollection services)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("根目录：" + currentDirectory);
            var assemblies = LoadAssemblys();
            foreach (Assembly assembly in assemblies)
            {
                Console.WriteLine("开始检查程序集：" + assembly.FullName);
                var types = assembly.GetTypes();
                foreach (var t in types)
                {
                    Console.WriteLine("开始检查类型：" + t.FullName);
                    services.SetAssamblyClassType(t);
                }
            }
        }
        private static List<Assembly> LoadAssemblys()
        {
            List<Assembly> assemblies = new List<Assembly>();
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine("根目录：" + currentDirectory);
            foreach (string dllPath in Directory.GetFiles(currentDirectory, "*.dll"))//还过滤掉不需要的dll
            {
                var an = AssemblyName.GetAssemblyName(dllPath);
                var assembly = AppDomain.CurrentDomain.Load(an);
                if (assembly.FullName.Contains("Microsoft") || assembly.FullName.Contains("Newtonsoft") || assembly.FullName.Contains("System"))
                {

                }
                else
                {
                    assemblies.Add(assembly);
                }
            }
            return assemblies;
        }
        private static void SetAssamblyClassType(this IServiceCollection services, Type type)
        {
            if (Attribute.GetCustomAttribute(type, typeof(CodePlugAttribute)) is CodePlugAttribute code)
            {
                var plug = new PlugInModel
                {
                    Name = code.CodePlugName,
                    Key = code.CodePlugName,
                    Author = code.Author,
                    CreateDate = code.CreateDate,
                    Description = code.Description,

                };
                if (code.Tags != null && code.Tags.Count() > 0)
                    plug.Tags = code.Tags.ToList();

                if (type.IsEnum)
                {
                    EnumCodeTable ect = new EnumCodeTable(type);
                    ect.CodePlugName = code.CodePlugName;//标识唯一性
                    plug.InstanceType = type;
                    plug.BaseType = typeof(CodeTable<CodeDataModel>);
                    services.AddTransient<CodeTable<CodeDataModel>>(p =>
                    {
                        return ect;
                    });
                }
                else
                {
                    try
                    {
                        //IRegName iReg = (IRegName)type;
                        //iReg.CodePlugName = code.CodePlugName;//标识唯一性
                       
                        plug.InstanceType =type;
                        plug.BaseType = code.BaseClass;
                        services.AddTransient(code.BaseClass, (p)=> {
                            object obj = Activator.CreateInstance(type, p);
                            type.GetProperty("CodePlugName").SetValue(obj, code.CodePlugName);
                            return obj;
                        });

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("注册插件失败:{0}:{1} ", ex.Message, type.Name);
                    }
                }
                Log(plug);
            }
        }
        private static void Log(PlugInModel plug)
        {
            string log = $"注册名：{plug.Name}\r\n基类:{plug.InstanceType.ToString()}\r\n实例类:{ plug.BaseType.ToString()}\r\n路径:{3}\r\n作者:{ plug.Author}\r\n描述:{ plug.Description}";
            Console.WriteLine(log);
        }
        public static T GetCodePlugService<T>(this IServiceProvider provider, string regName) where T : IRegName
        {
            return provider.GetServices<T>().Where(a => a.CodePlugName == regName).FirstOrDefault();
        }
    }
}
