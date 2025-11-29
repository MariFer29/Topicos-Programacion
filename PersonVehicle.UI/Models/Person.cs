using PersonVehicle.Model;
using System.ComponentModel.DataAnnotations;

namespace PersonVehicle.UI.Models
{
    public class Person
    {
        public int idPerson { get; set; }

        [Required(ErrorMessage = "La identificación es requerida")]
        [Display(Name = "Identificación")]
        public int Identification { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        public int Phone { get; set; }

        [Required(ErrorMessage = "El salario es requerido")]
        [Display(Name = "Salario")]
        public decimal Salario { get; set; }

        public List<Vehicle> Vehicles { get; set; } = new();
        public List<Owner> Owners { get; set; } = new();
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}