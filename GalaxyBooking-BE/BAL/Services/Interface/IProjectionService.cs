using BAL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interface
{
    public interface IProjectionService
    {
        Task<IEnumerable<ProjectionDto>> GetAllAsync();
        Task<ProjectionDto> GetByIdAsync(Guid id);
        Task AddAsync(ProjectionDto projection);
        Task UpdateAsync(ProjectionDto projection);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ProjectionDto>> FindByFilmIdAsync(Guid filmId);
        Task<IEnumerable<ProjectionDto>> FindByRoomIdAsync(Guid roomId);
    }
}
