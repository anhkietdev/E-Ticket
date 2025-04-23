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
        public virtual ICollection<SeatDto> Seats { get; set; }
        public virtual ICollection<ProjectionDto> Projections { get; set; }
    }

    public class SeatDto
    {
        public required string SeatNumber { get; set; }
        public required string Row { get; set; }
    }
}
