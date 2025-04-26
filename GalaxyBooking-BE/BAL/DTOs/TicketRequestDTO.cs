namespace BAL.DTOs
{
    public class TicketRequestDTO
    {
        public int SeatAmount { get; set; }
        public List<Guid> SeatIds { get; set; }
        public Guid RoomId { get; set; }
        public Guid ProjectionId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
