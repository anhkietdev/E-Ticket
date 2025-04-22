using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;
using System;
using System.Threading.Tasks;

namespace DAL.Repository.Implement
{
    public class SeatRepository : Repository<Seat>, ISeatRepository
    {
        private readonly AppDbContext _context;

        public SeatRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Seat> FindByIdAsync(Guid id)
        {
            return await GetAsync(
                filter: s => s.Id == id && !s.IsDeleted,
                includeProperties: "Room,Tickets");
        }
    }
}