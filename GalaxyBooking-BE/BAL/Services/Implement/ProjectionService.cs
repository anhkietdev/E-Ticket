using AutoMapper;
using BAL.DTOs;
using BAL.Services.Interface;
using DAL.Models;
using DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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

        public async Task AddAsync(ProjectionDto projectionDto)
        {
            var projection = new Projection
            {
                StartTime = projectionDto.StartTime,
                EndTime = projectionDto.EndTime,
                Price = projectionDto.Price,
                FilmId = projectionDto.FilmId,
                RoomId = projectionDto.RoomId
            };
            await _unitOfWork.ProjectionRepository.AddAsync(projection);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var projection = await _unitOfWork.ProjectionRepository.GetAsync(p => p.Id == id && !p.IsDeleted, includeProperties: "Film,Room");
            if (projection == null)
            {
                throw new Exception("Projection not found or has been deleted");
            }
            await _unitOfWork.ProjectionRepository.RemoveAsync(projection);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProjectionDto>> GetAllAsync()
        {
            var projections = await _unitOfWork.ProjectionRepository.GetAllAsync(
                filter: p => !p.IsDeleted);
            return _mapper.Map<IEnumerable<ProjectionDto>>(projections);
        }

        public async Task<ProjectionDto> GetByIdAsync(Guid id)
        {
            var projection = await _unitOfWork.ProjectionRepository.GetAsync(
                filter: p => p.Id == id && !p.IsDeleted,
                includeProperties: "Film,Room");
            if (projection == null)
            {
                throw new Exception("Projection not found or has been deleted");
            }
            return _mapper.Map<ProjectionDto>(projection);
        }

        public async Task<PagedDto<ProjectionDto>> GetPagingAsync(
            int pageNumber,
            int pageSize,
            DateTime? startTime = null,
            Guid? filmId = null,
            Guid? roomId = null)
        {
            Expression<Func<Projection, bool>> filter = p =>
                !p.IsDeleted &&
                (!startTime.HasValue || p.StartTime.Date == startTime.Value.Date) &&
                (!filmId.HasValue || p.FilmId == filmId.Value) &&
                (!roomId.HasValue || p.RoomId == roomId.Value);

            var projections = await _unitOfWork.ProjectionRepository.GetPagingAsync(
                filter: filter,
                includeProperties: "Film,Room",
                orderBy: p => p.StartTime,
                pageNumber: pageNumber,
                pageSize: pageSize);

            var totalItems = await _unitOfWork.ProjectionRepository.CountAsync(filter);
            var projectionDtos = _mapper.Map<ICollection<ProjectionDto>>(projections);
            return new PagedDto<ProjectionDto>(pageNumber, pageSize, totalItems, projectionDtos);
        }

        public async Task UpdateAsync(ProjectionDto projectionDto)
        {
            var projection = await _unitOfWork.ProjectionRepository.GetAsync(
                p => p.Id == projectionDto.Id && !p.IsDeleted);
            if (projection == null)
            {
                throw new Exception("Projection not found or has been deleted");
            }

            projection.StartTime = projectionDto.StartTime;
            projection.EndTime = projectionDto.EndTime;
            projection.Price = projectionDto.Price;
            projection.FilmId = projectionDto.FilmId;
            projection.RoomId = projectionDto.RoomId;

            await _unitOfWork.ProjectionRepository.UpdateAsync(projection);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProjectionDto>> FindByFilmIdAsync(Guid filmId)
        {
            var projections = await _unitOfWork.ProjectionRepository.FindByFilmIdAsync(filmId);
            return _mapper.Map<IEnumerable<ProjectionDto>>(projections);
        }

        public async Task<IEnumerable<ProjectionDto>> FindByRoomIdAsync(Guid roomId)
        {
            var projections = await _unitOfWork.ProjectionRepository.FindByRoomIdAsync(roomId);
            return _mapper.Map<IEnumerable<ProjectionDto>>(projections);
        }
    }
}
