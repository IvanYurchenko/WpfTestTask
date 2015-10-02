using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfTestTask.Models;
using WpfTestTask.Services;
using WpfTestTask.Workers;

namespace WpfTestTask
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly ViewModel _viewModel;

		private bool _isListChanged;

		private bool _isCellChangingStarted;

		private bool _isReading;

		public ViewModel ViewModel
		{
			get { return _viewModel; }
		}

		public MainWindow()
		{
			InitializeComponent();
			_viewModel = new ViewModel();
			DataContext = _viewModel;
		}

		private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
		{
			_viewModel.BindingList.RaiseListChangedEvents = true;
			_viewModel.BindingList.ListChanged += ListChanged;

			Task.Run((() =>
			{
				var bw = new MyBackgroundWorker(this);

				while (true)
				{
					if (!_isCellChangingStarted)
					{
						if (_isListChanged)
						{
							_isListChanged = false;
							bw.WriteData(ViewModel.BindingList.ToList());
						}
						else
						{
							_isReading = true;
							var list = bw.ReadData();
							Dispatcher.Invoke(() =>
							{
								ViewModel.BindingList.Clear();
								foreach (var model in list)
								{
									ViewModel.BindingList.Add(model);
								}
							});
							_isReading = false;
						}
					}

					Thread.Sleep(1000);
				}
			}));
		}
		
		void ListChanged(object sender, ListChangedEventArgs e)
		{
			_isCellChangingStarted = false;
			_isListChanged = true;
		}

		void CellChanged(object sender, DataGridCellEditEndingEventArgs dataGridCellEditEndingEventArgs)
		{
			_isCellChangingStarted = false;
			_isListChanged = true;
		}

		void CellChangingStarted(object sender, DataGridPreparingCellForEditEventArgs dataGridCellEditEndingEventArgs)
		{
			_isCellChangingStarted = true;
		}
	}
}
