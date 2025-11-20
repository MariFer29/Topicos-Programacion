using System.ComponentModel.DataAnnotations;

namespace PersonVehicle.UI.Models
{
    public class CreatePersonDto
    {
        [Required(ErrorMessage = "La identificación es requerida")]
        public string Identification { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
    }

    public class UpdatePersonDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    public class CreateVehicleDto
    {
        [Required(ErrorMessage = "La placa es requerida")]
        public string Plate { get; set; } = string.Empty;

        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        [Range(1900, 2030, ErrorMessage = "El año debe estar entre 1900 y 2030")]
        public int Year { get; set; }

        [Required(ErrorMessage = "La identificación del propietario es requerida")]
        public string OwnerIdentification { get; set; } = string.Empty;
    }

    public class UpdateVehicleDto
    {
        public string? Make { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
    }
}