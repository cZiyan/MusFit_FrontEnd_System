using System;
using System.Collections.Generic;

#nullable disable

namespace Lab1212_todo_mvc.Models
{
    public partial class RoomState
    {
        public int RsId { get; set; }
        public int RoomId { get; set; }
        public int CId { get; set; }
        public int ClassTimeId { get; set; }

        public virtual Room Room { get; set; }
    }
}
