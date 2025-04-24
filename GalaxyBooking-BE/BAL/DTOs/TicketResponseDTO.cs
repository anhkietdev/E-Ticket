namespace BAL.DTOs
{
    public class TicketResponseDTO
    {
        public int SeatAmount { get; set; }
        public List<SeatDto> SeatIds { get; set; }
        public Guid RoomId { get; set; }
        public Guid ProjectionId { get; set; }
        public string RedirectUrl { get; set; }
        public decimal TotalPrice { get; set; }
        public string RoomNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid FilmId { get; set; }
        public string FIlmName { get; set; }
        public RoomDto Room { get; set; }
        public FilmDto Film { get; set; }

    }
}
