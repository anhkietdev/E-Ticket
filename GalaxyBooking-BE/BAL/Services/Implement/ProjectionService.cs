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
    public class ProjectionService : IProjectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjectionResponseDto> CreateAsync(ProjectionRequestDto projectionDto)
        {
            if (projectionDto == null)
                throw new ArgumentNullException(nameof(projectionDto));

            // Kiểm tra FilmId và RoomId tồn tại
            var film = await _unitOfWork.FilmRepository.GetAsync(f => f.Id == projectionDto.FilmId && !f.IsDeleted);
            if (film == null)
                throw new Exception("Film not found or has been deleted");

            var room = await _unitOfWork.RoomRepository.GetAsync(r => r.Id == projectionDto.RoomId && !r.IsDeleted);
            if (room == null)
                throw new Exception("Room not found or has been deleted");

            var projection = _mapper.Map<Projection>(projectionDto);
            projection.Id = Guid.NewGuid();
            projection.IsDeleted = false;
            projection.CreatedAt = DateTime.Now;
            projection.UpdatedAt = DateTime.Now;
            projection.DeletedAt = null;

            await _unitOfWork.ProjectionRepository.AddAsync(projection);
            await _unitOfWork.SaveAsync();

            // Tải lại Projection với Film, Room và Tickets
            var savedProjection = await _unitOfWork.ProjectionRepository.GetAsync(
                p => p.Id == projection.Id && !p.IsDeleted,
                includeProperties: "Film,Room,Tickets");

            return _mapper.Map<ProjectionResponseDto>(savedProjection);
        }

        public async Task<ProjectionResponseDto> UpdateAsync(Guid id, ProjectionRequestDto projectionDto)
        {
            var projection = await _unitOfWork.ProjectionRepository.GetAsync(
                p => p.Id == id && !p.IsDeleted,
                includeProperties: "Film,Room,Tickets");
            if (projection == null)
                throw new Exception("Projection not found or has been deleted");

            // Kiểm tra FilmId và RoomId tồn tại
            var film = await _unitOfWork.FilmRepository.GetAsync(f => f.Id == projectionDto.FilmId && !f.IsDeleted);
            if (film == null)
                throw new Exception("Film not found or has been deleted");

            var room = await _unitOfWork.RoomRepository.GetAsync(r => r.Id == projectionDto.RoomId && !r.IsDeleted);
            if (room == null)
                throw new Exception("Room not found or has been deleted");

            _mapper.Map(projectionDto, projection);
            projection.UpdatedAt = DateTime.Now;

            await _unitOfWork.ProjectionRepository.UpdateAsync(projection);
            await _unitOfWork.SaveAsync();

            // Tải lại Projection với Film, Room và Tickets
            var updatedProjection = await _unitOfWork.ProjectionRepository.GetAsync(
                p => p.Id == id && !p.IsDeleted,
                includeProperties: "Film,Room,Tickets");

            return _mapper.Map<ProjectionResponseDto>(updatedProjection);
        }

        public async Task DeleteAsync(Guid id)
        {
            var projection = await _unitOfWork.ProjectionRepository.GetAsync(
                p => p.Id == id && !p.IsDeleted);
            if (projection == null)
                throw new Exception("Projection not found or has been deleted");

            projection.IsDeleted = true;
            projection.DeletedAt = DateTime.Now;
            projection.UpdatedAt = DateTime.Now;

            await _unitOfWork.ProjectionRepository.UpdateAsync(projection);
            await _unitOfWork.SaveAsync();
        }

        public async Task<ProjectionResponseDto> GetByIdAsync(Guid id)
        {
            var projection = await _unitOfWork.ProjectionRepository.GetAsync(
                filter: p => p.Id == id && !p.IsDeleted,
                includeProperties: "Film,Room,Tickets");
            if (projection == null)
                throw new Exception("Projection not found or has been deleted");

            return _mapper.Map<ProjectionResponseDto>(projection);
        }

        public async Task<PagedDto<ProjectionResponseDto>> GetPagingAsync(
            int pageNumber,
            int pageSize,
            Guid? filmId = null,
            Guid? roomId = null,
            DateTime? startTime = null)
        {
            Expression<Func<Projection, bool>> filter = p =>
                !p.IsDeleted &&
                (!filmId.HasValue || p.FilmId == filmId.Value) &&
                (!roomId.HasValue || p.RoomId == roomId.Value) &&
                (!startTime.HasValue || p.StartTime.Date == startTime.Value.Date);

            var projections = await _unitOfWork.ProjectionRepository.GetPagingAsync(
                filter: filter,
                includeProperties: "Film,Room,Tickets",
                orderBy: p => p.StartTime,
                pageNumber: pageNumber,
                pageSize: pageSize);

            var totalItems = await _unitOfWork.ProjectionRepository.CountAsync(filter);
            var projectionDtos = _mapper.Map<ICollection<ProjectionResponseDto>>(projections);
            return new PagedDto<ProjectionResponseDto>(pageNumber, pageSize, totalItems, projectionDtos);
        }

        public async Task<IEnumerable<ProjectionResponseDto>> GetAllAsync()
        {
            var projections = await _unitOfWork.ProjectionRepository.GetAllAsync(
                filter: p => !p.IsDeleted,
                includeProperties: "Film,Room,Tickets");
            return _mapper.Map<IEnumerable<ProjectionResponseDto>>(projections);
        }

        public async Task<IEnumerable<ProjectionResponseDto>> FindByFilmIdAsync(Guid filmId)
        {
            var projections = await _unitOfWork.ProjectionRepository.FindByFilmIdAsync(filmId);
            return _mapper.Map<IEnumerable<ProjectionResponseDto>>(projections);
        }

        public async Task<IEnumerable<ProjectionResponseDto>> FindByRoomIdAsync(Guid roomId)
        {
            var projections = await _unitOfWork.ProjectionRepository.FindByRoomIdAsync(roomId);
            return _mapper.Map<IEnumerable<ProjectionResponseDto>>(projections);
        }
    }
}