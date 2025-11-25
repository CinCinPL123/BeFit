using System.ComponentModel.DataAnnotations;

namespace BeFit.DTOs
{
    public class SessionDTO
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime? EndTime { get; set; }
    }
}
