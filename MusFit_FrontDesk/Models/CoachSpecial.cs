using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit_FrontDesk.Models
{
    public partial class CoachSpecial
    {
        public int CsId { get; set; }
        public int EId { get; set; }
        public int LcId { get; set; }

        public virtual Employee EIdNavigation { get; set; }
        public virtual LessionCategory Lc { get; set; }
    }
}
