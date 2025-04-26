using BAL.DTOs.Authentication;
using DAL.Models;

namespace BAL.Services.Interface
{
    public interface IUserService
    {
        Task<AuthenResultDto> LoginAsync(LoginDto loginDto);
        Task<AuthenResultDto> RegisterAsync(RegisterDto registerDto);
        Task ForgotPasswordAsync(string email);
        Task<bool> VerifyOtpAsync(VerifyOtpDto verifyDto);
        Task<User> GetProfileAsync();

        Task<ICollection<User>> GetAllAsync();

    }
}
