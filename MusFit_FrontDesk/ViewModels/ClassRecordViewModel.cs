using System;

namespace MusFit_FrontDesk.ViewModels
{
    public class ClassRecordViewModel
    {
        public string CrAttendance { get; set; }
        public string CrContent { get; set; }
        public string CtDate { get; set; }
        public DateTime CtDateFrom { get; set; }
        public DateTime CtDateTo { get; set; }
        public short CtLession { get; set; }
        public string CNumber { get; set; }
        public string CName { get; set; }
        public string Time { get; set; }
        public string LcName { get; set; }
        public string Weekday { get; set; }
    }
}
