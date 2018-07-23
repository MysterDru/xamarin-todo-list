using System;
using Newtonsoft.Json;

namespace TodoApp.Models
{
	[PropertyChanged.AddINotifyPropertyChangedInterface]
	public class TodoItem : BaseModel
	{
		public string Title { get; set; }

		public bool IsCompleted { get; set; }

		public Guid ListId { get; set; }

		public DateTime? CompletedOn { get; set; }

		[JsonIgnore]
		public string Description => CompletedOn.HasValue ? $"Completed at {CompletedOn.Value.ToString("g")}" : null;
	}
}
