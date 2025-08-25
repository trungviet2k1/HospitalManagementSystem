using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Repositories.IRepository;

namespace DataAccess.Repositories.RepositoryImp
{
    public class RoomRepository(RoomDAO roomDAO) : IRoomRepository
    {
        public Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return roomDAO.GetAllRoomsAsync();
        }

        public Task<Room> GetRoomByIdAsync(int roomId)
        {
            return roomDAO.GetRoomByIdAsync(roomId);
        }

        public Task AddRoomAsync(Room room)
        {
            return roomDAO.AddRoomAsync(room);
        }

        public Task DeleteRoomAsync(int roomId)
        {
            return roomDAO.DeleteRoomAsync(roomId);
        }

        public Task UpdateRoomAsync(Room room)
        {
            return roomDAO.UpdateRoomAsync(room);
        }
    }
}