using System.ComponentModel;
using WpfTestTask.Enums;
using WpfTestTask.Models;

namespace WpfTestTask
{
	public class ViewModel
	{
		public ViewModel()
		{
			BindingList = new BindingList<RowModel>
			{
				new RowModel
				{
					Id = 1,
					Description = "Foo",
					State = State.Opened,
					IsCompleted = false,
				},
				new RowModel
				{
					Id = 2,
					Description = "Bar",
					State = State.InProgress,
					IsCompleted = true,
				}
			};
		}

		public BindingList<RowModel> BindingList { get; set; }
	}
}