using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using WpfTestTask.Enums;

namespace WpfTestTask.Models
{
	public class RowModel : IDataErrorInfo, INotifyPropertyChanged
	{
		private static readonly Regex DescriptionRegex = new Regex(@"^[^,]+$");

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
				NotifyErrorChanged();
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
				NotifyErrorChanged();
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
				NotifyErrorChanged();
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
				NotifyErrorChanged();
			}
		}

		public string Error
		{
			get
			{
				StringBuilder sb = new StringBuilder();

				PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this);
				foreach (PropertyDescriptor prop in props)
				{
					string propertyError = this[prop.Name];
					if (!string.IsNullOrEmpty(propertyError))
					{
						sb.Append((sb.Length != 0 ? ", " : "") + propertyError);
					}
				}

				return sb.Length == 0 ? null : sb.ToString();
			}
		}

		public string this[string columnName]
		{
			get
			{
				if (columnName == "Description")
				{
					if (string.IsNullOrEmpty(Description.Trim()))
						return "Description is required";

					if (!DescriptionRegex.Match(Description).Success)
						return "Description shouldn't contain comas";
				}

				return "";
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

		private void NotifyErrorChanged()
		{
			NotifyPropertyChanged("Error");
		}
	}
}