using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit_FrontDesk.Models
{
    public partial class KnowledgeColumn
    {
        public int KColumnId { get; set; }
        public string KTitle { get; set; }
        public string KContent { get; set; }
        public string KAuthor { get; set; }
        public DateTime KDate { get; set; }
        public byte[] KPhoto1 { get; set; }
        public byte[] KPhoto2 { get; set; }
    }
}
