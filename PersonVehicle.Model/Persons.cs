using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace PersonVehicle.Model
{
    [Table("Persons")]
    public class Persons
    {
        [Key][JsonIgnore] public int idPerson { get; set; }  // Identificador único en la base de datos
        [Required] public int Identification { get; set; } // Identificación única de la persona
        [Required] public string FirstName { get; set; } // Primer nombre de la persona
        [Required] public string LastName { get; set; } // Apellido de la persona
        [Required] public string Email { get; set; } // Correo electrónico
        [Required] public int Phone { get; set; } // Número de teléfono
        [Required] public decimal Salario { get; set; } // Salario de la Persona
        [NotMapped][JsonIgnore] public ICollection<Owner> Owners { get; set; } = new List<Owner>();
    }
}
