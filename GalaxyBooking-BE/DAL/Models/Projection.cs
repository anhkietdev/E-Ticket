using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Projection
    {
        [Key]
        public int ProjectionId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        // Khóa ngoại đến phim
        public int FilmId { get; set; }

        [ForeignKey("FilmId")]
        public virtual Film Film { get; set; }

        // Khóa ngoại đến phòng chiếu
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

        // Quan hệ 1-nhiều với Ticket
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
