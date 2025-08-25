using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using System.Collections.ObjectModel;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class StaffViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepository;
        private ObservableCollection<User>? _staffList;

        public ObservableCollection<User>? StaffList
        {
            get => _staffList;
            set => SetProperty(ref _staffList, value);
        }

        public StaffViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            LoadStaffsAsync();
        }

        public async void LoadStaffsAsync()
        {
            var staffs = await _userRepository.GetAllUsersAsync();
            StaffList = new ObservableCollection<User>(staffs);
        }
    }
}