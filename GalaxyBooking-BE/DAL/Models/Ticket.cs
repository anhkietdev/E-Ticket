namespace DAL.Models
{
    public class Ticket : BaseEntity
    {
        public DateTime PurchaseTime { get; set; }

        public decimal Price { get; set; }

        public int ProjectionId { get; set; }

        public virtual Projection Projection { get; set; }

        public int SeatId { get; set; }

        public virtual Seat Seat { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
