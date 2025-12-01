using System.ComponentModel.DataAnnotations;

namespace PersonVehicle.Model
{
    public class msjResp
    {
        // Llave primaria para la tabla msjResp.
        [Key]
        public int id { get; set; }

        // Mensaje que describe el resultado de alguna operación en la lógica del sistema.
        public string Mensaje { get; set; }
    }
}

