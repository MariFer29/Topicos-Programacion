namespace PersonVehicleApi.Model.Dtos
{
    public class OwnerDto
    {
        public string Identification { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}


