using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Agrega los servicios necesarios para usar controladores (API)
builder.Services.AddControllers();

// Herramientas para documentar y explorar la API (Swagger)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CAMBIAR PARA BASE DE DATOS
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); //Conexion

builder.Services.AddDbContext<PersonVehicle.DA.AppDbContext>(options =>
    options.UseSqlServer(connectionString));

//CAMBIAR PARA MEMORIA 
//builder.Services.AddDbContext<PersonVehicle.DA.AppDbContext>(options =>
//    options.UseInMemoryDatabase("ProyTAP"));


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


