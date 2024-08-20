using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using BusinessObject.Models;
using DataAccess.Repositories.IRepository;
using HospitalManagementSystem_WPF.ViewModel;

namespace HospitalManagementSystem_WPF.ViewModel
{
    public class RoomViewModel : BaseViewModel
    {
        /*private readonly IRoomRepository _roomRepository;

        public ObservableCollection<Room> Rooms { get; set; }

        public RoomViewModel(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
            Rooms = new ObservableCollection<Room>();
            LoadRooms();
        }

        private async void LoadRooms()
        {
            var rooms = await _roomRepository.GetAllRoomsAsync();
            foreach (var room in rooms)
            {
                Rooms.Add(room);
            }
        }*/
    }
}