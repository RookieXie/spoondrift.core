using Spoondrift.Code.Util;
using Spoondrift.Code.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Spoondrift.Code.Config.Form
{
    public class DataFormConfig : FileXmlConfigBase, IReadXmlCallback
    {
        public DataFormConfig()
        {
            ColumnLegals = new HashSet<string>();
        }

        public virtual string Name { get; set; }

        public virtual string TableName { get; set; }

        public virtual bool IsExpand { get; set; }

        public virtual string Title { get; set; }

        [XmlIgnore]
        public virtual string PrimaryKey { get; set; }
        [XmlIgnore]
        public virtual HashSet<string> ColumnLegals { get; set; }

        [XmlArrayItem("Column")]
        public virtual List<ColumnConfig> Columns { get; set; }

       // [XmlArrayItem("ColumnGroup")]
        //public List<ColumnGroupConfig> ColumnGroups { get; set; }

        void IReadXmlCallback.OnReadXml()
        {
            Columns = Columns.OrderBy(a => a.Order).ToList();
            Columns.ForEach(a =>
            {
                var isFControlUnitShow = false;
                try
                {
                    isFControlUnitShow = false;
                }
                catch
                { }
                if (isFControlUnitShow && a.Name == "FControlUnitID" && a.ControlType == ControlType.Hidden)
                {
                    a.ControlType = ControlType.Detail;
                    a.InternalShowPage = "List";
                }


                //if (a.Navigation != null)
                //{
                //    a.Search = null;//有了导航 何必需要搜索呢
                //}

                //if (a.ControlType == ControlType.Text && a.Search != null && a.Search.IsLike == null)
                //{
                //    a.Search.IsLike = true;
                //}


                if (!a.DisplayName.IsEmpty() && a.DisplayName.Length > 14 && a.DisplayName != "empty-title")
                {
                    a.DisplayName = a.DisplayName.Substring(0, 14);

                }
                else
                {
                    a.DisplayName = a.DisplayName.RemoveEnd("_f");
                }
                //if (a.DefaultValue == null)
                //    a.DefaultValueStr = string.Empty;
                //else if (a.DefaultValue.Value.IsEmpty())
                //    a.DefaultValueStr = string.Empty;
                //else if (!a.DefaultValue.NeedParse)
                //    a.DefaultValueStr = a.DefaultValue.Value;
                //else
                //    a.DefaultValueStr = a.DefaultValue.ExeValue();
                if (a.InternalShowPage.IsEmpty())
                    a.ShowPage = PageStyle.All;
                else
                {
                    var arr = a.InternalShowPage.Split('|');
                    arr.ToList().ForEach(str =>
                    {
                        a.ShowPage = a.ShowPage | str.ToEnum<PageStyle>();
                    });
                }
            });
            //字段分组
            //if (ColumnGroups != null && ColumnGroups.Count > 0)
            //{
            //    var allGroupColumns = new List<string>();
            //    ColumnGroups = ColumnGroups.OrderBy(x => x.Order).ToList();
            //    ColumnGroups.ForEach(group =>
            //    {
            //        group.Columns = new List<ColumnConfig>();
            //        var nameList = group.ColumnNames.Split(',').ToList();
            //        nameList.ForEach(a =>
            //        {
            //            var column = Columns.FirstOrDefault(b => b.Name == a);
            //            if (column != null)
            //            {
            //                group.Columns.Add(column);
            //            }
            //        });
            //        allGroupColumns.AddRange(nameList);
            //    });
            //    //构造默认分组
            //    var ungroupColumns = new List<string>();
            //    var defaultGroup = new ColumnGroupConfig();
            //    defaultGroup.Columns = new List<ColumnConfig>();
            //    defaultGroup.Name = "Default";
            //    Columns.ForEach(x =>
            //    {
            //        if (!allGroupColumns.Contains(x.Name))
            //        {
            //            defaultGroup.Columns.Add(x);
            //        }
            //    });
            //    if (defaultGroup.Columns.Count > 0)
            //    {
            //        ColumnGroups.Add(defaultGroup);
            //    }

            //}
        }

        //附加基本的列（若配置没有）
        public void AddBaseColumns(string baseFormFileName)
        {
            try
            {
                string fpath = Path.Combine("", "",
                    "form", baseFormFileName);
                var baseForm = XmlUtil.ReadFromFile<DataFormConfig>(fpath);
                foreach (var column in baseForm.Columns)
                {
                    if (this.Columns.FirstOrDefault(n => n.Name.ToUpper() == column.Name.ToUpper()) == null)
                    {
                        this.Columns.Add(column);
                    }
                }
            }
            catch { }
        }
    }
}
