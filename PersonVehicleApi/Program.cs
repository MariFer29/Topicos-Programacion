using Microsoft.EntityFrameworkCore;
using PersonVehicle.BL;
using PersonVehicle.DA;

var builder = WebApplication.CreateBuilder(args);

// Agrega los servicios necesarios para usar controladores (API)
builder.Services.AddControllers();

// Herramientas para documentar y explorar la API (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Obtiene la cadena de conexión desde appsettings.json
/*CODIGO MAFER
// Si no existe, usa una cadena de conexión por defecto
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       //?? "Server=(localdb)\\mssqllocaldb;Database=PersonVehicleDb;Trusted_Connection=True;";

// Registro del DbContext para que la aplicación pueda usar EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registro de la Capa BL (Business Logic)
builder.Services.AddScoped<PersonsBL>();
builder.Services.AddScoped<VehiclesBL>();
CODIGO MAFER*/



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); //Conexion

builder.Services.AddDbContext<PersonVehicle.DA.AppDbContext>(options =>
    options.UseSqlServer(connectionString)); // en Base de Datos

//builder.Services.AddDbContext<PersonVehicle.DA.AppDbContext>(options =>
//    options.UseInMemoryDatabase("ProyTAP")); //En Memoria


builder.Services.AddScoped<PersonVehicle.BL.IOwnerRepository, PersonVehicle.DA.OwnerRepository>();
builder.Services.AddScoped<PersonVehicle.BL.IPersonRepository, PersonVehicle.DA.PersonRepository>();
builder.Services.AddScoped<PersonVehicle.BL.IVehicleRepository, PersonVehicle.DA.VehicleRepository>();
builder.Services.AddScoped<PersonVehicle.BL.IAdministradorDePersons, PersonVehicle.BL.AdministradorDePersons>();
builder.Services.AddScoped<PersonVehicle.BL.IAdministradorDeVehicles, PersonVehicle.BL.AdministradorDeVehicles>();


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


/*CODIGO MAFER
//Crear un scope temporal para ejecutar migraciones y datos iniciales
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
        db.Vehicles.Add(new PersonVehicle.Model.Vehicle
        {
            Plate = "ABC-123",
            Make = "Toyota",
            Model = "Corolla",
            Year = 2018,
            Owner = p
        });

        db.Vehicles.Add(new PersonVehicle.Model.Vehicle
        {
            Plate = "XYZ-999",
            Make = "Honda",
            Model = "Civic",
            Year = 2020,
            Owner = p
        });

        db.SaveChanges(); // Guarda los datos iniciales en la base
    }
}*/

app.Run(); // Ejecuta la aplicación


