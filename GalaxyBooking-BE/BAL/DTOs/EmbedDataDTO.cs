using System.Text.Json.Serialization;

namespace BAL.DTOs
{
    public class EmbedDataDTO
    {
        [JsonPropertyName("ticketIds")]
        public List<Guid> TicketIds { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("projectionId")]
        public string ProjectionId { get; set; }

        [JsonPropertyName("redirecturl")]
        public string RedirectUrl { get; set; } = string.Empty;
    }
}
