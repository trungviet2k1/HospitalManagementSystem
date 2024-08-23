using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using System.Collections.ObjectModel;

namespace HospitalManagementSystem_WPF.ViewModel
{
     public class PatientViewModel : BaseViewModel
    {
        private readonly IPatientRepository _patientRepository;
        private ObservableCollection<Patient>? _patientList;

        public ObservableCollection<Patient>? PatientList
        {
            get => _patientList;
            set => SetProperty(ref _patientList, value);
        }

        public PatientViewModel(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
            LoadRoomAsync();
        }

        public async void LoadRoomAsync()
        {
            var patient = await _patientRepository.GetAllPatientsAsync();
            PatientList = new ObservableCollection<Patient>(patient);
        }
    }
}