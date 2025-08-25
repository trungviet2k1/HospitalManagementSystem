using System.Globalization;
using System.Windows.Data;

namespace HospitalManagementSystem_WPF
{
    public class BooleanToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                // Nếu có parameter thì xử lý theo parameter
                var context = parameter?.ToString();

                return context switch
                {
                    "User" => boolValue ? "Active" : "Inactive",
                    "Patient" => boolValue ? "Admitted" : "Discharged",
                    "Room" => boolValue ? "Available" : "Occupied",
                    _ => boolValue ? "Yes" : "No"
                };
            }

            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}