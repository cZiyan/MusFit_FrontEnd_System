using System;
using System.Collections.Generic;

#nullable disable

namespace Lab1212_todo_mvc.Models
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
