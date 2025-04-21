using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class IdentityUser
    {
        [Key]
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; } = default!;
        public byte[]? PasswordHash { get; private set; } = default!;
        public byte[]? PasswordSalt { get; private set; } = default!;
        public UserRole Role { get; private set; }
        public string? OtpCode { get; private set; }
        public DateTime? OtpCodeExpiration { get; private set; }
    }
}
