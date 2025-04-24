using BAL.DTOs;

namespace BAL.Services.Interface
{
    public interface ITicketService
    {
        Task<TicketResponseDTO> CreateTicket(TicketRequestDTO request);
    }
}
