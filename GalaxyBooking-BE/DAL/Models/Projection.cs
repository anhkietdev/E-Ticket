using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Projection : BaseEntity
    {
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
