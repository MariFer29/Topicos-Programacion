using System.ComponentModel.DataAnnotations;

namespace PersonVehicle.UI.Models
{
    public class Owner
    {
        public int idOwner { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una persona")]
        [Display(Name = "Persona")]
        public int Persons_idPerson { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un vehículo")]
        [Display(Name = "Vehículo")]
        public int Vehicle_idVehicle { get; set; }

        // Para mostrar en la vista
        public string? PersonName { get; set; }
        public string? VehiclePlate { get; set; }
    }


}
