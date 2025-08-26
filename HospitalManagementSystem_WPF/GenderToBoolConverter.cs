using System.Globalization;
using System.Windows.Data;

namespace HospitalManagementSystem_WPF
{
    public class GenderToBoolConverter : IValueConverter
    {
        // Convert từ string Gender sang bool
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            var gender = value.ToString();
            var param = parameter.ToString(); // "Male" hoặc "Female"

            return string.Equals(gender, param, StringComparison.OrdinalIgnoreCase);
        }

        // Convert ngược từ bool sang string Gender
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return null;

            var isChecked = (bool)value;
            var param = parameter.ToString(); // "Male" hoặc "Female"

            return isChecked ? param : Binding.DoNothing;
        }
    }
}