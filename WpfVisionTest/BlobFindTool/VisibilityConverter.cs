using System;
using System.Windows;
using System.Windows.Data;

namespace WpfVisionTest.BlobFindTool
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is VisibilityType visibilityType)
            {
                return visibilityType == VisibilityType.Beginner ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
