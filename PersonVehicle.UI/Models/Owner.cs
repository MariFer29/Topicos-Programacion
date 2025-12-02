using System.ComponentModel.DataAnnotations;

namespace PersonVehicle.UI.Models
{
    public class Owner
    {
        public int idOwner { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una persona")]
        [Display(Name = "Persona")]
        public int Persons_idPerson { get; set; }

        public int Person_idPerson { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un vehículo")]
        [Display(Name = "Vehículo")]
        public int Vehicle_idVehicle { get; set; }

        public int OwnerIdentification { get; set; }

        // Relación con la persona (para mostrar información completa)
        public Persons? Person { get; set; }

        // Para mostrar en la vista
        public string? PersonName { get; set; }
        public string? VehiclePlate { get; set; }
    }
}
