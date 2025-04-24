using DAL.Models;

namespace BAL.Services.Interface
{
    public interface IAuthenticationService
    {
        Task<User> LoginAsync(string email, string password);
    }
}
