using Spoondrift.Code.Config;
using Spoondrift.Code.Config.Form;
using Spoondrift.Code.PageView.FormViewCreator;
using Spoondrift.Code.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.PageView
{
    public abstract class BaseOptionCreator : OptionCreator
    {
        protected BaseOptions BaseOptions { get; set; }

        public override BaseOptions Create()
        {
            //BaseOptions.DataObject = "";
            BaseOptions.IsKey = this.Config.IsKey;
            BaseOptions.IsParentColumn = this.Config.IsParentColumn;
            BaseOptions.IsReadOnly = this.Config.IsReadOnly;
            BaseOptions.RegName = this.Config.RegName;
            BaseOptions.DetialFormatFun = this.Config.DetialFormatFun;
            BaseOptions.DisplayName = this.Config.DisplayName;
            BaseOptions.Prompt = this.Config.Prompt;
            BaseOptions.ValPrompt = this.Config.ValPrompt;

            string tableName = this.FormView.TableName;
            string colName = this.Config.Name;
            string insertTableName = tableName + "_INSERT";

            if (ViewPageStyle == PageStyle.Insert) //新增的时候如果存在默认值，将构造一张新表存放默认值，新表名为原表名加上"_INSERT"后缀
            {
                if (!this.Config.DefaultValueStr.IsEmpty())
                {
                    if (!this.PageView.Data.Tables.Contains(insertTableName))
                    {
                        this.PageView.Data.Tables.Add(insertTableName);
                    }
                    var dt = this.PageView.Data.Tables[insertTableName];
                    if (!dt.Columns.Contains(colName))
                        dt.Columns.Add(colName);
                    if (dt.Rows.Count == 0)
                    {
                        var row = dt.NewRow();
                        row[colName] = this.Config.DefaultValueStr.Replace("\n        ", "");
                        dt.Rows.Add(row);
                    }
                    else
                        dt.Rows[0][colName] = this.Config.DefaultValueStr.Replace("\n        ", "");
                    BaseOptions.DataValue = new JsDataValue(insertTableName, colName);
                    BaseOptions.DataValue.IsChange = true;
                }
            }
            else
            {
                BaseOptions.DataValue = new JsDataValue(tableName, colName);
            }
            if (ViewPageStyle != PageStyle.List && ViewPageStyle != PageStyle.Detail)
            {
                PostSetting ps = new PostSetting() { TableName = tableName, ColumnName = colName };
                BaseOptions.PostSetting = ps;
            }
            if (ViewPageStyle == PageStyle.Insert || ViewPageStyle == PageStyle.Update)
            {
                if (Config.ControlLegal != null)
                {
                    var kind = Config.ControlLegal.Kind;
                    string legalFun = Config.ControlLegal.CustomLegalFun;
                    string reg = Config.ControlLegal.Reg;
                    string errMsg = Config.ControlLegal.ErrMsg;
                    if (kind == LegalKind.custom && legalFun.IsEmpty())
                    {
                        //Debug.AssertNotNullOrEmpty(legalFun, "自定义验证控件时，需要在CustomLegalFun节点指定一个自定义函数", this);
                    }
                    else if (kind == LegalKind.customReg && reg.IsEmpty())
                    {
                        //Debug.AssertNotNullOrEmpty(reg, "正则表达式验证控件时，需要在Reg节点指定一个正则表达式", this);
                    }
                    else if (!legalFun.IsEmpty())
                    {
                        kind = LegalKind.custom;
                    }
                    else if (!reg.IsEmpty())
                    {
                        kind = LegalKind.customReg;
                    }
                    //Debug.Assert(legalFun.IsEmpty() || reg.IsEmpty(), "只需指定一种控件验证方式，现在既配置了自定义验证，又配置了正则表达式验证", this);
                    ControlLegal cl = new ControlLegal()
                    {
                        Kind = kind,
                        CustomLegalFun = legalFun,
                        Reg = reg,
                        ErrMsg = errMsg,
                        LegalExpression = this.Config.ControlLegal.LegalExpression

                    };
                    BaseOptions.Legal = cl;
                }
            }
            return BaseOptions;
        }
    }
}
