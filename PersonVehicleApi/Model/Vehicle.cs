using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PersonVehicleApi.Model
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        public string Plate { get; set; } // ejemplo: ABC-123

        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]

        [JsonIgnore]
        public Person Owner { get; set; }
    }
}

