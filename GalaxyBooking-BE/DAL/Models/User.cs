namespace DAL.Models
{
    public class User : BaseEntity
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public UserRole Role { get; set; } = UserRole.Customer;
        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
