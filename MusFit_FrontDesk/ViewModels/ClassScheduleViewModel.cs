using System;

namespace MusFit_FrontDesk.ViewModels
{
    public class ClassScheduleViewModel
    {
        public string CNumber { get; set; }
        public string CName { get; set; }
        public string EName { get; set; }
        public DateTime CtDate { get; set; }
        public short CtLession { get; set; }
        public string Weekday { get; set; }
        public TimeSpan TStartTime { get; set; }
        public TimeSpan TEndTime { get; set; }
        public string LcName { get; set; }
        public string LcThemeColor { get; set; }
        public int? RoomName { get; set; }
    }
}
