using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using HospitalManagementSystem.HospitalManagementSystem_WPF;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class MedicationViewModel : BaseViewModel, ICrudOperations
    {
        private readonly IMedicationRepository _medicationRepository;
        private Medication _selectedMedication;
        private ObservableCollection<Medication> _medicationList;
        private ObservableCollection<Inventory> _inventories;

        public Medication SelectedMedication
        {
            get => _selectedMedication;
            set => SetProperty(ref _selectedMedication, value);
        }

        public ObservableCollection<Medication> MedicationList
        {
            get => _medicationList;
            set => SetProperty(ref _medicationList, value);
        }

        public ObservableCollection<Inventory> Inventories
        {
            get => _inventories;
            set => SetProperty(ref _inventories, value);
        }

        public ICommand LoadMedicationsCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public MedicationViewModel(IMedicationRepository medicationRepository)
        {
            _medicationRepository = medicationRepository;
            MedicationList = new ObservableCollection<Medication>();
            Inventories = new ObservableCollection<Inventory>();

            LoadMedicationsCommand = new RelayCommand(async (param) => await LoadMedications());
            AddCommand = new RelayCommand((param) => Add());
            EditCommand = new RelayCommand((param) => Edit());
            DeleteCommand = new RelayCommand(async (param) => await Delete());

            // Tải dữ liệu ban đầu
            LoadMedicationsCommand.Execute(null);
        }

        private async Task LoadMedications()
        {
            try
            {
                var medications = await _medicationRepository.GetAllMedicationsAsync();
                MedicationList.Clear();
                foreach (var med in medications)
                {
                    MedicationList.Add(med);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách thuốc: {ex.Message}");
            }
        }

        public void Add()
        {
            try
            {
                var dialog = App.ServiceProvider.GetRequiredService<MedicationDialogWindow>();
                var vm = new MedicationDialogViewModel(_medicationRepository);
                dialog.DataContext = vm;

                if (dialog.ShowDialog() == true)
                {
                    LoadMedicationsCommand.Execute(null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm thuốc: {ex.Message}");
            }
        }

        public void Edit()
        {
            if (SelectedMedication == null)
            {
                MessageBox.Show("Vui lòng chọn thuốc để chỉnh sửa");
                return;
            }

            try
            {
                var dialog = App.ServiceProvider.GetRequiredService<MedicationDialogWindow>();
                var vm = new MedicationDialogViewModel(_medicationRepository, SelectedMedication);
                dialog.DataContext = vm;

                if (dialog.ShowDialog() == true)
                {
                    LoadMedicationsCommand.Execute(null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chỉnh sửa thuốc: {ex.Message}");
            }
        }

        public async Task Delete()
        {
            if (SelectedMedication == null)
            {
                MessageBox.Show("Vui lòng chọn thuốc để xóa");
                return;
            }

            var result = MessageBox.Show(
                $"Bạn có chắc muốn xóa thuốc '{SelectedMedication.MedicationName}'?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _medicationRepository.DeleteMedicationAsync(SelectedMedication.MedicationId);
                    MessageBox.Show("Thuốc đã được xóa thành công");
                    await LoadMedications();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa thuốc: {ex.Message}");
                }
            }
        }

        // Cập nhật inventory khi chọn medication
        private void UpdateInventory()
        {
            if (SelectedMedication != null && SelectedMedication.Inventories != null)
            {
                Inventories.Clear();
                foreach (var inventory in SelectedMedication.Inventories)
                {
                    Inventories.Add(inventory);
                }
            }
            else
            {
                Inventories.Clear();
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(SelectedMedication))
            {
                UpdateInventory();
            }
        }

        async void ICrudOperations.Delete()
        {
            await Delete();
        }
    }
}