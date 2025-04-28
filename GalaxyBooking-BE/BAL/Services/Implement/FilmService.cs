using AutoMapper;
using BAL.DTOs;
using BAL.Services.Interface;
using DAL.Models;
using DAL.Repository.Interface;
using System.Linq.Expressions;

namespace BAL.Services.Implement
{
    public class FilmService : IFilmService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FilmService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FilmResponseDto> CreateAsync(FilmRequestDto filmDto)
        {
            if (filmDto == null)
                throw new ArgumentNullException(nameof(filmDto));

            var film = _mapper.Map<Film>(filmDto);
            film.Id = Guid.NewGuid();
            film.CreatedBy = filmDto.CreatedBy;
            film.IsDeleted = false;
            film.CreatedAt = DateTime.Now;
            film.UpdatedAt = DateTime.Now;
            film.DeletedAt = null;
            film.Status = filmDto.Status;

            await _unitOfWork.FilmRepository.AddAsync(film);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<FilmResponseDto>(film);
        }

        public async Task<FilmResponseDto> UpdateAsync(Guid id, FilmRequestDto filmDto)
        {
            var film = await _unitOfWork.FilmRepository.GetAsync(
                f => f.Id == id && !f.IsDeleted,
                includeProperties: "FilmGenres");
            if (film == null)
                throw new Exception("Film not found or has been deleted");

            _mapper.Map(filmDto, film);
            film.Status = filmDto.Status;
            film.UpdatedBy = filmDto.UpdatedBy;
            film.UpdatedAt = DateTime.Now;

            await _unitOfWork.FilmRepository.UpdateAsync(film);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<FilmResponseDto>(film);
        }

        public async Task DeleteAsync(Guid id)
        {
            var film = await _unitOfWork.FilmRepository.GetAsync(
                f => f.Id == id && !f.IsDeleted,
                includeProperties: "FilmGenres");
            if (film == null)
                throw new Exception("Film not found or has been deleted");

            film.IsDeleted = true;
            film.DeletedAt = DateTime.Now;
            film.UpdatedAt = DateTime.Now;

            await _unitOfWork.FilmRepository.UpdateAsync(film);
            await _unitOfWork.SaveAsync();
        }

        public async Task<FilmResponseDto> GetByIdAsync(Guid id)
        {
            var film = await _unitOfWork.FilmRepository.GetAsync(
                filter: f => f.Id == id && !f.IsDeleted,
                includeProperties: "FilmGenres.Genre,Projections");
            if (film == null)
                throw new Exception("Film not found or has been deleted");

            return _mapper.Map<FilmResponseDto>(film);
        }

        public async Task<PagedDto<FilmResponseDto>> GetPagingAsync(
            int pageNumber,
            int pageSize,
            string? title = null,
            string? director = null,
            DateTime? releaseDate = null)
        {
            Expression<Func<Film, bool>> filter = f =>
                !f.IsDeleted &&
                (string.IsNullOrEmpty(title) || f.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(director) || f.Director.Contains(director, StringComparison.OrdinalIgnoreCase)) &&
                (!releaseDate.HasValue || f.ReleaseDate.Date == releaseDate.Value.Date);

            var films = await _unitOfWork.FilmRepository.GetPagingAsync(
                filter: filter,
                includeProperties: "FilmGenres",
                orderBy: f => f.Title,
                pageNumber: pageNumber,
                pageSize: pageSize);

            var totalItems = await _unitOfWork.FilmRepository.CountAsync(filter);
            var filmDtos = _mapper.Map<ICollection<FilmResponseDto>>(films);
            return new PagedDto<FilmResponseDto>(pageNumber, pageSize, totalItems, filmDtos);
        }

        public async Task<IEnumerable<FilmResponseDto>> GetFilmsAsync()
        {
            var films = await _unitOfWork.FilmRepository.GetAllAsync(
                filter: f => !f.IsDeleted,
                includeProperties: "FilmGenres.Genre,Projections");
            return _mapper.Map<IEnumerable<FilmResponseDto>>(films);
        }

        public async Task<IEnumerable<FilmResponseDto>> FindByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Enumerable.Empty<FilmResponseDto>();

