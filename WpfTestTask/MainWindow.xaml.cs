using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WpfTestTask.Enums;
using WpfTestTask.Models;
using WpfTestTask.Workers;

namespace WpfTestTask
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

#if DEBUG
		private const int RefreshPeriod = 3000;
#else
		private const int RefreshPeriod = 20000;
#endif

		private static readonly string StringForSufix = Guid.NewGuid().ToString();

		private readonly ViewModel _viewModel;

		private readonly object _objForLock;

		private bool _isListChanged;

		private bool _isCellChangingStarted;

		public static bool IsGridValid { get; set; }

		public static List<string> ExpandersStates { get; set; }

		public ViewModel ViewModel
		{
			get { return _viewModel; }
		}

		public MainWindow()
		{
			InitializeComponent();
			_viewModel = new ViewModel();
			DataContext = _viewModel;
			ExpandersStates = new List<string>();
			_objForLock = new object();
		}

		private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
		{
			ViewModel.BindingList.RaiseListChangedEvents = true;
			ViewModel.BindingList.ListChanged += ListChanged;
			
			IsGridValid = true;

			RunBackgroundWorker();
		}

		private void ListChanged(object sender, ListChangedEventArgs e)
		{
			lock (_objForLock)
			{
				_isListChanged = true;
			}
		}

		private void CellChanged(object sender, DataGridCellEditEndingEventArgs e)
		{
			ComboBox comboBox = e.EditingElement as ComboBox;
			
			if (comboBox != null)
			{
				var setToCompleted = string.Equals(comboBox.Text,
					State.Completed.ToString(),
					StringComparison.InvariantCultureIgnoreCase);
				
				if (setToCompleted)
				{
					var item = e.Row.Item as RowModel;
					if (item != null)
					{
						item.IsCompleted = true;
					}
				}
			}

			lock (_objForLock)
			{
				_isCellChangingStarted = false;
				_isListChanged = true;
			}
		}

		private void CellChangingStarted(object sender, DataGridPreparingCellForEditEventArgs dataGridCellEditEndingEventArgs)
		{
			lock (_objForLock)
			{
				_isCellChangingStarted = true;
			}
		}

		private void RunBackgroundWorker()
		{
			Task.Run(() =>
			{
				var bw = new MyBackgroundWorker(this);

				while (true)
				{
					if (!_isCellChangingStarted && IsGridValid)
					{
						if (_isListChanged)
						{
							lock (_objForLock)
							{
								_isListChanged = false;
							}

							bw.WriteData(ViewModel.BindingList.ToList());
						}
						else
						{
							var list = bw.ReadData();
							Dispatcher.Invoke(() =>
							{
								ViewModel.BindingList.Clear();
								foreach (var model in list)
								{
									ViewModel.BindingList.Add(model);
								}

								ViewModel.GroupedModels.Refresh();
							});
						}
					}

					Thread.Sleep(RefreshPeriod);
				}
			});
		}

		private void exp_Expanded(object sender, RoutedEventArgs e)
		{
			ExpandCollapseExpander(sender as Expander, e, false);
		}

		private void exp_Collapsed(object sender, RoutedEventArgs e)
		{
			ExpandCollapseExpander(sender as Expander, e, true);
		}

		private static void ExpandCollapseExpander(Expander exp, RoutedEventArgs e, bool doCollapse)
		{
			CollectionViewGroup collectionViewGroup = exp.DataContext as CollectionViewGroup;

			if (collectionViewGroup == null)
			{
				return;
			}

			string viewGroupId = FormViewGroupIdentifier(collectionViewGroup, null);
			if (doCollapse)
			{
				if (!ExpandersStates.Contains(viewGroupId))
				{
					ExpandersStates.Add(viewGroupId);
				}
			}
			else
			{
				ExpandersStates.Remove(viewGroupId);
			}

			e.Handled = true;
		}

		public static string FormViewGroupIdentifier(CollectionViewGroup collectionViewGroup, string sufix)
		{
			string formViewGroupIdentifier = collectionViewGroup.Name + sufix;
			CollectionViewGroup parentgroup = GetParent(collectionViewGroup);

			if (parentgroup == null)
			{
				return formViewGroupIdentifier;
			}

			return FormViewGroupIdentifier(parentgroup, StringForSufix + formViewGroupIdentifier);
		}

		private static CollectionViewGroup GetParent(CollectionViewGroup collectionViewGroup)
		{
			Type type = collectionViewGroup.GetType();

			// if we are at the root level return null as there is no parent
			if (type.Name == "CollectionViewGroupRoot")
			{
				return null;
			}

			CollectionViewGroup parentgroup =
				type.GetProperty("Parent", System.Reflection.BindingFlags.GetProperty |
											System.Reflection.BindingFlags.Instance |
											System.Reflection.BindingFlags.NonPublic)
				.GetValue(collectionViewGroup, null)
				as CollectionViewGroup;

			return parentgroup;
		}
	}
}
