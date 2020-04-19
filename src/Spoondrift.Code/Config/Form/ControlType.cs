using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Spoondrift.Code.Config.Form
{
    [CodePlug("ControlType", Description = "控件类型")]
    public enum ControlType
    {
        None = 0,
        [Description("单行文本控件")]
        Text = 1,
        [Description("多行文本控件")]
        TextArea = 2,
        [Description("多行文本详情控件")]
        DetailArea = 3,
        [Description("详情控件")]
        Detail = 4,
        [Description("密码控件")]
        Password = 5,
        [Description("日期控件")]
        Date = 6,
        [Description("时间控件")]
        DateTime = 7,
        [Description("日期详情控件")]
        DetailDate = 8,
        [Description("隐藏控件")]
        Hidden = 9,
        [Description("富文本编辑器控件")]
        Editor = 10,
        [Description("是否值控件")]
        SingleCheckBox = 11,
        [Description("多选框控件")]
        CheckBox = 12,
        [Description("下拉框控件")]
        Combo = 13,
        [Description("单选控件")]
        Radio = 14,
        [Description("单文件上传控件")]
        SingleFileUpload = 15,
        [Description("多文件上传控件")]
        MultiFileUpload = 16,
        [Description("单图片上传控件")]
        SingleImageUpload = 17,
        [Description("多图片上传控件")]
        MultiImageUpload = 18,
        [Description("单选选择器控件")]
        Selector = 19,
        [Description("多选选择器控件")]
        MultiSelector = 20,
        [Description("树形单选选择器控件")]
        TreeSingleSelector = 21,
        [Description("树形多选选择器控件")]
        TreeMultiSelector = 22,
        [Description("省市县控件")]
        PCAS = 23,
        [Description("链接控件")]
        Link = 24,
        [Description("下载链接控件")]
        DownLink = 25,
        [Description("富文本显示控件")]
        EditorDetail = 26,
        [Description("图片显示控件")]
        ImageDetail = 27,
        [Description("多选导航控件")]
        CheckBoxNavi = 28,
        [Description("单选导航控件")]
        RadioNavi = 29,
        [Description("树形单选导航控件")]
        TreeSingleNavi = 30,
        [Description("树形多选导航控件")]
        TreeMultiNavi = 31,
        [Description("单选表单控件")]
        FormSingleSelector = 32,
        [Description("多选表单控件")]
        FormMultiSelector = 33,
        [Description("星星评分控件")]
        StarScore = 34,
        [Description("多图片显示控件")]
        AllImageShow = 35,
        [Description("时分秒控件")]
        Time = 36,
        [Description("文件浏览控件")]
        FileDetail = 37,
        [Description("内嵌页面控件")]
        InnerPage = 38,
        [Description("布尔导航控件")]
        SingleRadioNavi = 39,
        [Description("文档选择控件")]
        DocumentSelector = 40,
        [Description("文档浏览控件")]
        DocumentDetail = 41,
        [Description("用户选择器控件")]
        UserSelector = 42,
        [Description("页面选择器控件")]
        PageSelector = 43,
        [Description("自定义控件")]
        Custom = 44,
        [Description("内嵌表单控件")]
        InnerForm = 50,
        [Description("列表框选择控件")]
        ListBox = 51,
        [Description("导航筛选器控件")]
        NaviFilter = 52,
        [Description("双字段控件")]
        TwoColumns = 53,
        [Description("金额控件")]
        Amount = 54,
        [Description("金额显示控件")]
        AmountDetail = 55,
        [Description("双字段显示控件")]
        TwoColumnsDetail = 56,
        [Description("流水码控件")]
        MaskCode = 60,
        [Description("记忆控件")]
        /// <summary>
        /// 记忆控件
        /// </summary>
        Momery = 80,
        [Description("MarkDown控件")]
        /// <summary>
        /// MarkDown编辑器控件
        /// </summary>
        MarkDown = 90,
        [Description("json表单控件")]
        JsonForm = 95,
        //Amount = 70,
        //AmountDetail = 71
        [Description("模板控件")]
        /// <summary>
        /// 模板控件
        /// </summary>
        XFormCol = 100,
        [Description("模板只读控件")]
        /// <summary>
        /// 模板控件
        /// </summary>
        XFormColDetail = 101

    }
}
