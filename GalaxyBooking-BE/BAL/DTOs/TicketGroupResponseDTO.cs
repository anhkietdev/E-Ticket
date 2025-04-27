namespace BAL.DTOs
{
    public class TicketGroupResponseDTO
    {
        public string AppTransId { get; set; }
        public List<TicketDto> Tickets { get; set; }
    }
}
