using AutoMapper;
using BAL.DTOs;
using BAL.Services.Interface;
using BAL.Services.ZaloPay.Request;
using DAL.Common;
using DAL.Models;
using DAL.Repository.Interface;

namespace BAL.Services.Implement
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IZaloPayService _zaloPayService;
        private readonly IMapper _mapper;
        public TicketService(IUnitOfWork unitOfWork, IZaloPayService zaloPayService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _zaloPayService = zaloPayService;
            _mapper = mapper;
        }
        public async Task<TicketResponseDTO> CreateTicket(Guid userId, TicketRequestDTO request)
        {
            var projection = await _unitOfWork.ProjectionRepository.GetAsync(p => p.Id == request.ProjectionId) ?? throw new ArgumentNullException("Not found projection");
            var film = await _unitOfWork.FilmRepository.GetAsync(p => p.Id == projection.FilmId) ?? throw new ArgumentNullException("Not found film");
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
            foreach (var item in request.SeatIds)
            {
                var seat = await _unitOfWork.SeatRepository.GetAsync(s => s.Id == item) ?? throw new ArgumentNullException("Not found seat");
                if (!seat.IsEnable)
                {
                    throw new Exception("One or more seats is disabled");
                }
                seat.IsEnable = false;
                seatLst.Add(seat);

                var ticket = new Ticket
                {
                    Id = Guid.NewGuid(),
                    Price = seat.IsVip ? (projection.Price + (projection.Price * (decimal)0.2)) : projection.Price,
                    PurchaseTime = DateTime.Now,
                    ProjectionId = request.ProjectionId,
                    SeatId = item,
                    UserId = userId,
                    IsPaymentSuccess = false,
                    CreatedBy = request.CreatedBy,
                };

                ticketLst.Add(ticket);
            }

            List<Item> itemLst = new List<Item>();

            foreach (var ticket in ticketLst)
            {
                Item item = new Item();
                item.ItemId = ticket.Id;
                itemLst.Add(item);
            }

            var requestZaloPay = new PaymentDTO
            {
                UserId = userId,
                PaymentContent = $"Purchase ticket {DateTime.UtcNow}",
                PaymentCurrency = "VND",
                PaymentRefId = $"Cine-{Guid.NewGuid()}",
                RequiredAmount = request.TotalPrice,
                BankCode = "zalopayapp",
                TicketIds = ticketLst.Select(x => x.Id).ToList(),
                ProjectionId = request.ProjectionId,
                Items = itemLst
            };

            (bool returnStatus, string message) = await _zaloPayService.CreateZalopayPayment(requestZaloPay);

            foreach (var item in ticketLst)
            {
                item.AppTransId = GlobalCache.AppTransIdCache;
            }

            await _unitOfWork.TicketRepository.AddRange(ticketLst);
            await _unitOfWork.SeatRepository.UpdateRange(seatLst);
            await _unitOfWork.SaveAsync();

            return new TicketResponseDTO
            {
                ProjectionId = projection.Id,
                FilmId = film.Id,
                FIlmName = film.Title,
                StartTime = projection.StartTime,
                EndTime = projection.EndTime,
                RoomNumber = room.RoomNumber,
                RoomId = room.Id,
                SeatAmount = request.SeatAmount,
                SeatIds = _mapper.Map<List<SeatDto>>(seatLst),
                RedirectUrl = returnStatus ? message : string.Empty,
                TotalPrice = request.TotalPrice,
                AppTransId = GlobalCache.AppTransIdCache,
            };
        }

        public async Task<bool> DeleteTicketByAppTransId(Guid apptransId)
        {
            var ticketLst = await _unitOfWork.TicketRepository.GetAllAsync(e => e.AppTransId == GlobalCache.AppTransIdCache);

            foreach (var item in ticketLst)
            {
                item.IsDeleted = true;
            }

            await _unitOfWork.TicketRepository.UpdateRange(ticketLst);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> DeleteTicketById(Guid ticketId)
        {
            var ticket = await _unitOfWork.TicketRepository.GetAsync(e => e.Id == ticketId) ?? throw new ArgumentNullException($"Ticket {nameof(ticketId)} not found");
            ticket.IsDeleted = true;
            await _unitOfWork.TicketRepository.UpdateAsync(ticket);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<TicketDto> GetTicketById(Guid ticketId)
        {
            var ticket = await _unitOfWork.TicketRepository.GetAsync(e => e.Id == ticketId
            , "Projection,Projection.Room,Projection.Film,Seat") ?? throw new ArgumentNullException("Ticket not found");
            var ticketDto = new TicketDto
            {
                Id = ticket.Id,
                PurchaseTime = ticket.PurchaseTime,
                ProjectionId = ticket.ProjectionId,
                SeatId = ticket.SeatId,
                UserId = ticket.UserId,
                SeatNumber = ticket.Seat?.SeatNumber,
                RoomNumber = ticket.Projection?.Room?.RoomNumber,
                StartTime = ticket.Projection?.StartTime ?? DateTime.MinValue,
                EndTime = ticket.Projection?.EndTime ?? DateTime.MinValue,
                FilmId = ticket.Projection.FilmId,
                Title = ticket.Projection.Film.Title,
            };

            return ticketDto;
        }

        public async Task<PagedDto<TicketGroupResponseDTO>> GetTicketByUserId(Guid UserId, int pageNumber,
            int pageSize)
        {
            var allTickets = await _unitOfWork.TicketRepository.GetPagingAsync(e => e.UserId == UserId
            , "Projection,Projection.Room,Projection.Film,Seat"
            , orderBy: e => e.CreatedAt
            , pageNumber: pageNumber
            , pageSize: pageSize);

            var grouped = allTickets
             .GroupBy(t => t.AppTransId)
             .Select(group => new TicketGroupResponseDTO
             {
                 AppTransId = group.Key,
                 Tickets = group.Select(ticket => new TicketDto
                 {
                     Id = ticket.Id,
                     PurchaseTime = ticket.PurchaseTime,
                     ProjectionId = ticket.ProjectionId,
                     SeatId = ticket.SeatId,
                     UserId = ticket.UserId,
                     SeatNumber = ticket.Seat?.SeatNumber,
                     RoomNumber = ticket.Projection?.Room?.RoomNumber,
                     StartTime = ticket.Projection?.StartTime ?? DateTime.MinValue,
                     EndTime = ticket.Projection?.EndTime ?? DateTime.MinValue,
                     FilmId = ticket.Projection.FilmId,
                     Title = ticket.Projection.Film.Title,
                 }).ToList()
             }).ToList();

            // Paging the grouped result
            var totalItems = grouped.Count;
            var pagedGrouped = grouped
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedDto<TicketGroupResponseDTO>(pageNumber, pageSize, totalItems, pagedGrouped);
        }

        public async Task<PagedDto<TicketGroupResponseDTO>> GetTickets(int pageNumber, int pageSize)
        {
            var allTickets = await _unitOfWork.TicketRepository.GetPagingAsync(
                filter: null
           , "Projection,Projection.Room,Projection.Film,Seat"
           , orderBy: e => e.CreatedAt
           , pageNumber: pageNumber
           , pageSize: pageSize);

            var grouped = allTickets
             .GroupBy(t => t.AppTransId)
             .Select(group => new TicketGroupResponseDTO
             {
                 AppTransId = group.Key,
                 Tickets = group.Select(ticket => new TicketDto
                 {
                     Id = ticket.Id,
                     PurchaseTime = ticket.PurchaseTime,
                     ProjectionId = ticket.ProjectionId,
                     SeatId = ticket.SeatId,
                     UserId = ticket.UserId,
                     SeatNumber = ticket.Seat?.SeatNumber,
                     Row = ticket.Seat.Row,
                     SeatName = ticket.Seat?.SeatNumber + ticket.Seat.Row,
                     RoomNumber = ticket.Projection?.Room?.RoomNumber,
                     StartTime = ticket.Projection?.StartTime ?? DateTime.MinValue,
                     EndTime = ticket.Projection?.EndTime ?? DateTime.MinValue,
                     FilmId = ticket.Projection.FilmId,
                     Title = ticket.Projection.Film.Title,
                 }).ToList()
             }).ToList();

            // Paging the grouped result
            var totalItems = grouped.Count;
            var pagedGrouped = grouped
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedDto<TicketGroupResponseDTO>(pageNumber, pageSize, totalItems, pagedGrouped);
        }

        public async Task<List<TicketGroupResponseDTO>> GetTicketsByCurrentAppTransId()
        {
            var allTickets = await _unitOfWork.TicketRepository.GetAllAsync(e => e.AppTransId == GlobalCache.AppTransIdCache
         , "Projection,Projection.Room,Projection.Film,Seat"
         , orderBy: e => e.CreatedAt);

            var grouped = allTickets
             .GroupBy(t => t.AppTransId)
             .Select(group => new TicketGroupResponseDTO
             {
                 AppTransId = group.Key,
                 Tickets = group.Select(ticket => new TicketDto
                 {
                     Id = ticket.Id,
                     PurchaseTime = ticket.PurchaseTime,
                     ProjectionId = ticket.ProjectionId,
                     SeatId = ticket.SeatId,
                     UserId = ticket.UserId,
                     SeatNumber = ticket.Seat?.SeatNumber,
                     Row = ticket.Seat.Row,
                     SeatName = ticket.Seat?.SeatNumber + ticket.Seat.Row,
                     RoomNumber = ticket.Projection?.Room?.RoomNumber,
                     StartTime = ticket.Projection?.StartTime ?? DateTime.MinValue,
                     EndTime = ticket.Projection?.EndTime ?? DateTime.MinValue,
                     FilmId = ticket.Projection.FilmId,
                     Title = ticket.Projection.Film.Title,
                 }).ToList()
             }).ToList();

            return grouped;
        }

        public async Task<List<TicketDto>> UpdatePaymentByAppTransId()
        {
            var ticketLst = await _unitOfWork.TicketRepository.GetAllAsync(e => e.AppTransId == GlobalCache.AppTransIdCache);

            foreach (var item in ticketLst)
            {
                item.IsPaymentSuccess = true;
            }

            await _unitOfWork.TicketRepository.UpdateRange(ticketLst);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<List<TicketDto>>(ticketLst);
        }
    }
}
