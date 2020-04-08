using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Config.Form
{
    [CodePlug("ControlType", Description = "控件类型")]
    public enum ControlType
    {
        None = 0,
        Text = 1,
        TextArea = 2,
        DetailArea = 3,
        Detail = 4,
        Password = 5,
        Date = 6,
        DateTime = 7,
        DetailDate = 8,
        Hidden = 9,
        Editor = 10,
        SingleCheckBox = 11,
        CheckBox = 12,
        Combo = 13,
        Radio = 14,
        SingleFileUpload = 15,
        MultiFileUpload = 16,
        SingleImageUpload = 17,
        MultiImageUpload = 18,
        Selector = 19,
        MultiSelector = 20,
        TreeSingleSelector = 21,
        TreeMultiSelector = 22,
        PCAS = 23,
        Link = 24,
        DownLink = 25,
        EditorDetail = 26,
        ImageDetail = 27,
        CheckBoxNavi = 28,
        RadioNavi = 29,
        TreeSingleNavi = 30,
        TreeMultiNavi = 31,
        FormSingleSelector = 32,
        FormMultiSelector = 33,
        StarScore = 34,
        AllImageShow = 35,
        Time = 36,
        FileDetail = 37,
        InnerPage = 38,
        SingleRadioNavi = 39,
        DocumentSelector = 40,
        DocumentDetail = 41,
        UserSelector = 42,
        PageSelector = 43,
        Custom = 44,
        InnerForm = 50,
        ListBox = 51,
        NaviFilter = 52,
        TwoColumns = 53,
        Amount = 54,
        AmountDetail = 55,
        TwoColumnsDetail = 56,
        MaskCode = 60,
        /// <summary>
        /// 记忆控件
        /// </summary>
        Momery = 80,
        /// <summary>
        /// MarkDown编辑器控件
        /// </summary>
        MarkDown = 90
        //Amount = 70,
        //AmountDetail = 71


    }
}
