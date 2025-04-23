using BAL.DTOs;
using BAL.Services.Interface;
using DAL.Models;
using DAL.Repository.Interface;

namespace BAL.Services.Implement
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IZaloPayService _zaloPayService;
        public TicketService(IUnitOfWork unitOfWork, IZaloPayService zaloPayService)
        {
            _unitOfWork = unitOfWork;
            _zaloPayService = zaloPayService;
        }
        public async Task<TicketResponseDTO> CreateTicket(TicketRequestDTO request)
        {
            var projection = await _unitOfWork.ProjectionRepository.GetAsync(p => p.Id == request.ProjectionId) ?? throw new ArgumentNullException("Not found projection");
            var room = await _unitOfWork.RoomRepository.GetAsync(r => r.Id == request.RoomId, "Seats, Projections") ?? throw new ArgumentNullException("Not found room");

            if (request.SeatAmount > room.Capacity)
            {
                throw new ArgumentNullException("Seat amount > room capacity");
            }

            var isExistSeat = await _unitOfWork.TicketRepository.AnyAsync(s => request.SeatIds.Contains(s.SeatId));

            if (isExistSeat)
            {
                throw new ArgumentNullException("One or more seats is not enable ");
            }

            List<Ticket> ticketLst = new List<Ticket>();
            List<Seat> seatLst = new List<Seat>();
            decimal totalPrice = 0;
            foreach (var item in request.SeatIds)
            {
                var seat = await _unitOfWork.SeatRepository.GetAsync(s => s.Id == item);
                seat.IsEnable = false;
                seatLst.Add(seat);

                var ticket = new Ticket
                {
                    Id = Guid.NewGuid(),
                    Price = projection.Price,
                    PurchaseTime = DateTime.Now,
                    ProjectionId = request.ProjectionId,
                    SeatId = item,
                    UserId = request.UserId,
                    IsPaymentSuccess = false,
                };

                ticketLst.Add(ticket);
                totalPrice += ticket.Price;
            }

            await _unitOfWork.TicketRepository.AddRange(ticketLst);
            await _unitOfWork.SeatRepository.UpdateRange(seatLst);
            await _unitOfWork.SaveAsync();

            var requestZaloPay = new PaymentDTO
            {
                UserId = request.UserId,
                PaymentContent = $"Purchase ticket {DateTime.UtcNow}",
                PaymentCurrency = "VND",
                PaymentRefId = $"Cine-{Guid.NewGuid()}",
                RequiredAmount = totalPrice,
                BankCode = "zalopayapp",
                TicketIds = ticketLst.Select(x => x.Id).ToList()
            };

            (bool returnStatus, string message) = await _zaloPayService.CreateZalopayPayment(requestZaloPay);

            return new TicketResponseDTO
            {
                ProjectionId = projection.Id,
                RoomId = room.Id,
                SeatAmount = request.SeatAmount,
                SeatIds = request.SeatIds,
                RedirectUrl = returnStatus ? message : string.Empty,
                TotalPrice = totalPrice,
            };
        }
    }
}
