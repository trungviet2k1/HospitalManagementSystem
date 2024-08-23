using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using System.Collections.ObjectModel;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class StaffViewModel : BaseViewModel
    {
        private readonly IUserRepository _userRepository;
        private ObservableCollection<User>? _userList;

        public ObservableCollection<User>? StaffList
        {
            get => _userList;
            set => SetProperty(ref _userList, value);
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