
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = "";

        [Required]
        public int CareerId { get; set; }
        public Career? Career { get; set; }

        [Range(1, 10)]
        public int Credits { get; set; } = 3;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
