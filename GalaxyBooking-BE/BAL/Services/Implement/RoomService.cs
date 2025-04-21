using AutoMapper;
using BAL.DTOs;
using BAL.Services.Interface;
using DAL.Models;
using DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Services.Implement
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoomResponseDto> CreateAsync(RoomRequestDto roomDto)
        {
            if (roomDto == null)
                throw new ArgumentNullException(nameof(roomDto));

            var room = _mapper.Map<Room>(roomDto);
            room.Id = Guid.NewGuid();
            room.IsDeleted = false;
            room.CreatedAt = DateTime.Now;
            room.UpdatedAt = DateTime.Now;
            room.DeletedAt = null;
            room.Seats = new List<Seat>(); // Khởi tạo danh sách rỗng
            room.Projections = new List<Projection>(); // Khởi tạo danh sách rỗng

            await _unitOfWork.RoomRepository.AddAsync(room);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<RoomResponseDto>(room);
        }

        public async Task<RoomResponseDto> UpdateAsync(Guid id, RoomRequestDto roomDto)
        {
            if (roomDto == null)
                throw new ArgumentNullException(nameof(roomDto));

            var room = await _unitOfWork.RoomRepository.GetAsync(
                r => r.Id == id && !r.IsDeleted,
                includeProperties: "Seats,Projections");
            if (room == null)
                throw new Exception("Room not found or has been deleted");

            _mapper.Map(roomDto, room);
            room.UpdatedAt = DateTime.Now;

            await _unitOfWork.RoomRepository.UpdateAsync(room);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<RoomResponseDto>(room);
        }

        public async Task DeleteAsync(Guid id)
        {
            var room = await _unitOfWork.RoomRepository.GetAsync(
                r => r.Id == id && !r.IsDeleted,
                includeProperties: "Seats,Projections");
            if (room == null)
                throw new Exception("Room not found or has been deleted");

            room.IsDeleted = true;
            room.DeletedAt = DateTime.Now;
            room.UpdatedAt = DateTime.Now;

            await _unitOfWork.RoomRepository.UpdateAsync(room);
            await _unitOfWork.SaveAsync();
        }

        public async Task<RoomResponseDto> GetByIdAsync(Guid id)
        {
            var room = await _unitOfWork.RoomRepository.GetAsync(
                filter: r => r.Id == id && !r.IsDeleted,
                includeProperties: "Seats,Projections");
            if (room == null)
                throw new Exception("Room not found or has been deleted");

            return _mapper.Map<RoomResponseDto>(room);
        }

        public async Task<ICollection<RoomResponseDto>> GetAllAsync()
        {
            var rooms = await _unitOfWork.RoomRepository.GetAllAsync(
                filter: r => !r.IsDeleted,
                includeProperties: "Seats,Projections");
            return _mapper.Map<ICollection<RoomResponseDto>>(rooms);
        }

        public async Task<RoomResponseDto> FindByIdAsync(Guid id)
        {
            var room = await _unitOfWork.RoomRepository.FindByIdAsync(id);
            if (room == null || room.IsDeleted)
                throw new Exception("Room not found or has been deleted");

            return _mapper.Map<RoomResponseDto>(room);
        }

        public async Task<RoomResponseDto> FindByRoomNumberAsync(string roomNumber)
        {
            if (string.IsNullOrWhiteSpace(roomNumber))
                throw new ArgumentException("Room number cannot be empty", nameof(roomNumber));

            var room = await _unitOfWork.RoomRepository.FindByRoomNumberAsync(roomNumber);
            if (room == null || room.IsDeleted)
                throw new Exception("Room not found or has been deleted");

            return _mapper.Map<RoomResponseDto>(room);
        }

        public async Task<ICollection<RoomResponseDto>> FindByRoomTypeAsync(RoomType roomType)
        {
            var rooms = await _unitOfWork.RoomRepository.FindByRoomTypeAsync(roomType);
            var activeRooms = rooms.Where(r => !r.IsDeleted).ToList();
            return _mapper.Map<ICollection<RoomResponseDto>>(activeRooms);
        }
    }
}