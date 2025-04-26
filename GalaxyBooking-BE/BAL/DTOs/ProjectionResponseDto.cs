using System;
using System.Collections.Generic;

namespace BAL.DTOs
{
    public class ProjectionResponseDto
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Price { get; set; }
        public FilmDto Film { get; set; }
        public RoomDto Room { get; set; }
        public ICollection<TicketDto> Tickets { get; set; }
    }

    public class FilmDto
    {
        public string Title { get; set; }
    }

    public class RoomDto
    {
        public string RoomNumber { get; set; }
        public Guid Id { get; set; }
    }

    public class TicketDto
    {
        public Guid Id { get; set; }
        public DateTime PurchaseTime { get; set; }
        public Guid ProjectionId { get; set; }
        public Guid SeatId { get; set; }
        public Guid UserId { get; set; }
        public Guid FilmId { get; set; }
        public string SeatNumber { get; set; }
        public string RoomNumber { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string AppTransId { get; set; }
    }
}