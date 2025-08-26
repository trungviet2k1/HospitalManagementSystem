using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HospitalManagementSystem_WPF
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Hiển thị buttons chỉ khi CurrentViewModel không phải là NoPermissionViewModel
            return value is ViewModel.NoPermissionViewModel ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}