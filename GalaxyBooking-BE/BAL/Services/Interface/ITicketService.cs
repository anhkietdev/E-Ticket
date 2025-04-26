using BAL.DTOs;

namespace BAL.Services.Interface
{
    public interface ITicketService
    {
        Task<TicketResponseDTO> CreateTicket(Guid userId, TicketRequestDTO request);
        Task<PagedDto<TicketDto>> GetTicketByUserId(Guid UserId, int pageNumber,
            int pageSize);
        Task<PagedDto<TicketDto>> GetTickets(int pageNumber,
            int pageSize);
        Task<TicketDto> GetTicketById(Guid ticketId);
        Task<List<TicketDto>> UpdatePaymentByAppTransId();
        Task<bool> DeleteTicketById(Guid ticketId);
    }
}
