using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Start Time", Description = "Session start date and time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time", Description = "Session end date and time")]
        public DateTime? EndTime { get; set; }

        [Display(Name = "Created by")]
        public string CreatedById { get; set; }
        [Display(Name = "Created by")]
        public virtual AppUser? CreatedBy { get; set; }

    }
}
