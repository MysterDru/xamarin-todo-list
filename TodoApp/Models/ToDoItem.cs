using System;
namespace TodoApp.Models
{
	public class ToDoItem : BaseModel
    {
		public string Title { get; set; }

		public string Notes { get; set; }

		public DateTime DueDate { get; set; }

		public bool IsCompleted { get; set; }

		public Guid ListId { get; set; }
    }
}
