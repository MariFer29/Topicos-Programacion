using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Agrega los servicios necesarios para usar controladores (API)
builder.Services.AddControllers();

// Herramientas para documentar y explorar la API (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Obtiene la cadena de conexión desde appsettings.json
/*
// Si no existe, usa una cadena de conexión por defecto
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       //?? "Server=(localdb)\\mssqllocaldb;Database=PersonVehicleDb;Trusted_Connection=True;";

// Registro del DbContext para que la aplicación pueda usar EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
*/

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); //Conexion

//builder.Services.AddDbContext<PersonVehicle.DA.AppDbContext>(options =>
//    options.UseSqlServer(connectionString)); // en Base de Datos

builder.Services.AddDbContext<PersonVehicle.DA.AppDbContext>(options =>
    options.UseInMemoryDatabase("ProyTAP")); //En Memoria


builder.Services.AddScoped<PersonVehicle.BL.IOwnerRepository, PersonVehicle.DA.OwnerRepository>();
builder.Services.AddScoped<PersonVehicle.BL.IPersonRepository, PersonVehicle.DA.PersonRepository>();
builder.Services.AddScoped<PersonVehicle.BL.IVehicleRepository, PersonVehicle.DA.VehicleRepository>();
builder.Services.AddScoped<PersonVehicle.BL.IAdministradorDePersons, PersonVehicle.BL.AdministradorDePersons>();
builder.Services.AddScoped<PersonVehicle.BL.IAdministradorDeVehicles, PersonVehicle.BL.AdministradorDeVehicles>();


var app = builder.Build();

// Habilita Swagger siempre (local y Azure)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API PersonasVehículos v1");
    c.RoutePrefix = "swagger"; // Accesible desde /swagger/index.html
});

// Redirige solicitudes HTTP a HTTPS
app.UseHttpsRedirection();

// Middleware de autorización (por si se agrega seguridad luego)
app.UseAuthorization();

// Mapea automáticamente los controladores a sus rutas
app.MapControllers();


app.Run(); // Ejecuta la aplicación


