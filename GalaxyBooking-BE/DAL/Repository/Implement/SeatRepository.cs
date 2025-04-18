using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;

namespace DAL.Repository.Implement
{
    internal class SeatRepository : Repository<Seat>, ISeatRepository
    {
        public SeatRepository(AppDbContext context) : base(context)
        {
        }
    }
}
