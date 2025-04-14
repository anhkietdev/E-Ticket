namespace DAL.Models
{
    public class Seat : BaseEntity
    {
        public required string SeatNumber { get; set; }

        public required string Row { get; set; }

        public int RoomId { get; set; }

        public virtual Room Room { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
