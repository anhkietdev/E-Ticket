using BAL.DTOs;

namespace BAL.Services.Interface
{
    public interface IGenreService
    {
        Task<GenreResponseDto> CreateAsync(GenreRequestDto genreDto);
        Task<GenreResponseDto> UpdateAsync(Guid id, GenreRequestDto genreDto);
        Task DeleteAsync(Guid id);
        Task<GenreResponseDto> GetByIdAsync(Guid id);
        Task<PagedDto<GenreResponseDto>> GetPagingAsync(
            int pageNumber,
            int pageSize,
            string? name = null);
        Task<IEnumerable<GenreResponseDto>> GetAllAsync();
        Task<GenreResponseDto> FindByIdAsync(Guid id);
    }
}