using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonVehicleApi.Model
{
    public class Person
    {
        public int Id { get; set; }  // Identificador único en la base de datos

        [Required]
        public string Identification { get; set; } // Identificación única de la persona

        [Required]
        public string FirstName { get; set; } // Primer nombre de la persona

        public string LastName { get; set; } // Apellido de la persona
        public string Email { get; set; } // Correo electrónico
        public string Phone { get; set; } // Número de teléfono

        // Lista de vehículos asociados a esta persona
        public List<Vehicle> Vehicles { get; set; } = new();
    }
}
