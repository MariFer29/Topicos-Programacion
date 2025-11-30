using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PersonVehicle.Model
{
    [Table("Vehicles")]
    public class Vehicles
    {
        [Key][JsonIgnore] public int idVehicle { get; set; } // Identificador único del vehículo
        [Required] public string Plate { get; set; } // Placa del vehículo (única)
        [Required] public string Make { get; set; } // Marca del vehículo
        [Required] public string Model { get; set; } // Modelo del vehículo
        [Required] public int Year { get; set; } // Año del vehículo
        [NotMapped] public int PersonIdentification { get; set; }
        //[NotMapped] public string PersonFirstName { get; set; }
        //[NotMapped] public string PersonLastName { get; set; }
        [NotMapped][JsonIgnore] public Owner? Owner { get; set; }
    }


}
