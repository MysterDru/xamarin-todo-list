using System;
using System.Collections.Generic;

namespace TodoApp.Models
{
    public class TodoList
    {
		public Guid Id { get; set; }

		public string Title { get; set; }

		public bool IsActive { get; set; }
    }
}
