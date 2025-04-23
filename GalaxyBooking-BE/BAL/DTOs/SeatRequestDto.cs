namespace BAL.DTOs
{
    public class SeatRequestDto
    {
        public required string SeatNumber { get; set; }

        public required string Row { get; set; }

        public Guid RoomId { get; set; }
    }
}
