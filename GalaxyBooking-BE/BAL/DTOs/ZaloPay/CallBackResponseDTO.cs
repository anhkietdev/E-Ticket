using System.Text.Json.Serialization;

namespace BAL.DTOs.ZaloPay
{
    public class CallBackResponseDTO
    {
        [JsonPropertyName("returncode")]
        public int ReturnCode { get; set; }

        [JsonPropertyName("returnmessage")]
        public string ReturnMessage { get; set; }
    }
}
