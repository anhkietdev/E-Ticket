using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repository.Implement
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Room> FindByIdAsync(Guid id)
        {
            return await _context.Set<Room>()
                .Include(r => r.Seats)
                .Include(r => r.Projections)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<Room> FindByRoomNumberAsync(string roomNumber)
        {
            return await _context.Set<Room>()
                .Include(r => r.Seats)
                .Include(r => r.Projections)
                .FirstOrDefaultAsync(r => r.RoomNumber == roomNumber && !r.IsDeleted);
        }

        public async Task<ICollection<Room>> FindByRoomTypeAsync(RoomType roomType)
        {
            return await _context.Set<Room>()
                .Include(r => r.Seats)
                .Include(r => r.Projections)
                .Where(r => r.Type == roomType && !r.IsDeleted)
                .ToListAsync();
        }
    }
}