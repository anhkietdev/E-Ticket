using DAL.Models;

namespace DAL.Repository.Interface
{
    public interface IAuthenticationRepository
    {
        Task<User> LoginAsync(string email, string password);
    }
}
