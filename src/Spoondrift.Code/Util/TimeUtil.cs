using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Util
{
    public class TimeUtil
    {
        public static DateTime GetMondayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1)
                i = 6;
            TimeSpan timeSpanSub = new TimeSpan(i, 0, 0, 0);
            TimeSpan timeSpanAdd = new TimeSpan(0, 9, 0, 0);
            return someDate.Date.Subtract(timeSpanSub).Add(timeSpanAdd);
        }
        public static DateTime GetFridayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Friday;
            TimeSpan timeSpanSub, timeSpanAdd;
            if (i == -5) //星期天
            {
                i = 2;
            }
            if (i > 0)
            {
                timeSpanSub = new TimeSpan(i, 0, 0, 0);
                timeSpanAdd = new TimeSpan(0, 17, 30, 0);
                return someDate.Date.Subtract(timeSpanSub).Add(timeSpanAdd); ;
            }
            else
            {
                i = -i;
                timeSpanAdd = new TimeSpan(i, 17, 30, 0);
                return someDate.Date.Add(timeSpanAdd);
            }
        }
        public static string GetTimesSpan()
        {
            return ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
        }
    }
}
