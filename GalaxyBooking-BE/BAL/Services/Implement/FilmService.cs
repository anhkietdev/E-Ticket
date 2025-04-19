using BAL.DTOs;
using BAL.Services.Interface;
using DAL.Models;
using DAL.Repository.Interface;

namespace BAL.Services.Implement
{
    public class FilmService : IFilmService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilmService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task CreateAsync(FilmDto filmDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<FilmDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Film>> GetFilms()
        {
            try
            {
                var films = await _unitOfWork.FilmRepository.GetAllAsync();
                return films.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFilms: {ex.Message}");
                throw;
            }
        }

        public Task<PagedDto<FilmDto>> GetPagingAsync(int pageNumber, int pageSize, string? title = null, string? director = null, DateTime? releaseDate = null)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, FilmDto filmDto)
        {
            throw new NotImplementedException();
        }
    }
}
