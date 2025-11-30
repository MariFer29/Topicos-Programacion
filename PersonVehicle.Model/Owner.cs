using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PersonVehicle.Model
{
    [Table("Owner")]
    public class Owner
    {
        [Key][JsonIgnore] public int idOwner { get; set; }

        [JsonIgnore] public int Person_idPerson { get; set; }
        [NotMapped] public int OwnerIdentification { get; set; }
        [NotMapped][JsonIgnore] public Persons? Person { get; set; }
        //[NotMapped] public string PlateVehicle { get; set; }
        [JsonIgnore] public int Vehicle_idVehicle { get; set; }
        [NotMapped][JsonIgnore] public Vehicles? Vehicle { get; set; }
    }
}
