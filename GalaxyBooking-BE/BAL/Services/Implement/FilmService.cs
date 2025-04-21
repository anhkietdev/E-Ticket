using AutoMapper;
using BAL.DTOs;
using BAL.Services.Interface;
using DAL.Models;
using DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

        public async Task CreateAsync(FilmDto filmDto)
        {
            if (filmDto == null)
                throw new ArgumentNullException(nameof(filmDto));

            var film = _mapper.Map<Film>(filmDto);
            await _unitOfWork.FilmRepository.AddAsync(film);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Guid id, FilmDto filmDto)
        {
            var film = await _unitOfWork.FilmRepository.GetAsync(
                f => f.Id == id && !f.IsDeleted,
                includeProperties: "FilmGenres");
            if (film == null)
                throw new Exception("Film not found or has been deleted");

            _mapper.Map(filmDto, film);
            await _unitOfWork.FilmRepository.UpdateAsync(film);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var film = await _unitOfWork.FilmRepository.GetAsync(
                f => f.Id == id && !f.IsDeleted,
                includeProperties: "FilmGenres");
            if (film == null)
                throw new Exception("Film not found or has been deleted");

            await _unitOfWork.FilmRepository.RemoveAsync(film);
            await _unitOfWork.SaveAsync();
        }

        public async Task<FilmDto> GetByIdAsync(Guid id)
        {
            var film = await _unitOfWork.FilmRepository.GetAsync(
                filter: f => f.Id == id && !f.IsDeleted,
                includeProperties: "FilmGenres");
            if (film == null)
                throw new Exception("Film not found or has been deleted");

            return _mapper.Map<FilmDto>(film);
        }

        public async Task<PagedDto<FilmDto>> GetPagingAsync(
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
            var filmDtos = _mapper.Map<ICollection<FilmDto>>(films);
            return new PagedDto<FilmDto>(pageNumber, pageSize, totalItems, filmDtos);
        }

        public async Task<IEnumerable<FilmDto>> GetFilmsAsync()
        {
            var films = await _unitOfWork.FilmRepository.GetAllAsync(
                filter: f => !f.IsDeleted,
                includeProperties: "FilmGenres");
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }

        public async Task<IEnumerable<FilmDto>> FindByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Enumerable.Empty<FilmDto>();

            var films = await _unitOfWork.FilmRepository.GetFilmsByTitleAsync(title);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }

        public async Task<IEnumerable<FilmDto>> FindByDirectorAsync(string director)
        {
            if (string.IsNullOrWhiteSpace(director))
                return Enumerable.Empty<FilmDto>();

            var films = await _unitOfWork.FilmRepository.GetFilmsByDirectorAsync(director);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }

        public async Task<IEnumerable<FilmDto>> FindByReleaseDateAsync(DateTime releaseDate)
        {
            var films = await _unitOfWork.FilmRepository.GetFilmsByReleaseDateAsync(releaseDate);
            return _mapper.Map<IEnumerable<FilmDto>>(films);
        }
    }
}