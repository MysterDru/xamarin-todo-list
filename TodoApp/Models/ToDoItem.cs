using System;
namespace TodoApp.Models
{
    public class ToDoItem
    {
		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Notes { get; set; }

		public DateTime DueDate { get; set; }

		public bool IsCompleted { get; set; }

		public Guid ListId { get; set; }
    }
}
