//using PersonVehicle.UI.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//// Agregar HttpClient para comunicación con la API
//builder.Services.AddHttpClient("PersonVehicleApi", client =>
//{
//    client.BaseAddress = new Uri("https://localhost:7062/"); // Puerto corregido para tu API
//});

//// Registrar el servicio de API
//builder.Services.AddScoped<ApiService>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();

using GestionDePersonas.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var personasApiConfig = builder.Configuration.GetSection("PersonasApi");
var urlBase = personasApiConfig.GetValue<string>("BaseUrl");
var apiKey = personasApiConfig.GetValue<string>("ApiKey");
builder.Services.AddHttpClient("PersonasApi", client =>
{
    client.BaseAddress = new Uri(urlBase);
    client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
});

builder.Services.AddScoped<ServicioApi>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=GestionDePersonas}/{action=Index}/{id?}");

app.Run();
