using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class RoomResponseDto
    {
        public Guid Id { get; set; }
        public string RoomNumber { get; set; }
        public RoomType Type { get; set; }
        public int Capacity { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
        public virtual ICollection<Projection> Projections { get; set; }
    }
}
