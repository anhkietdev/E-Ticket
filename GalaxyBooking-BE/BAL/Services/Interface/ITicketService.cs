using BAL.DTOs;

namespace BAL.Services.Interface
{
    public interface ITicketService
    {
        Task<TicketResponseDTO> CreateTicket(Guid userId, TicketRequestDTO request);
        Task<PagedDto<TicketGroupResponseDTO>> GetTicketByUserId(Guid UserId, int pageNumber,
            int pageSize);
        Task<PagedDto<TicketGroupResponseDTO>> GetTickets(int pageNumber,
            int pageSize);
        Task<List<TicketGroupResponseDTO>> GetTicketsByCurrentAppTransId();
        Task<TicketDto> GetTicketById(Guid ticketId);
        Task<List<TicketDto>> UpdatePaymentByAppTransId();
        Task<bool> DeleteTicketById(Guid ticketId);
        Task<bool> DeleteTicketByAppTransId(Guid apptransId);
    }
}
