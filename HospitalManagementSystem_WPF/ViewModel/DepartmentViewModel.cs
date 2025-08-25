using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using System.Collections.ObjectModel;
using System.Windows;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class DepartmentViewModel : BaseViewModel, ICrudOperations
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;

        private ObservableCollection<Department>? _departmentList;
        private Department? _selectedDepartment;

        public ObservableCollection<Department>? DepartmentList
        {
            get => _departmentList;
            set => SetProperty(ref _departmentList, value);
        }

        public Department? SelectedDepartment
        {
            get => _selectedDepartment;
            set => SetProperty(ref _selectedDepartment, value);
        }

        public DepartmentViewModel(IDepartmentRepository departmentRepository, IUserRepository userRepository)
        {
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;

            LoadDepartmentsAsync();
        }

        public async void LoadDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllDepartmentsAsync();
            DepartmentList = [.. departments];
        }

        public async void Add()
        {
            var newDept = new Department();
            var allUsers = new ObservableCollection<User>(await _userRepository.GetAllUsersAsync());

            var dialog = new View.DepartmentDialogWindow(newDept, allUsers)
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                // Cập nhật DepartmentHeadId nếu có
                if (newDept.DepartmentHead != null)
                    newDept.DepartmentHeadId = newDept.DepartmentHead.UserId;

                // Thêm vào database
                await _departmentRepository.AddDepartmentAsync(newDept);

                // Cập nhật ObservableCollection
                DepartmentList?.Add(newDept);
                SelectedDepartment = newDept;
            }
        }

        public async void Edit()
        {
            if (SelectedDepartment == null)
            {
                MessageBox.Show("Vui lòng chọn phòng ban để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var allUsers = new ObservableCollection<User>(await _userRepository.GetAllUsersAsync());

            var dialog = new View.DepartmentDialogWindow(SelectedDepartment, allUsers)
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                // Cập nhật DepartmentHeadId nếu có
                if (SelectedDepartment.DepartmentHead != null)
                    SelectedDepartment.DepartmentHeadId = SelectedDepartment.DepartmentHead.UserId;
                else
                    SelectedDepartment.DepartmentHeadId = null;

                // Cập nhật database
                await _departmentRepository.UpdateDepartmentAsync(SelectedDepartment);

                // Refresh ObservableCollection
                var index = DepartmentList?.IndexOf(SelectedDepartment) ?? -1;
                if (index >= 0 && DepartmentList != null)
                    DepartmentList[index] = SelectedDepartment;
            }
        }

        public async void Delete()
        {
            if (SelectedDepartment == null)
            {
                MessageBox.Show("Vui lòng chọn phòng ban để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa phòng ban {SelectedDepartment.DepartmentName}?",
                                         "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await _departmentRepository.DeleteDepartmentAsync(SelectedDepartment.DepartmentId);
                DepartmentList?.Remove(SelectedDepartment);
                SelectedDepartment = null;
            }
        }
    }
}