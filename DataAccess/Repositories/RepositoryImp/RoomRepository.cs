using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepository;

namespace DataAccess.Repositories.RepositoryImp
{
    public class RoomRepository : IRoomRepository
    {
        private readonly RoomDAO _roomDAO;

        public RoomRepository(RoomDAO roomDAO) => _roomDAO = roomDAO;

        public Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return _roomDAO.GetAllRoomsAsync();
        }

        public Task<Room> GetRoomByIdAsync(int roomId)
        {
            return _roomDAO.GetRoomByIdAsync(roomId);
        }

        public Task AddRoomAsync(Room room)
        {
            return _roomDAO.AddRoomAsync(room);
        }

        public Task DeleteRoomAsync(int roomId)
        {
            return _roomDAO.DeleteRoomAsync(roomId);
        }

        public Task UpdateRoomAsync(Room room)
        {
            return _roomDAO.UpdateRoomAsync(room);
        }
    }
}