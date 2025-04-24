using Newtonsoft.Json;

namespace BAL.Services.ZaloPay.Request
{
    public class CreateZalopayRequest
    {
        public int? AppId { get; set; }
        public string AppUser { get; set; }
        public long AppTime { get; set; }
        public long Amount { get; set; }
        public string AppTransId { get; set; } = string.Empty;
        public string ReturnUrl { get; set; }
        public string EmbedData { get; set; } = string.Empty;
        public string Mac { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CallbackUrl { get; set; } = string.Empty;
        public string Item { get; set; } = string.Empty;
    }

    public class Item
    {
        [JsonProperty("itemid")]
        public Guid ItemId { get; set; }
    }
}
