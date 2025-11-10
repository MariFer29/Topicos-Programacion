using Microsoft.EntityFrameworkCore;
using PersonVehicleApi.DA;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core - leer connection string de appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Server=(localdb)\\mssqllocaldb;Database=PersonVehicleDb;Trusted_Connection=True;";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Usar Swagger siempre en Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

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
        db.Vehicles.Add(new PersonVehicleApi.Model.Vehicle { Plate = "ABC-123", Make = "Toyota", Model = "Corolla", Year = 2018, Owner = p });
        db.Vehicles.Add(new PersonVehicleApi.Model.Vehicle { Plate = "XYZ-999", Make = "Honda", Model = "Civic", Year = 2020, Owner = p });
        db.SaveChanges();
    }
}

app.Run();
