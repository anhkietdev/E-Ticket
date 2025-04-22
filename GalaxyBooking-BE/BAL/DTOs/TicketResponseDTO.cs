namespace BAL.DTOs
{
    public class TicketResponseDTO
    {
        public int SeatAmount { get; set; }
        public List<Guid> SeatIds { get; set; }
        public Guid RoomId { get; set; }
        public Guid ProjectionId { get; set; }
    }
}
