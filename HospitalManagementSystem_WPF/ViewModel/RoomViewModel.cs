using System.Collections.ObjectModel;
using BusinessObject.Models;
using DataAccess.Repositories.IRepository;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class RoomViewModel : BaseViewModel
    {
        private readonly IRoomRepository _roomRepository;
        private ObservableCollection<Room>? _roomList;

        public ObservableCollection<Room>? RoomList
        {
            get => _roomList;
            set => SetProperty(ref _roomList, value);
        }

        public RoomViewModel(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
            LoadRoomAsync();
        }

        public async void LoadRoomAsync()
        {
            var rooms = await _roomRepository.GetAllRoomsAsync();
            RoomList = new ObservableCollection<Room>(rooms);
        }
    }
}