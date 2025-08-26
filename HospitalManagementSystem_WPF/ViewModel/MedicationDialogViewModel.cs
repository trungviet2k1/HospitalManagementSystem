using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BusinessObject.Models;
using DataAccess.Repositories.IRepository;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class MedicationDialogViewModel : BaseViewModel
    {
        private readonly IMedicationRepository _medicationRepository;
        private Medication _medication;
        private string _location;

        public int MedicationId { get; set; }
        public string MedicationName { get; set; }
        public int QuantityInStock { get; set; }
        public string Unit { get; set; }
        public string Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        public ObservableCollection<Inventory> Inventories { get; set; }

        public ICommand SaveCommand { get; }

        public MedicationDialogViewModel(IMedicationRepository medicationRepository, Medication medication = null)
        {
            _medicationRepository = medicationRepository;
            Inventories = new ObservableCollection<Inventory>();

            if (medication != null)
            {
                // Chế độ chỉnh sửa
                _medication = medication;
                MedicationId = medication.MedicationId;
                MedicationName = medication.MedicationName;
                QuantityInStock = medication.QuantityInStock;
                Unit = medication.Unit;

                // Load inventories
                if (medication.Inventories != null)
                {
                    foreach (var inv in medication.Inventories)
                    {
                        Inventories.Add(inv);
                    }
                }
            }
            else
            {
                // Chế độ thêm mới
                _medication = new Medication();
                QuantityInStock = 0;
                Unit = "Hộp"; // Default unit
            }

            SaveCommand = new RelayCommand(async (param) => await SaveMedication());
        }

        private async Task SaveMedication()
        {
            if (string.IsNullOrEmpty(MedicationName))
            {
                MessageBox.Show("Vui lòng nhập tên thuốc");
                return;
            }

            try
            {
                if (_medication.MedicationId == 0) // Thêm mới
                {
                    var newMedication = new Medication
                    {
                        MedicationName = MedicationName,
                        QuantityInStock = QuantityInStock,
                        Unit = Unit,
                        IsDeleted = false,
                        Inventories = []
                    };

                    // Thêm inventory record nếu có số lượng
                    if (QuantityInStock > 0 && !string.IsNullOrEmpty(Location))
                    {
                        newMedication.Inventories.Add(new Inventory
                        {
                            Quantity = QuantityInStock,
                            Location = Location,
                            LastUpdated = DateTime.Now
                        });
                    }

                    await _medicationRepository.AddMedicationAsync(newMedication);
                    MessageBox.Show("Thêm thuốc thành công");
                }
                else // Chỉnh sửa
                {
                    _medication.MedicationName = MedicationName;
                    _medication.QuantityInStock = QuantityInStock;
                    _medication.Unit = Unit;

                    // Cập nhật inventory nếu có thay đổi
                    if (QuantityInStock > 0 && !string.IsNullOrEmpty(Location))
                    {
                        var existingInventory = _medication.Inventories?.FirstOrDefault();
                        if (existingInventory != null)
                        {
                            existingInventory.Quantity = QuantityInStock;
                            existingInventory.Location = Location;
                            existingInventory.LastUpdated = DateTime.Now;
                        }
                        else
                        {
                            _medication.Inventories ??= new List<Inventory>();
                            _medication.Inventories.Add(new Inventory
                            {
                                Quantity = QuantityInStock,
                                Location = Location,
                                LastUpdated = DateTime.Now
                            });
                        }
                    }

                    await _medicationRepository.UpdateMedicationAsync(_medication);
                    MessageBox.Show("Cập nhật thuốc thành công");
                }

                // Đóng dialog
                CloseWindow(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thuốc: {ex.Message}");
            }
        }

        private void CloseWindow(bool dialogResult)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.DialogResult = dialogResult;
                    window.Close();
                    break;
                }
            }
        }
    }
}