using System.Text.Json.Serialization;

namespace BAL.DTOs.ZaloPay
{
    public class CallBackObj
    {
        [JsonPropertyName("appid")]
        public long AppId { get; set; }

        [JsonPropertyName("apptransid")]
        public string AppTransId { get; set; }

        [JsonPropertyName("apptime")]
        public long AppTime { get; set; }

        [JsonPropertyName("appuser")]
        public string AppUser { get; set; }

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("embeddata")]
        public string EmbedData { get; set; }

        [JsonPropertyName("item")]
        public string Item { get; set; }

        [JsonPropertyName("zptransid")]
        public long ZpTransId { get; set; }

        [JsonPropertyName("servertime")]
        public long ServerTime { get; set; }

        [JsonPropertyName("channel")]
        public int Channel { get; set; }

        [JsonPropertyName("merchantuserid")]
        public string MerchantUserId { get; set; }

        [JsonPropertyName("userfeeamount")]
        public long UserFeeAmount { get; set; }

        [JsonPropertyName("discountamount")]
        public long DiscountAmount { get; set; }
    }
}
