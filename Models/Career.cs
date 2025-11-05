
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class Career
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = "";

        [StringLength(200)]
        public string? Description { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
