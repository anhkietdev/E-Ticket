using DAL.Models;
using System;
using System.Threading.Tasks;

namespace DAL.Repository.Interface
{
    public interface ISeatRepository : IRepository<Seat>
    {
        Task<Seat> FindByIdAsync(Guid id);
    }
}