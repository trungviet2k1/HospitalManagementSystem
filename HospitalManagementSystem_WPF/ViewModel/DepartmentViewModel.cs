using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using System.Collections.ObjectModel;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class DepartmentViewModel : BaseViewModel
    {
        private readonly IDepartmentRepository _departmentRepository;
        private ObservableCollection<Department>? _departmentList;

        public ObservableCollection<Department>? DepartmentList
        {
            get => _departmentList;
            set => SetProperty(ref _departmentList, value);
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
    }
}