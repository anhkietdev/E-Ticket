using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<User> LoginAsync(string email, string password);
    }
}
