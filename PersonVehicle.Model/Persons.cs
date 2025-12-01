using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PersonVehicle.Model
{
    // Indica que esta clase se mapea a la tabla "Persons".
    [Table("Persons")]
    public class Persons
    {
        // Llave primaria generada por la base de datos.
        // Se ignora en JSON para no exponer información interna.
        [Key]
        [JsonIgnore]
        public int idPerson { get; set; }

        // Identificación oficial de la persona (cédula).
        // Es obligatoria y debe ser única.
        [Required]
        public int Identification { get; set; }

        // Primer nombre de la persona.
        [Required]
        public string FirstName { get; set; }

        // Primer apellido.
        [Required]
        public string LastName { get; set; }

        // Correo electrónico de contacto.
        [Required]
        public string Email { get; set; }

        // Número telefónico.
        [Required]
        public int Phone { get; set; }

        // Salario de la persona.
        // Se usa decimal por ser valor monetario.
        [Required]
        public decimal Salario { get; set; }

        // Lista de vehículos asociados como propietario.
        // No se mapea a la BD porque la relación se maneja desde Owner.
        // Se ignora en JSON para evitar ciclos o cargar demasiada información.
        [NotMapped]
        [JsonIgnore]
        public ICollection<Owner> Owners { get; set; } = new List<Owner>();
    }
}
