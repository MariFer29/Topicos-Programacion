using System.ComponentModel.DataAnnotations;

namespace PersonVehicle.UI.Models
{
    public class Vehicles
    {
        public int idVehicle { get; set; }

        [Required(ErrorMessage = "La placa es requerida")]
        [Display(Name = "Placa")]
        public string Plate { get; set; } = string.Empty;

        [Required(ErrorMessage = "La marca es requerida")]
        [Display(Name = "Marca")]
        public string Make { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es requerido")]
        [Display(Name = "Modelo")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año es requerido")]
        [Display(Name = "Año")]
        [Range(1900, 2030, ErrorMessage = "El año debe estar entre 1900 y 2030")]
        public int Year { get; set; }

        // Para ver quién es el propietario actual (solo ID)
        [Display(Name = "Propietario actual")]
        public int? PersonIdentification { get; set; }

        // Relación con Owner que contiene la información completa
        public Owner? Owner { get; set; }

        // Propiedad computada para mostrar el nombre del propietario
        public string OwnerName => Owner?.Person != null ? Owner.Person.FullName : "";
    }
}