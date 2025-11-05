
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        [Required, StringLength(120)]
        public string StudentName { get; set; } = "";

        [Required, EmailAddress]
        public string StudentEmail { get; set; } = "";

        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    }
}
