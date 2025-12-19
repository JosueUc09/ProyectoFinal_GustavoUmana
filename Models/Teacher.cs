using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";
    }
}
