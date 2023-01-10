using System;
using System.Collections.Generic;

#nullable disable

namespace Lab1212_todo_mvc.Models
{
    public partial class TodoItem
    {
        public long TodoItemId { get; set; }
        public string Name { get; set; }
        public bool? IsComplete { get; set; }
    }
}
