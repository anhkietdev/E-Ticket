using AutoMapper;
using BAL.DTOs;
using BAL.Services.Interface;
using DAL.Models;
using DAL.Repository.Interface;
using System.Linq.Expressions;

namespace BAL.Services.Implement
{
    public class FilmService2 : IFilmService2
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FilmService2(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(FilmDto filmDto)
        {
            var film = new Film
            {
                Title = filmDto.Title,
                Description = filmDto.Description,
                Duration = filmDto.Duration,
                Director = filmDto.Director,
                ReleaseDate = filmDto.ReleaseDate,
                FilmGenres = filmDto.FilmGenres,
                Projections = filmDto.Projections
            };
            await _unitOfWork.FilmRepository.AddAsync(film);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            Film? film = await _unitOfWork.FilmRepository.GetAsync(f => f.Id == id);
            if (film == null)
            {
                throw new Exception("Film not found");
            }
            await _unitOfWork.FilmRepository.RemoveAsync(film);
            await _unitOfWork.SaveAsync();
        }

        public async Task<PagedDto<FilmDto>> GetPagingAsync(int pageNumber, int pageSize, string? title = null, string? director = null, DateTime? releaseDate = null)
        {
            Expression<Func<Film, bool>> filter = f =>
                (string.IsNullOrEmpty(title) || f.Title.Contains(title)) &&
                (string.IsNullOrEmpty(director) || f.Director.Contains(director)) &&
                (!releaseDate.HasValue || f.ReleaseDate.Date == releaseDate.Value.Date);

            var films = await _unitOfWork.FilmRepository.GetPagingAsync(
                filter: filter,
                orderBy: f => f.Title,
                pageNumber: pageNumber,
                pageSize: pageSize
            );

            var totalItems = await _unitOfWork.FilmRepository.CountAsync(filter);
            var filmDtos = _mapper.Map<ICollection<FilmDto>>(films);
            return new PagedDto<FilmDto>(pageNumber, pageSize, totalItems, filmDtos);
        }

        public async Task<FilmDto> GetByIdAsync(Guid id)
        {
            var film = await _unitOfWork.FilmRepository.GetAsync(f => f.Id == id);
            if (film == null)
            {
                throw new Exception("Film not found");
            }
            return _mapper.Map<FilmDto>(film);
        }

        public async Task UpdateAsync(Guid id, FilmDto filmDto)
        {
            var film = await _unitOfWork.FilmRepository.GetAsync(f => f.Id == id);
            if (film == null)
            {
                throw new Exception("Film not found");
            }
            // Update the film properties
            film.Title = filmDto.Title;
            film.Description = filmDto.Description;
            film.Duration = filmDto.Duration;
            film.Director = filmDto.Director;
            film.ReleaseDate = filmDto.ReleaseDate;
            film.FilmGenres = filmDto.FilmGenres;
            film.Projections = filmDto.Projections;

            // Update the film in the repository
            await _unitOfWork.FilmRepository.UpdateAsync(film);
            await _unitOfWork.SaveAsync();
        }
    }
}
