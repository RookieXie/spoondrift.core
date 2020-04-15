using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Config.Form
{
    [CodePlug("LegalKind", Description = "验证类型")]
    public enum LegalKind
    {
        none = 0,

        /// <summary>
        /// 自定义
        /// </summary>
        custom = 1,

        /// <summary>
        /// 非空
        /// </summary>
        notNull = 2,


        /// <summary>
        /// 自定义正则表达式
        /// </summary>
        customReg = 3,

        /// <summary>
        /// 固定电话
        /// </summary>
        tel = 4,

        /// <summary>
        /// 手机号码
        /// </summary>
        mobile = 5,

        /// <summary>
        /// 邮箱地址
        /// </summary>
        email = 6,

        /// <summary>
        /// 密码
        /// </summary>
        pwd = 7,

        /// <summary>
        /// 用户名
        /// </summary>
        username = 8,



        /// <summary>
        /// 用户名验证
        /// </summary>
        UserNameLegal = 9,

        /// <summary>
        /// 密码验证
        /// </summary>
        PassWordLegal = 10,

        /// <summary>
        /// 联系方式验证（移动电话、座机）
        /// </summary>
        MobilePhoneLegal = 11,

        /// <summary>
        /// 电子邮箱验证
        /// </summary>
        EmailLegal = 12,

        /// <summary>
        /// 身份证号码验证
        /// </summary>
        IDCardLegal = 13,

        /// <summary>
        /// 邮政编码验证
        /// </summary>
        PostCodeLegal = 14,

        /// <summary>
        /// 详细内容、介绍字数验证
        /// </summary>
        ContextLegal = 15,

        /// <summary>
        /// 标题字数验证
        /// </summary>
        TitleLegal = 16,

        /// <summary>
        /// 商品价格（大于0）验证
        /// </summary>
        PriceLegal = 17,

        /// <summary>
        /// 车辆（承重、初始公里数、油耗）验证
        /// </summary>
        VehicleLimitLegal = 18,

        /// <summary>
        /// 座位验证
        /// </summary>
        SeatLegal = 19,

        #region 可以为空值，若不为空启用验证
        EmailLegalNull = 20,

        IDCardLegalNull = 21,

        PostCodeLegalNull = 22,

        MobilePhoneLegalNull = 23,

        VehicleLimitLegalNull = 24,

        PriceLegalNull = 25,

        SeatLegalNull = 26,

        ContextLegalNull = 27,

        TitleLegalNull = 28,


        #endregion

        SelectionNotNull = 29,
        RadioNotNull = 30,

        nonnegativeIntegerNull = 31,
        nonnegativeInteger = 32,
        /// <summary>
        /// 自定义验证，非空
        /// </summary>
        customNull = 33,

        MorethanZeroLegal = 34,
        MorethanZeroLegalNull = 35,
        UploadCountLegal = 36,
        /// <summary>
        /// 验证PCAS控件必填项个数
        /// </summary>
        PCASRequiredCountLegal = 37,
        /// <summary>
        /// 表达式验证
        /// </summary>
        ExpressionLegal = 100
    }
}
