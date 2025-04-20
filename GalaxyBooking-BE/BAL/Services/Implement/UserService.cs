using BAL.DTOs;
using BAL.Services.Interface;

namespace BAL.Services.Implement
{
    public class UserService : IUserService
    {
        public Task<AuthenResultDto> LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenResultDto> RegisterAsync(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }
    }
}
