using System.ComponentModel.DataAnnotations;

namespace PersonVehicle.UI.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La identificación es requerida")]
        [Display(Name = "Identificación")]
        public string Identification { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Apellido")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        public string Phone { get; set; } = string.Empty;

        public List<Vehicle> Vehicles { get; set; } = new();

        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}