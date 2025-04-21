using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Implement
{
    public class AuthenticationRepository : Repository<User>, IAuthenticationRepository
    {
        private readonly AppDbContext _context;
        public AuthenticationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            if (user.Password != password)
                return null;

            return user;
        }
    }
}
