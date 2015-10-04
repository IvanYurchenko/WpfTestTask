using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using WpfTestTask.Enums;

namespace WpfTestTask.Models
{
    public class RowModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private static readonly Regex DescriptionRegex = new Regex(@"^[A-Za-z0-9 ]+$");

        private int _id;
        private string _description;
        private State _state;
        private bool _isCompleted;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged("Id");
                NotifyPropertyChanged("Error");
            }
        }

        public string Description
        {
            get
            {
                return string.IsNullOrEmpty(_description) ? string.Empty : _description;
            }
            set
            {
                _description = value;
                NotifyPropertyChanged("Description");
                NotifyPropertyChanged("Error");
            }
        }

        public State State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                NotifyPropertyChanged("Error");
            }
        }

        public bool IsCompleted
        {
            get
            {
                return _isCompleted;
            }
            set
            {
                _isCompleted = value;
                NotifyPropertyChanged("Error");
            }
        }

        public string this[string columnName]
        {
            get
            {
                string errorMessage = null;
                switch (columnName)
                {
                    case "Description":
                        if (string.IsNullOrEmpty(Description))
                        {
                            errorMessage = "Description is required";
                        }
                        else if (!DescriptionRegex.Match(Description).Success)
                        {
                            errorMessage = "Description may only contain characters, numbers or spaces";
                        }

                        break;

                }

                return errorMessage;
            }
        }

        public string Error
        {
            get
            {
                var sb = new StringBuilder();

                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this);
                foreach (PropertyDescriptor prop in props)
                {
                    string propertyError = this[prop.Name];
                    if (!string.IsNullOrEmpty(propertyError))
                    {
                        sb.Append((sb.Length != 0 ? ", " : "") + propertyError);
                    }
                }

                string result = sb.Length == 0 ? null : sb.ToString();
                MainWindow.IsGridValid = string.IsNullOrEmpty(result);
                return result;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}