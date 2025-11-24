using Microsoft.EntityFrameworkCore;
using PersonVehicleApi.DA;
using PersonVehicleApi.BL; 

var builder = WebApplication.CreateBuilder(args);

// Agrega los servicios necesarios para usar controladores (API)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configurar el manejo de ciclos de referencia para evitar errores al serializar
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Herramientas para documentar y explorar la API (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Obtiene la cadena de conexión desde appsettings.json
// Si no existe, usa una cadena de conexión por defecto
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Server=(localdb)\\mssqllocaldb;Database=PersonVehicleDb;Trusted_Connection=True;";

// Registro del DbContext para que la aplicación pueda usar EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registro de la Capa BL (Business Logic)
builder.Services.AddScoped<PersonsBL>();
builder.Services.AddScoped<VehiclesBL>();


var app = builder.Build();

// Habilita Swagger solo en modo Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirige solicitudes HTTP a HTTPS
app.UseHttpsRedirection();

// Middleware de autorización (por si se agrega seguridad luego)
app.UseAuthorization();

// Mapea automáticamente los controladores a sus rutas
app.MapControllers();

// Crear un scope temporal para ejecutar migraciones y datos iniciales
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Aplica migraciones automáticamente al iniciar
    db.Database.Migrate();

    // Si no hay personas en la base de datos, se agregan datos iniciales
    if (!db.Persons.Any())
    {
        var p = new PersonVehicleApi.Model.Person
        {
            Identification = "12345678",
            FirstName = "Juan",
            LastName = "Pérez",
            Email = "juan@example.com",
            Phone = "60000000"
        };

        db.Persons.Add(p);

        // Vehículos de ejemplo asociados a la persona
        db.Vehicles.Add(new PersonVehicleApi.Model.Vehicle
        {
            Plate = "ABC-123",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2018,
            Owner = p
        });

        db.Vehicles.Add(new PersonVehicleApi.Model.Vehicle
        {
            Plate = "XYZ-999",
            Make = "Honda",
            Model = "Civic",
            Year = 2020,
            Owner = p
        });

        db.SaveChanges(); // Guarda los datos iniciales en la base
    }
}

app.Run(); // Ejecuta la aplicación


