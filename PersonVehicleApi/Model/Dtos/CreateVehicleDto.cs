namespace PersonVehicleApi.Model.Dtos
{
    public class CreateVehicleDto
    {
        public string Plate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string OwnerIdentification { get; set; } 
    }
}

