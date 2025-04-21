using BAL.Services.Interface;
using DAL.Repository.Interface;

namespace BAL.Services.Implement
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authRepository;

        public AuthenticationService(IAuthenticationRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<string> LoginAsync(string email, string password)
        {
            try
            {
                // Gọi repository để xác thực
                var user = await _authRepository.LoginAsync(email, password);

                if (user == null)
                {
                    throw new UnauthorizedAccessException("Invalid username or password.");
                }

                return "Login successful";
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (nên dùng ILogger)
                Console.WriteLine($"Error in LoginAsync: {ex.Message}");
                throw;
            }
        }
    }
}
