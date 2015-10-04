using System.ComponentModel;
using WpfTestTask.Enums;
using WpfTestTask.Models;

namespace WpfTestTask
{
	public class ViewModel
	{
		public ViewModel()
		{
		    BindingList = new BindingList<RowModel>();
		}

		public BindingList<RowModel> BindingList { get; set; }
	}
}