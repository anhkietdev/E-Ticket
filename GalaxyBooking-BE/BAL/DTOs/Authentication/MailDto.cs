namespace BAL.DTOs.Authentication
{
    public class MailDto
    {
        public IReadOnlyCollection<string> Receivers { get; set; } = null!;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public IReadOnlyCollection<string> Attachments { get; set; } = new List<string>();
    }
}
