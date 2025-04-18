using DAL.Context;
using DAL.Models;
using DAL.Repository.Interface;

namespace DAL.Repository.Implement
{
    internal class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(AppDbContext context) : base(context)
        {
        }
    }
}
