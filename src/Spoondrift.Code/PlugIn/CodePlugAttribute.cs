using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PlugIn
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    public class CodePlugAttribute : Attribute
    {
        public Type BaseClass
        {
            get;
            set;
        }
        /// <value>只读属性<c>RegName</c>：注册名
        /// </value>
        /// <summary>注册名</summary>
        public string RegName { get; private set; }

        /// <value>属性<c>Description</c>：功能描述
        /// </value>
        /// <summary>功能描述</summary>
        public string Description { get; set; }

        /// <value>属性<c>Author</c>：作者
        /// </value>
        /// <summary>作者</summary>
        public string Author { get; set; }

        /// <value>属性<c>CreateDate</c>：创建时间
        /// </value>
        /// <summary>创建时间</summary>
        public string CreateDate { get; set; }

        public PlugInTag[] Tags { get; set; }
        public CodePlugAttribute(string regName)
        {
        }
    }
}
