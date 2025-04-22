using BAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BAL.Services.Interface
{
    public interface IProjectionService
    {
        Task<ProjectionResponseDto> CreateAsync(ProjectionRequestDto projectionDto);
        Task<ProjectionResponseDto> UpdateAsync(Guid id, ProjectionRequestDto projectionDto);
        Task DeleteAsync(Guid id);
        Task<ProjectionResponseDto> GetByIdAsync(Guid id);
        Task<PagedDto<ProjectionResponseDto>> GetPagingAsync(
            int pageNumber,
            int pageSize,
            Guid? filmId = null,
            Guid? roomId = null,
            DateTime? startTime = null);
        Task<IEnumerable<ProjectionResponseDto>> GetAllAsync();
        Task<IEnumerable<ProjectionResponseDto>> FindByFilmIdAsync(Guid filmId);
        Task<IEnumerable<ProjectionResponseDto>> FindByRoomIdAsync(Guid roomId);
    }
}