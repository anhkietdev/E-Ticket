using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repository.Interface
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<Room> FindByIdAsync(Guid id);
        Task<Room> FindByRoomNumberAsync(string roomNumber);
        Task<ICollection<Room>> FindByRoomTypeAsync(RoomType roomType);
    }
}