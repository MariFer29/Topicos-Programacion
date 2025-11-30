
using System.ComponentModel.DataAnnotations;

namespace PersonVehicle.Model.DTO
{
    public class UpdatePersonDto
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public int Phone { get; set; }
        [Required] public decimal Salario { get; set; }
    }


}
