using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseStatistics
    {
        [Display(Name = "Exercise Type")]
        public string ExerciseTypeName { get; set; }

        [Display(Name = "Execution Count")]
        public int ExecutionCount { get; set; }

        [Display(Name = "Total Repetitions")]
        public int TotalRepetitions { get; set; }

        [Display(Name = "Average Weight (kg)")]
        public double? AverageWeight { get; set; }

        [Display(Name = "Max Weight (kg)")]
        public double? MaxWeight { get; set; }
    }
}
