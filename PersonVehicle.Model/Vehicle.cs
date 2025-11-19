using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PersonVehicleApi.Model
{
    public class Vehicle
    {
        public int Id { get; set; } // Identificador único del vehículo

        [Required]
        public string Plate { get; set; } // Placa del vehículo (única)

        public string Make { get; set; } // Marca del vehículo
        public string Model { get; set; } // Modelo del vehículo
        public int Year { get; set; } // Año del vehículo

        [Required]
        public int OwnerId { get; set; } // ID de la persona propietaria

        [ForeignKey("OwnerId")]
        [JsonIgnore] // Se ignora en JSON para evitar ciclos
        public Person Owner { get; set; } // Referencia al dueño del vehículo
    }
}


