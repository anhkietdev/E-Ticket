using BAL.Interfaces;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
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
