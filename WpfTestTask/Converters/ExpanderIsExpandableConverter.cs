using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using WpfTestTask.Models;

namespace WpfTestTask.Converters
{
	public class ExpanderIsExpandableConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var group = value as CollectionViewGroup;
			if (group == null)
			{
				return false;
			}

			var item = group.Items.FirstOrDefault() as RowModel;

			if (item == null)
			{
				return false;
			}

			return item.IsCompleted;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}