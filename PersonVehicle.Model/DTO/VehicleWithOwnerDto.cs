namespace PersonVehicle.Model.DTO
{
    // DTO para devolver información completa del vehículo con propietario
    // Solo se usa en la búsqueda por placa
    public class VehicleWithOwnerDto
    {
        public string Plate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int PersonIdentification { get; set; }
        
        // Información completa del propietario
        public OwnerInfoDto? Owner { get; set; }
    }

    public class OwnerInfoDto
    {
        public int OwnerIdentification { get; set; }
        public PersonInfoDto? Person { get; set; }
    }

    public class PersonInfoDto
    {
        public int Identification { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public decimal Salario { get; set; }
    }
}
