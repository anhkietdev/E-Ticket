namespace BAL.DTOs
{
    public class SeatRequestDto
    {
        public required string SeatNumber { get; set; }

        public required string Row { get; set; }
        public bool IsEnable { get; set; } = true;
        public Guid RoomId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
