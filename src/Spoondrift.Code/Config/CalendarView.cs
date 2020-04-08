using Spoondrift.Code.PlugIn;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spoondrift.Code.Config
{
    [CodePlug("CalendarView", Description = " 日历视图")]
    public enum CalendarView
    {
        none = 0,
        month = 1,
        agendaWeek = 2,
        agendaDay = 3
    }
}
