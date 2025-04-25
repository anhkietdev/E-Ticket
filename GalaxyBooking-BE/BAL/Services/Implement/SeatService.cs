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
    public class SeatService : ISeatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SeatService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SeatResponseDto> CreateAsync(SeatRequestDto seatDto)
        {
            if (seatDto == null)
                throw new ArgumentNullException(nameof(seatDto));

            // Kiểm tra RoomId tồn tại
            var room = await _unitOfWork.RoomRepository.GetAsync(r => r.Id == seatDto.RoomId && !r.IsDeleted);
            if (room == null)
                throw new Exception("Room not found or has been deleted");

            // Kiểm tra SeatNumber và Row duy nhất trong Room
            var existingSeat = await _unitOfWork.SeatRepository.GetAsync(
                s => s.RoomId == seatDto.RoomId && s.SeatNumber == seatDto.SeatNumber && s.Row == seatDto.Row && !s.IsDeleted);
            if (existingSeat != null)
                throw new Exception("Seat with the same SeatNumber and Row already exists in this Room");

            var seat = _mapper.Map<Seat>(seatDto);
            seat.Id = Guid.NewGuid();
            seat.CreatedBy = seatDto.CreatedBy;
            seat.IsDeleted = false;
            seat.CreatedAt = DateTime.Now;
            seat.UpdatedAt = DateTime.Now;
            seat.DeletedAt = null;

            await _unitOfWork.SeatRepository.AddAsync(seat);
            await _unitOfWork.SaveAsync();

            // Tải lại Seat với Room và Tickets
            var savedSeat = await _unitOfWork.SeatRepository.GetAsync(
                s => s.Id == seat.Id && !s.IsDeleted,
                includeProperties: "Room,Tickets");

            return _mapper.Map<SeatResponseDto>(savedSeat);
        }

        public async Task<SeatResponseDto> UpdateAsync(Guid id, SeatRequestDto seatDto)
        {
            var seat = await _unitOfWork.SeatRepository.GetAsync(
                s => s.Id == id && !s.IsDeleted,
                includeProperties: "Room,Tickets");
            if (seat == null)
                throw new Exception("Seat not found or has been deleted");

            // Kiểm tra RoomId tồn tại
            var room = await _unitOfWork.RoomRepository.GetAsync(r => r.Id == seatDto.RoomId && !r.IsDeleted);
            if (room == null)
                throw new Exception("Room not found or has been deleted");

            // Kiểm tra SeatNumber và Row duy nhất trong Room (ngoại trừ seat hiện tại)
            var existingSeat = await _unitOfWork.SeatRepository.GetAsync(
                s => s.RoomId == seatDto.RoomId && s.SeatNumber == seatDto.SeatNumber &&
                     s.Row == seatDto.Row && s.Id != id && !s.IsDeleted);
            if (existingSeat != null)
                throw new Exception("Seat with the same SeatNumber and Row already exists in this Room");

            _mapper.Map(seatDto, seat);
            seat.UpdatedAt = DateTime.Now;
            seat.UpdatedBy = seatDto.UpdatedBy;

            await _unitOfWork.SeatRepository.UpdateAsync(seat);
            await _unitOfWork.SaveAsync();

            // Tải lại Seat với Room và Tickets
            var updatedSeat = await _unitOfWork.SeatRepository.GetAsync(
                s => s.Id == id && !s.IsDeleted,
                includeProperties: "Room,Tickets");

            return _mapper.Map<SeatResponseDto>(updatedSeat);
        }

        public async Task DeleteAsync(Guid id)
        {
            var seat = await _unitOfWork.SeatRepository.GetAsync(
                s => s.Id == id && !s.IsDeleted);
            if (seat == null)
                throw new Exception("Seat not found or has been deleted");

            seat.IsDeleted = true;
            seat.DeletedAt = DateTime.Now;
            seat.UpdatedAt = DateTime.Now;

            await _unitOfWork.SeatRepository.UpdateAsync(seat);
            await _unitOfWork.SaveAsync();
        }

        public async Task<SeatResponseDto> GetByIdAsync(Guid id)
        {
            var seat = await _unitOfWork.SeatRepository.GetAsync(
                filter: s => s.Id == id && !s.IsDeleted,
                includeProperties: "Room,Tickets");
            if (seat == null)
                throw new Exception("Seat not found or has been deleted");

            return _mapper.Map<SeatResponseDto>(seat);
        }

        public async Task<PagedDto<SeatResponseDto>> GetPagingAsync(
            int pageNumber,
            int pageSize,
            Guid? roomId = null,
            string? seatNumber = null,
            string? row = null)
        {
            Expression<Func<Seat, bool>> filter = s =>
                !s.IsDeleted &&
                (!roomId.HasValue || s.RoomId == roomId.Value) &&
                (string.IsNullOrEmpty(seatNumber) || s.SeatNumber.Contains(seatNumber, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(row) || s.Row.Contains(row, StringComparison.OrdinalIgnoreCase));

            var seats = await _unitOfWork.SeatRepository.GetPagingAsync(
                filter: filter,
                includeProperties: "Room,Tickets",
                orderBy: s => s.SeatNumber,
                pageNumber: pageNumber,
                pageSize: pageSize);

            var totalItems = await _unitOfWork.SeatRepository.CountAsync(filter);
            var seatDtos = _mapper.Map<ICollection<SeatResponseDto>>(seats);
            return new PagedDto<SeatResponseDto>(pageNumber, pageSize, totalItems, seatDtos);
        }

        public async Task<IEnumerable<SeatResponseDto>> GetAllAsync()
        {
            var seats = await _unitOfWork.SeatRepository.GetAllAsync(
                filter: s => !s.IsDeleted,
                includeProperties: "Room,Tickets");
            return _mapper.Map<IEnumerable<SeatResponseDto>>(seats);
        }

        public async Task<SeatResponseDto> FindByIdAsync(Guid id)
        {
            var seat = await _unitOfWork.SeatRepository.FindByIdAsync(id);
            if (seat == null)
                throw new Exception("Seat not found or has been deleted");

            return _mapper.Map<SeatResponseDto>(seat);
        }
    }
}