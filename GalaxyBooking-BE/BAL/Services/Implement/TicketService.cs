using BAL.DTOs;
using BAL.Services.Interface;
using DAL.Models;
using DAL.Repository.Interface;

namespace BAL.Services.Implement
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<TicketResponseDTO> CreateTicket(TicketRequestDTO request)
        {
            var projection = await _unitOfWork.ProjectionRepository.GetAsync(p => p.Id == request.ProjectionId) ?? throw new ArgumentNullException("Not found room");
            var room = await _unitOfWork.RoomRepository.GetAsync(r => r.Id == request.RoomId, "Seats, Projections") ?? throw new ArgumentNullException("Not found room");

            if (request.SeatAmount > room.Capacity)
            {
                throw new ArgumentNullException("Seat amount > room capacity");
            }

            var isExistSeat = request.SeatIds.All(id => room.Seats.Select(s => s.Id).ToList().Contains(id));

            if (!isExistSeat)
            {
                throw new ArgumentNullException("One or more seats do not exist in the room");
            }

            List<Ticket> ticketLst = new List<Ticket>();
            foreach (var item in request.SeatIds)
            {
                var ticket = new Ticket
                {
                    Price = request.Price,
                    PurchaseTime = DateTime.Now,
                    ProjectionId = request.ProjectionId,
                    SeatId = item,
                    UserId = request.UserId,
                };

                ticketLst.Add(ticket);
            }

            await _unitOfWork.TicketRepository.AddRange(ticketLst);
            await _unitOfWork.SaveAsync();

            return new TicketResponseDTO
            {
                ProjectionId = projection.Id,
                RoomId = room.Id,
                SeatAmount = request.SeatAmount,
                SeatIds = request.SeatIds,
            };
        }
    }
}