            var films = await _unitOfWork.FilmRepository.GetFilmsByTitleAsync(title);
            return _mapper.Map<IEnumerable<FilmResponseDto>>(films);
        }

        public async Task<IEnumerable<FilmResponseDto>> FindByDirectorAsync(string director)
        {
            if (string.IsNullOrWhiteSpace(director))
                return Enumerable.Empty<FilmResponseDto>();

            var films = await _unitOfWork.FilmRepository.GetFilmsByDirectorAsync(director);
            return _mapper.Map<IEnumerable<FilmResponseDto>>(films);
        }

        public async Task<IEnumerable<FilmResponseDto>> FindByReleaseDateAsync(DateTime releaseDate)
        {
            var films = await _unitOfWork.FilmRepository.GetFilmsByReleaseDateAsync(releaseDate);
            return _mapper.Map<IEnumerable<FilmResponseDto>>(films);
        }

        public async Task<PagedDto<FilmResponseDto>> GetNewFilmPagingAsync(int pageNumber, int pageSize)
        {
            Expression<Func<Film, bool>> filter = f =>
             f.Status == FilmStatus.New;

            var films = await _unitOfWork.FilmRepository.GetPagingAsync(
                filter: filter,
                includeProperties: "FilmGenres",
                orderBy: f => f.Title,
                pageNumber: pageNumber,
                pageSize: pageSize);

            var totalItems = await _unitOfWork.FilmRepository.CountAsync(filter);
            var filmDtos = _mapper.Map<ICollection<FilmResponseDto>>(films);
            return new PagedDto<FilmResponseDto>(pageNumber, pageSize, totalItems, filmDtos);
        }

        public async Task<PagedDto<FilmResponseDto>> GetInprogressFilmPagingAsync(int pageNumber, int pageSize)
        {
            Expression<Func<Film, bool>> filter = f =>
             f.Status == FilmStatus.InProgress;

            var films = await _unitOfWork.FilmRepository.GetPagingAsync(
                filter: filter,
                includeProperties: "FilmGenres",
                orderBy: f => f.Title,
                pageNumber: pageNumber,
                pageSize: pageSize);

            var totalItems = await _unitOfWork.FilmRepository.CountAsync(filter);
            var filmDtos = _mapper.Map<ICollection<FilmResponseDto>>(films);
            return new PagedDto<FilmResponseDto>(pageNumber, pageSize, totalItems, filmDtos);
        }

        public async Task<PagedDto<FilmResponseDto>> GetEndFilmPagingAsync(int pageNumber, int pageSize)
        {
            Expression<Func<Film, bool>> filter = f =>
             f.IsDeleted && f.Status == FilmStatus.End;

            var films = await _unitOfWork.FilmRepository.GetPagingAsync(
                filter: filter,
                includeProperties: "FilmGenres",
                orderBy: f => f.Title,
                pageNumber: pageNumber,
                pageSize: pageSize);

            var totalItems = await _unitOfWork.FilmRepository.CountAsync(filter);
            var filmDtos = _mapper.Map<ICollection<FilmResponseDto>>(films);
            return new PagedDto<FilmResponseDto>(pageNumber, pageSize, totalItems, filmDtos);
        }

        public async Task<bool> UpdateFilmStatusCronJobs()
        {
            var films = await _unitOfWork.FilmRepository.GetAllAsync();
            var now = DateTime.Now;
            List<Film> filmsUpdate = new List<Film>();
            foreach (var item in films.ToList())
            {
                if (now < item.ReleaseDate)
                {
                    item.Status = FilmStatus.New;
                    filmsUpdate.Add(item);
                }
                else if (now >= item.ReleaseDate && now <= item.ReleaseDate.AddMonths(1))
                {
                    item.Status = FilmStatus.InProgress;
                    filmsUpdate.Add(item);
                }
                else if (now > item.ReleaseDate.AddMonths(1))
                {
                    item.Status = FilmStatus.End;
                    item.IsDeleted = true;
                    filmsUpdate.Add(item);
                }
            }

            await _unitOfWork.FilmRepository.UpdateRange(filmsUpdate);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}