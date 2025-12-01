using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PersonVehicle.Model
{
    // Indica que esta clase representa la tabla "Vehicles" en la base de datos.
    [Table("Vehicles")]
    public class Vehicles
    {
        // Llave primaria del vehículo.
        // Se ignora en JSON para no exponer el ID interno.
        [Key]
        [JsonIgnore]
        public int idVehicle { get; set; }

        // Número de placa del vehículo. Es obligatoria y debe ser única.
        [Required]
        public string Plate { get; set; }

        // Marca del vehículo.
        [Required]
        public string Make { get; set; }

        // Modelo del vehículo.
        [Required]
        public string Model { get; set; }

        // Año de fabricación.
        [Required]
        public int Year { get; set; }

        // Identificación del dueño.
        // No se almacena en base de datos, solo se usa para transferir datos.
        [NotMapped]
        public int PersonIdentification { get; set; }

        // Propietario asociado al vehículo.
        // No se mapea a la BD porque la relación se maneja desde la tabla Owner.
        // Se ignora en JSON para evitar ciclos y sobrecarga de datos.
        [NotMapped]
        [JsonIgnore]
        public Owner? Owner { get; set; }
    }
}
