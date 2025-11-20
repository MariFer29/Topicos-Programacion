using System.ComponentModel.DataAnnotations;

namespace PersonVehicle.UI.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La placa es requerida")]
        [Display(Name = "Placa")]
        public string Plate { get; set; } = string.Empty;

        [Display(Name = "Marca")]
        public string Make { get; set; } = string.Empty;

        [Display(Name = "Modelo")]
        public string Model { get; set; } = string.Empty;

        [Display(Name = "Año")]
        [Range(1900, 2030, ErrorMessage = "El año debe estar entre 1900 y 2030")]
        public int Year { get; set; }

        public int OwnerId { get; set; }
        public Person? Owner { get; set; }

        public string OwnerName => Owner != null ? Owner.FullName : "";
    }
}