using BeFit.Models;
using System.ComponentModel.DataAnnotations;

namespace BeFit.DTOs
{
    public class ExerciseDTO
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Session", Description = "Training session")]
        public int SessionId { get; set; }

        [Required]
        [Display(Name = "Exercise Type", Description = "Type of exercise")]
        public int ExerciseTypeId { get; set; }

        [Display(Name = "Weight", Description = "Weight in kilograms")]
        public double? Weight { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Series", Description = "Number of series")]
        public int Series { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Repetitions", Description = "Number of repetitions per series")]
        public int Repetitions { get; set; }
    }
}
