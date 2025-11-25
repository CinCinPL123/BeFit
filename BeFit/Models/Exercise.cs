
using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Session", Description = "Training session")]
        public int SessionId { get; set; }
        [Display(Name = "Session")]
        public required Session Session { get; set; }

        [Required]
        [Display(Name = "Exercise Type", Description = "Type of exercise")]
        public int ExerciseTypeId { get; set; }

        [Display(Name = "Exercise Type")]
        public required ExerciseType ExerciseType { get; set; }

        [Display(Name = "Weight", Description = "Weight in kilograms")]
        public double? Weight { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Series", Description = "Number of series")]
        public int Series { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Repetitions", Description = "Number of repetitions per series")]
        public int Repetitions { get; set; }

        [Display(Name = "Created by")]
        public string CreatedById { get; set; }
        [Display(Name = "Created by")]
        public virtual AppUser? CreatedBy { get; set; }
    }
}
