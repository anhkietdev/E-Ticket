namespace DAL.Models
{
    public class Ticket : BaseEntity
    {
        public DateTime PurchaseTime { get; set; }

        public decimal Price { get; set; }

        public Guid ProjectionId { get; set; }

        public virtual Projection Projection { get; set; }

        public Guid SeatId { get; set; }

        public virtual Seat Seat { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }
        public bool IsPaymentSuccess { get; set; } = false;
        public string AppTransId { get; set; }
    }
}
