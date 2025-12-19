using System.ComponentModel.DataAnnotations;
using ProyectoFinal.Models;

public class Student
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
