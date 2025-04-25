using System;
using System.ComponentModel.DataAnnotations;

namespace BAL.DTOs
{
    public class ProjectionRequestDto
    {
        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "StartDate must be in 'yyyy-MM-dd' format")]
        public string StartDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd"); // e.g., "2025-04-25"

        [Required]
        [RegularExpression(@"^\d{2}:\d{2}:\d{2}$", ErrorMessage = "StartTime must be in 'HH:mm:ss' format")]
        public string StartTime { get; set; } = DateTime.Now.ToString("HH:mm:ss"); // e.g., "09:15:24"

        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "EndDate must be in 'yyyy-MM-dd' format")]
        public string EndDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd"); // e.g., "2025-04-25"

        [Required]
        [RegularExpression(@"^\d{2}:\d{2}:\d{2}$", ErrorMessage = "EndTime must be in 'HH:mm:ss' format")]
        public string EndTime { get; set; } = DateTime.Now.AddHours(2).ToString("HH:mm:ss"); // e.g., "11:15:24"

        [Required]
        public decimal Price { get; set; }

        [Required]
        public Guid FilmId { get; set; }

        [Required]
        public Guid RoomId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}