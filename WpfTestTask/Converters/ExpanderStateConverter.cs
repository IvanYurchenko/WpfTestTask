using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace WpfTestTask.Converters
{
	public class ExpanderStateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			CollectionViewGroup collectionViewGroup = value as CollectionViewGroup;
			List<string> expandersStates = MainWindow.ExpandersStates;

			if (expandersStates == null || !expandersStates.Any())
			{
				return false;
			}

			string groupId = MainWindow.FormViewGroupIdentifier(collectionViewGroup, null);
			bool contains = expandersStates.Contains(groupId);
			return contains;
		}

		public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
		{
			return new object();
		}
	}
}