using BAL.Services.ZaloPay.Request;

namespace BAL.DTOs
{
    public class PaymentDTO
    {
        public string PaymentContent { get; set; } = string.Empty;
        public string PaymentCurrency { get; set; } = string.Empty;
        public string PaymentRefId { get; set; } = string.Empty;
        public decimal? RequiredAmount { get; set; }
        public string? BankCode { get; set; }
        public Guid? UserId { get; set; }
        public List<Guid> TicketIds { get; set; }
        public Guid? ProjectionId { get; set; }
        public List<Item> Items { get; set; }
    }
}
