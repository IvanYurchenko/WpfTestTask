using WpfTestTask.Enums;

namespace WpfTestTask.Models
{
	public class RowModel
	{
		public int Id { get; set; }

		public string Description { get; set; }

		public State State { get; set; }

		public bool IsCompleted { get; set; }
	}
}