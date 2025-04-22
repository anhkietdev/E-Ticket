using BAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interface
{
    public interface IRoomService
    {
        // CRUD Operations
        Task<RoomResponseDto> CreateAsync(RoomRequestDto roomDto);
        Task<RoomResponseDto> UpdateAsync(Guid id, RoomRequestDto roomDto);
        Task DeleteAsync(Guid id);
        Task<RoomResponseDto> GetByIdAsync(Guid id);
        Task<ICollection<RoomResponseDto>> GetAllAsync();

        // Search Operations
        Task<RoomResponseDto> FindByIdAsync(Guid id);
        Task<RoomResponseDto> FindByRoomNumberAsync(string roomNumber);
        Task<ICollection<RoomResponseDto>> FindByRoomTypeAsync(RoomType roomType);
    }
}
