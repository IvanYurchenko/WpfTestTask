using System.Collections.Generic;
using System.ComponentModel;
using WpfTestTask.Models;
using WpfTestTask.Services;

namespace WpfTestTask.Workers
{
	public class MyBackgroundWorker
	{
		private readonly FileService _fileService;

		private readonly MainWindow _mainWindow;

		public MyBackgroundWorker(MainWindow mainWindow)
		{
			_mainWindow = mainWindow;
			
			_fileService = new FileService();
		}

		public BindingList<RowModel> ReadData()
		{
			var list = _fileService.ReadData();
			return list;
		}

		public void WriteData(List<RowModel> list)
		{
			_fileService.WriteData(_mainWindow.ViewModel.BindingList);
		}
	}
}