using System.Windows;

namespace HospitalManagementSystem_WPF
{
    public partial class MedicationDialogWindow : Window
    {
        public MedicationDialogWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}