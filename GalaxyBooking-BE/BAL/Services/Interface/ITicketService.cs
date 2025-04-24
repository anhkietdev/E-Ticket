using BAL.DTOs;

namespace BAL.Services.Interface
{
    public interface ITicketService
    {
        Task<TicketResponseDTO> CreateTicket(TicketRequestDTO request);
        Task<PagedDto<TicketDto>> GetTicketByUserId(Guid UserId, int pageNumber,
            int pageSize);
        Task<TicketDto> GetTicketById(Guid ticketId);
    }
}
