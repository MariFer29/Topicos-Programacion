using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonVehicle.Model
{
    public class msjResp
    {
        [Key] public int id { get; set; }
        public string Mensaje { get; set; }
    }
}
