using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;

namespace DAL.Repository.Implement
{
    public class IdentityUserRepository : Repository<IdentityUser>, IIdentityUserRepository
    {
        public IdentityUserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
