using System;
using System.Collections.Generic;

namespace TodoApp.Models
{
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class TodoList : BaseModel
	{
		public string Title { get; set; }

		public string Description { get; set; }

		public bool IsActive { get; set; }
	}
}
