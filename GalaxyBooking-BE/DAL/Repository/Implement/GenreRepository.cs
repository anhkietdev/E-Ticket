using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;

namespace DAL.Repository.Implement
{
    internal class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(AppDbContext context) : base(context)
        {
        }
    }

}
