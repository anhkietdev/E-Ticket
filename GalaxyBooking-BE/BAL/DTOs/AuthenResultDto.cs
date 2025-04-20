namespace BAL.DTOs
{
    public class AuthenResultDto
    {
        public string Token { get; set; };
        public string RefreshToken { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
