using System.Globalization;
using System.Windows.Data;
using BusinessObject.Models;

namespace HospitalManagementSystem_WPF
{
    public class UserGreetingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is User user && user.Role != null)
            {
                return $"Hello, {user.FullName} - {user.Role.RoleName}";
            }
            return "Hello, Guest";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}