using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class DepartmentViewModel : BaseViewModel, ICrudOperations
    {
        private readonly IDepartmentRepository _departmentRepository;
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

        public DepartmentViewModel(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
            LoadDepartmentsAsync();
        }

        public async void LoadDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllDepartmentsAsync();
            DepartmentList = new ObservableCollection<Department>(departments);
        }

        // ===================== Add =====================
        public async void Add()
        {
            var newDept = new Department();

            // Mở dialog nhập thông tin Department
            var dialog = new DepartmentDialogWindow(newDept)
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                // Thêm vào database
                await _departmentRepository.AddDepartmentAsync(newDept);

                // Thêm vào ObservableCollection
                DepartmentList?.Add(newDept);
                SelectedDepartment = newDept;
            }
        }

        // ===================== Edit =====================
        public async void Edit()
        {
            if (SelectedDepartment == null)
            {
                MessageBox.Show("Vui lòng chọn phòng ban để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new DepartmentDialogWindow(SelectedDepartment)
            {
                Owner = Application.Current.MainWindow
            };

            if (dialog.ShowDialog() == true)
            {
                // Cập nhật database
                await _departmentRepository.UpdateDepartmentAsync(SelectedDepartment);

                // Cập nhật ObservableCollection
                var index = DepartmentList?.IndexOf(SelectedDepartment) ?? -1;
                if (index >= 0 && DepartmentList != null)
                    DepartmentList[index] = SelectedDepartment;
            }
        }

        // ===================== Delete =====================
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