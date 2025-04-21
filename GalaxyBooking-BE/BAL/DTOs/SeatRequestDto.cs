using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTOs
{
    public class SeatRequestDto
    {
        public required string SeatNumber { get; set; }

        public required string Row { get; set; }

        public Guid RoomId { get; set; }
    }
}
