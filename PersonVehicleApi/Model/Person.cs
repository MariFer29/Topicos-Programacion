using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PersonVehicleApi.Model
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        public string Identification { get; set; } 

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public List<Vehicle> Vehicles { get; set; } = new();
    }
}

