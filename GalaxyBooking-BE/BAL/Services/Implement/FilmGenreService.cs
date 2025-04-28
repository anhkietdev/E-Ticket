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
    public class FilmGenreService : IFilmGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FilmGenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FilmGenresResponseDto> CreateAsync(FilmGenreRequestDto filmGenreDto)
        {
            //Validate of genre is exist?
            var checkGerneOrFilmExist = await _unitOfWork.FilmGenreRepository
                .GetAsync(fg => fg.FilmId == filmGenreDto.FilmId && fg.GenreId == filmGenreDto.GenreId && !fg.IsDeleted);
            if (checkGerneOrFilmExist != null)
                throw new Exception("Film genre already exists.");

            if (filmGenreDto == null)
                throw new ArgumentNullException(nameof(filmGenreDto));

            var filmGenre = _mapper.Map<FilmGenre>(filmGenreDto);
            filmGenre.Id = Guid.NewGuid();
            filmGenre.IsDeleted = false;
            filmGenre.CreatedAt = DateTime.Now;
            filmGenre.UpdatedAt = filmGenre.CreatedAt;
            filmGenre.DeletedAt = null;

            await _unitOfWork.FilmGenreRepository.AddAsync(filmGenre);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<FilmGenresResponseDto>(filmGenre);
        }

        public async Task<FilmGenresResponseDto> UpdateAsync(Guid id, FilmGenreRequestDto filmGenreDto)
        {
            if (filmGenreDto == null)
                throw new ArgumentNullException(nameof(filmGenreDto));

            var filmGenre = await _unitOfWork.FilmGenreRepository.GetAsync(
                fg => fg.Id == id && !fg.IsDeleted,
                includeProperties: "Film,Genre");
            if (filmGenre == null)
                throw new Exception("Film genre not found or has been deleted.");

            _mapper.Map(filmGenreDto, filmGenre);
            filmGenre.UpdatedAt = DateTime.Now;

            await _unitOfWork.FilmGenreRepository.UpdateAsync(filmGenre);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<FilmGenresResponseDto>(filmGenre);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filmGenre = await _unitOfWork.FilmGenreRepository.GetAsync(
                fg => fg.Id == id && !fg.IsDeleted,
                includeProperties: "Film,Genre");
            if (filmGenre == null)
                throw new Exception("Film genre not found or has been deleted.");

            filmGenre.IsDeleted = true;
            filmGenre.DeletedAt = DateTime.Now;
            filmGenre.UpdatedAt = DateTime.Now;

            await _unitOfWork.FilmGenreRepository.UpdateAsync(filmGenre);
            await _unitOfWork.SaveAsync();
        }

        public async Task<FilmGenresResponseDto> GetByIdAsync(Guid id)
        {
            var filmGenre = await _unitOfWork.FilmGenreRepository.GetAsync(
                fg => fg.Id == id && !fg.IsDeleted,
                includeProperties: "Film,Genre");
            if (filmGenre == null)
                throw new Exception("Film genre not found or has been deleted.");

            return _mapper.Map<FilmGenresResponseDto>(filmGenre);
        }

        public async Task<PagedDto<FilmGenresResponseDto>> GetPagingAsync(
            int pageNumber,
            int pageSize,
            Guid? filmId = null,
            Guid? genreId = null)
        {
            Expression<Func<FilmGenre, bool>> filter = fg =>
                !fg.IsDeleted &&
                (!filmId.HasValue || fg.FilmId == filmId.Value) &&
                (!genreId.HasValue || fg.GenreId == genreId.Value);

            var filmGenres = await _unitOfWork.FilmGenreRepository.GetPagingAsync(
                filter: filter,
                includeProperties: "Film,Genre",
                orderBy: fg => fg.Id.ToString(),
                pageNumber: pageNumber,
                pageSize: pageSize);

            var totalItems = await _unitOfWork.FilmGenreRepository.CountAsync(filter);
            var filmGenreDtos = _mapper.Map<ICollection<FilmGenresResponseDto>>(filmGenres);
            return new PagedDto<FilmGenresResponseDto>(pageNumber, pageSize, totalItems, filmGenreDtos);
        }

        public async Task<IEnumerable<FilmGenresResponseDto>> GetAllAsync()
        {
            var filmGenres = await _unitOfWork.FilmGenreRepository.GetAllAsync(
                filter: fg => !fg.IsDeleted,
                includeProperties: "Film,Genre");
            return _mapper.Map<IEnumerable<FilmGenresResponseDto>>(filmGenres);
        }

        public async Task<IEnumerable<FilmGenresResponseDto>> GetByFilmIdAsync(Guid filmId)
        {
            if (filmId == Guid.Empty)
                return Enumerable.Empty<FilmGenresResponseDto>();

            var filmGenres = await _unitOfWork.FilmGenreRepository.GetByFilmIdAsync(filmId);
            return _mapper.Map<IEnumerable<FilmGenresResponseDto>>(filmGenres);
        }

        public async Task<IEnumerable<FilmGenresResponseDto>> GetByGenreIdAsync(Guid genreId)
        {
            if (genreId == Guid.Empty)
                return Enumerable.Empty<FilmGenresResponseDto>();

            var filmGenres = await _unitOfWork.FilmGenreRepository.GetByGenreIdAsync(genreId);
            return _mapper.Map<IEnumerable<FilmGenresResponseDto>>(filmGenres);
        }
    }
}