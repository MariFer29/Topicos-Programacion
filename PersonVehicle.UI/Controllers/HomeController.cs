using Microsoft.AspNetCore.Mvc;
using PersonVehicle.UI.Models;
using System.Diagnostics;

namespace PersonVehicle.UI.Controllers
{
    public class HomeController(ApiService servicioApis) : Controller
    {
        private readonly ApiService _apiService = servicioApis;

        private const string apiKey = "123456";
        //private readonly ILogger<HomeController> _logger;
        //private readonly ApiService _apiService;

        //public HomeController(ILogger<HomeController> logger, ApiService apiService)
        //{
        //    _logger = logger;
        //    _apiService = apiService;
        //}

        public async Task<IActionResult> Index()
        {
            // Cargar todas las personas para mostrar en la lista
            ViewBag.Persons = await _apiService.ObtenerListaPersonasAsync();
            return View();
        }

        //PERSONAS
        [HttpPost]
        public async Task<IActionResult> CreatePerson(Persons person)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor complete todos los campos requeridos correctamente.";
                return RedirectToAction("Index");
            }

            var success = await _apiService.AgregarPersonaAsync(person) 
                //.AgregarPersonaAsync(person);

            if (success)
                TempData["Success"] = "Persona creada exitosamente.";

            else
                TempData["Error"] = "Error al crear la persona. Verifique que la identificación no exista.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SearchPersonByIdentification(int identification, Persons person)
        {
            if (person.Identification.ToString().Length != 9)
            {
                TempData["Error"] = "Por favor ingrese una identificación.";
                return RedirectToAction("Index");
            }

            var personIDe = await _apiService.ObtenerListaPersonasPorIdentificacionAsync(identification);
            
            if (personIDe == null)
            {
                TempData["Error"] = $"No se encontró ninguna persona con identificación: {identification}";
            }
            else
            {
                TempData["PersonResult"] = System.Text.Json.JsonSerializer.Serialize(person);
                TempData["Success"] = $"Persona encontrada: {person.FullName}";
            }

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public async Task<IActionResult> SearchVehiclesByOwner(string ownerIdentification)
        //{
        //    if (string.IsNullOrWhiteSpace(ownerIdentification))
        //    {
        //        TempData["Error"] = "Por favor ingrese una identificación.";
        //        return RedirectToAction("Index");
        //    }

        //    var vehicles = await _apiService.GetVehiclesByOwnerAsync(ownerIdentification);
            
        //    if (vehicles == null || !vehicles.Any())
        //    {
        //        TempData["Error"] = $"No se encontraron vehículos para la identificación: {ownerIdentification}";
        //    }
        //    else
        //    {
        //        TempData["VehiclesResult"] = System.Text.Json.JsonSerializer.Serialize(vehicles);
        //        TempData["Success"] = $"Se encontraron {vehicles.Count} vehículo(s).";
        //    }

        //    return RedirectToAction("Index");
        //}

        

        [HttpPost]
        public async Task<IActionResult> CreateVehicle(Vehicles vehicle)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor complete todos los campos requeridos correctamente.";
                return RedirectToAction("AllVehicles");
            }

            var success = await _apiService.AgregueNuevoVehiculo(vehicle);
            
            if (success)
            {
                TempData["Success"] = "Vehículo creado exitosamente.";
            }
            else
            {
                TempData["Error"] = "Error al crear el vehículo. Verifique que la placa no exista y que la identificación del propietario sea válida.";
            }

            return RedirectToAction("AllVehicles");
        }

        public async Task<IActionResult> EditPerson(int identification, Persons persona)
        {
            var person = await _apiService.EditarPersonaAsync(identification persona);
            if (person == null)
            {
                TempData["Error"] = "Persona no encontrada.";
                return RedirectToAction("Index");
            }

            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePerson(int identification, Persons person)
        {
            if (!ModelState.IsValid)
            {
                var existingPerson = await _apiService.GetPersonByIdentificationAsync(identification);
                return View("EditPerson", existingPerson);
            }

            var success = await _apiService.UpdatePersonAsync(identification, person);
            
            if (success)
            {
                TempData["Success"] = "Persona actualizada exitosamente.";
            }
            else
            {
                TempData["Error"] = "Error al actualizar la persona.";
            }

            return RedirectToAction("Index");
        }



        //VEHICULOS
        public async Task<IActionResult> AllVehicles()
        {
            var vehicles = await _apiService.GetAllVehiclesAsync();
            return View(vehicles);
        }

        public async Task<IActionResult> EditVehicle(string plate)
        {
            var vehicles = await _apiService.GetAllVehiclesAsync();
            var vehicle = vehicles.FirstOrDefault(v => v.Plate == plate);
            
            if (vehicle == null)
            {
                TempData["Error"] = "Vehículo no encontrado.";
                return RedirectToAction("AllVehicles");
            }

            return View(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVehicle(string plate, Vehicles vehicle)
        {
            if (!ModelState.IsValid)
            {
                var vehicles = await _apiService.GetAllVehiclesAsync();
                var existingVehicle = vehicles.FirstOrDefault(v => v.Plate == plate);
                return View("EditVehicle", existingVehicle);
            }

            var success = await _apiService.UpdateVehicleAsync(plate, vehicle);
            
            if (success)
            {
                TempData["Success"] = "Vehículo actualizado exitosamente.";
            }
            else
            {
                TempData["Error"] = "Error al actualizar el vehículo.";
            }

            return RedirectToAction("AllVehicles");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
