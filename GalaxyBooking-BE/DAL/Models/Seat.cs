using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace DAL.Models
{
    public class Seat : BaseEntity
    {
        [Key]
        public int SeatId { get; set; }

        [Required]
        [StringLength(10)]
        public string SeatNumber { get; set; }

        [Required]
        public string Row { get; set; }

        // Khóa ngoại đến phòng chiếu
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

        // Quan hệ 1-nhiều với Ticket
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
