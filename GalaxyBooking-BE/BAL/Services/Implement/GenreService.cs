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
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GenreResponseDto> CreateAsync(GenreRequestDto genreDto)
        {
            if (genreDto == null)
                throw new ArgumentNullException(nameof(genreDto));

            // Kiểm tra Name duy nhất
            var existingGenre = await _unitOfWork.GenreRepository.GetAsync(
                g => g.Name == genreDto.Name && !g.IsDeleted);
            if (existingGenre != null)
                throw new Exception("Genre with this name already exists");

            var genre = _mapper.Map<Genre>(genreDto);
            genre.Id = Guid.NewGuid();
            genre.CreatedBy = genreDto.CreatedBy;
            genre.IsDeleted = false;
            genre.CreatedAt = DateTime.Now;
            genre.UpdatedAt = DateTime.Now;
            genre.DeletedAt = null;

            await _unitOfWork.GenreRepository.AddAsync(genre);
            await _unitOfWork.SaveAsync();

            // Tải lại Genre với FilmGenres
            var savedGenre = await _unitOfWork.GenreRepository.GetAsync(
                g => g.Id == genre.Id && !g.IsDeleted,
                includeProperties: "FilmGenres");

            return _mapper.Map<GenreResponseDto>(savedGenre);
        }

        public async Task<GenreResponseDto> UpdateAsync(Guid id, GenreRequestDto genreDto)
        {
            var genre = await _unitOfWork.GenreRepository.GetAsync(
                g => g.Id == id && !g.IsDeleted,
                includeProperties: "FilmGenres");
            if (genre == null)
                throw new Exception("Genre not found or has been deleted");

            // Kiểm tra Name duy nhất (ngoại trừ genre hiện tại)
            var existingGenre = await _unitOfWork.GenreRepository.GetAsync(
                g => g.Name == genreDto.Name && g.Id != id && !g.IsDeleted);
            if (existingGenre != null)
                throw new Exception("Genre with this name already exists");

            _mapper.Map(genreDto, genre);
            genre.UpdatedAt = DateTime.Now;
            genre.UpdatedBy = genreDto.UpdatedBy;

            await _unitOfWork.GenreRepository.UpdateAsync(genre);
            await _unitOfWork.SaveAsync();

            // Tải lại Genre với FilmGenres
            var updatedGenre = await _unitOfWork.GenreRepository.GetAsync(
                g => g.Id == id && !g.IsDeleted,
                includeProperties: "FilmGenres");

            return _mapper.Map<GenreResponseDto>(updatedGenre);
        }

        public async Task DeleteAsync(Guid id)
        {
            var genre = await _unitOfWork.GenreRepository.GetAsync(
                g => g.Id == id && !g.IsDeleted);
            if (genre == null)
                throw new Exception("Genre not found or has been deleted");

            genre.IsDeleted = true;
            genre.DeletedAt = DateTime.Now;
            genre.UpdatedAt = DateTime.Now;

            await _unitOfWork.GenreRepository.UpdateAsync(genre);
            await _unitOfWork.SaveAsync();
        }

        public async Task<GenreResponseDto> GetByIdAsync(Guid id)
        {
            var genre = await _unitOfWork.GenreRepository.GetAsync(
                filter: g => g.Id == id && !g.IsDeleted,
                includeProperties: "FilmGenres");
            if (genre == null)
                throw new Exception("Genre not found or has been deleted");

            return _mapper.Map<GenreResponseDto>(genre);
        }

        public async Task<PagedDto<GenreResponseDto>> GetPagingAsync(
            int pageNumber,
            int pageSize,
            string? name = null)
        {
            Expression<Func<Genre, bool>> filter = g =>
                !g.IsDeleted &&
                (string.IsNullOrEmpty(name) || g.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            var genres = await _unitOfWork.GenreRepository.GetPagingAsync(
                filter: filter,
                includeProperties: "FilmGenres",
                orderBy: g => g.Name,
                pageNumber: pageNumber,
                pageSize: pageSize);

            var totalItems = await _unitOfWork.GenreRepository.CountAsync(filter);
            var genreDtos = _mapper.Map<ICollection<GenreResponseDto>>(genres);
            return new PagedDto<GenreResponseDto>(pageNumber, pageSize, totalItems, genreDtos);
        }

        public async Task<IEnumerable<GenreResponseDto>> GetAllAsync()
        {
            var genres = await _unitOfWork.GenreRepository.GetAllAsync(
                filter: g => !g.IsDeleted,
                includeProperties: "FilmGenres");
            return _mapper.Map<IEnumerable<GenreResponseDto>>(genres);
        }

        public async Task<GenreResponseDto> FindByIdAsync(Guid id)
        {
            var genre = await _unitOfWork.GenreRepository.FindByIdAsync(id);
            if (genre == null)
                throw new Exception("Genre not found or has been deleted");

            return _mapper.Map<GenreResponseDto>(genre);
        }
    }
}