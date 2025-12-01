using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PersonVehicle.Model
{
    // Indica que esta clase se mapeará a la tabla "Owner".
    [Table("Owner")]
    public class Owner
    {
        // Llave primaria del registro de propietarios.
        // Se ignora en JSON para no exponerlo en respuestas.
        [Key]
        [JsonIgnore]
        public int idOwner { get; set; }

        // ID de la persona a la que pertenece el vehículo.
        // Se guarda en la BD, pero no se muestra en JSON.
        [JsonIgnore]
        public int Person_idPerson { get; set; }

        // Identificación de la persona (cédula).
        // No se guarda en BD: solo se utiliza para solicitudes.
        [NotMapped]
        public int OwnerIdentification { get; set; }

        // Relación con la entidad Persons.
        // No se guarda directamente en esta tabla.
        // Se ignora en JSON para evitar ciclos o datos innecesarios.
        [NotMapped]
        [JsonIgnore]
        public Persons? Person { get; set; }

        // ID del vehículo asociado a este propietario.
        // No se expone en JSON.
        [JsonIgnore]
        public int Vehicle_idVehicle { get; set; }

        // Relación con la entidad Vehicles.
        // No se mapea en BD porque la relación viene desde Vehicles → Owner.
        // Se ignora en JSON para evitar recursión.
        [NotMapped]
        [JsonIgnore]
        public Vehicles? Vehicle { get; set; }
    }
}

