using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;

namespace DAL.Repository.Implement
{
    internal class ProjectionRepository : Repository<Projection>, IProjectionRepository
    {
        public ProjectionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
