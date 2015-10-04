using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public static bool IsGridValid { get; set; }

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
            
            IsGridValid = true;
            RunBackgroundWorker();
        }

        private void ListChanged(object sender, ListChangedEventArgs e)
        {
            _isCellChangingStarted = false;
            _isListChanged = true;
        }

        private void CellChanged(object sender, DataGridCellEditEndingEventArgs dataGridCellEditEndingEventArgs)
        {
            _isCellChangingStarted = false;
            _isListChanged = true;
        }

        private void CellChangingStarted(object sender, DataGridPreparingCellForEditEventArgs dataGridCellEditEndingEventArgs)
        {
            _isCellChangingStarted = true;
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
                            _isListChanged = false;
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
                            });
                        }
                    }
                    Thread.Sleep(1000);
                }

            });
        }
    }
}
