using DAL.Context;
using DAL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly AppDbContext _context;

        public AuthenticationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> LoginAsync(string email, string password)
        {
            try
            {
                // Tìm người dùng theo username
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

                if (user == null)
                {
                    return null; // Người dùng không tồn tại
                }

                return user;
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
