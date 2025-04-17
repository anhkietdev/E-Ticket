using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Room : BaseEntity
    {
        public string RoomNumber { get; set; }

        public RoomType Type { get; set; }

        public int Capacity { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }

        public virtual ICollection<Projection> Projections { get; set; }
    }
}
