using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Room : BaseEntity
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoomNumber { get; set; }

        [Required]
        public RoomType Type { get; set; }

        public int Capacity { get; set; }

        // Quan hệ 1-nhiều với ghế
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
