using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class ProjectionDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal Price { get; set; }

        public Guid FilmId { get; set; }

        public virtual Film Film { get; set; }

        public Guid RoomId { get; set; }

        public virtual Room Room { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
