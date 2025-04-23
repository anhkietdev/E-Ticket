using System.Text.Json.Serialization;

namespace BAL.DTOs.ZaloPay
{
    public class CallBackPaymentDTO
    {
        [JsonPropertyName("data")]
        public string Data { get; set; }
        [JsonPropertyName("mac")]
        public string Mac { get; set; }
    }
}
