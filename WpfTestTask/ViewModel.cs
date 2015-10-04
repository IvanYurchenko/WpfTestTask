using System.ComponentModel;
using System.Windows.Data;
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

			GroupedModels = new ListCollectionView(BindingList);
			GroupedModels.GroupDescriptions.Add(new PropertyGroupDescription("IsCompleted"));
		}

		public BindingList<RowModel> BindingList { get; set; }

		public ICollectionView GroupedModels { get; set; }
	}
}