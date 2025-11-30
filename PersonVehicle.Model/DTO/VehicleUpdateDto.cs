

using System.ComponentModel.DataAnnotations;

namespace PersonVehicle.Model.DTO
{
    public class VehicleUpdateDto
    {
        [Required] public string Plate { get; set; }
        [Required] public string Make { get; set; }
        [Required] public string Model { get; set; }
        [Required] public int Year { get; set; }
    }

}
