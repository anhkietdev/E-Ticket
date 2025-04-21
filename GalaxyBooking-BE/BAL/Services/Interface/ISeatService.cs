using BAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL.Services.Interface
{
    public interface ISeatService
    {
        Task<SeatResponseDto> CreateAsync(SeatRequestDto seatDto);
        Task<SeatResponseDto> UpdateAsync(Guid id, SeatRequestDto seatDto);
        Task DeleteAsync(Guid id);
        Task<SeatResponseDto> GetByIdAsync(Guid id);
        Task<PagedDto<SeatResponseDto>> GetPagingAsync(
            int pageNumber,
            int pageSize,
            Guid? roomId = null,
            string? seatNumber = null,
            string? row = null);
        Task<IEnumerable<SeatResponseDto>> GetAllAsync();
        Task<SeatResponseDto> FindByIdAsync(Guid id);
    }
}