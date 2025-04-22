using DAL.Models;
using System;
using System.Threading.Tasks;

namespace DAL.Repository.Interface
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<Genre> FindByIdAsync(Guid id);
    }
}