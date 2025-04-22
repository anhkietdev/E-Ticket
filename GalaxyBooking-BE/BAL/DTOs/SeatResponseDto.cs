using System;
using System.Collections.Generic;

namespace BAL.DTOs
{
    public class SeatResponseDto
    {
        public Guid Id { get; set; }
        public required string SeatNumber { get; set; }
        public required string Row { get; set; }
        public Guid RoomId { get; set; }
        public RoomDto Room { get; set; }
        public ICollection<TicketDto> Tickets { get; set; }
    }
}