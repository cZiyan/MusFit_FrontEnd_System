using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit_FrontDesk.Models
{
    public partial class TodoItem
    {
        public long TodoItemId { get; set; }
        public string Name { get; set; }
        public bool? IsComplete { get; set; }
    }
}
