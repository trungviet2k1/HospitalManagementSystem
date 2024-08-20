using HospitalManagementSystem.BusinessObject.Models;
using HospitalManagementSystem.DataAccess.Repositories.IRepository;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HospitalManagementSystem.WPF.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public ObservableCollection<Appointment> Appointments { get; set; }

        public ICommand LoadAppointmentsCommand { get; }

        public MainViewModel(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            LoadAppointmentsCommand = new RelayCommand(LoadAppointments);
        }

        private void LoadAppointments()
        {
            var appointments = _appointmentRepository.GetAllAppointments();
            Appointments = new ObservableCollection<Appointment>(appointments);
            OnPropertyChanged(nameof(Appointments));
        }
    }
}