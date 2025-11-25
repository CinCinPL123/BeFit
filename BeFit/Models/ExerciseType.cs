using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Name", Description = "Exercise type name")]
        public required string Name { get; set; }

    }
}
