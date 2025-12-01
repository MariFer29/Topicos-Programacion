using Microsoft.AspNetCore.Mvc;
using PersonVehicle.UI.Models;
using PersonVehicle.UI.Services;
using System.Diagnostics;

namespace PersonVehicle.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // Cargar todas las personas para mostrar en la lista
            ViewBag.Persons = await _apiService.ObtenerListaPersonasAsync();
            return View();
        }

        // ==================== PERSONAS ====================

        [HttpPost]
        public async Task<IActionResult> CreatePerson(Persons person)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Por favor complete todos los campos requeridos correctamente.";
                return RedirectToAction("Index");
            }

            var success = await _apiService.AgregarPersonaAsync(person);

            if (success)
                TempData["Success"] = "Persona creada exitosamente.";
            else
                TempData["Error"] = "Error al crear la persona. Verifique que la identificación no exista.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SearchPersonByIdentification(int identification)
        {
            if (identification <= 0)
            {
                TempData["Error"] = "Por favor ingrese una identificación válida.";
                return RedirectToAction("Index");
            }

            var person = await _apiService.ObtenerListaPersonasPorIdentificacionAsync(identification);
            
            if (person == null)
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

        public async Task<IActionResult> EditPerson(int identification)
        {
            var person = await _apiService.GetPersonByIdentificationAsync(identification);
            
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

        [HttpPost]
        public async Task<IActionResult> DeletePerson(int identification)
        {
            var success = await _apiService.EliminarPersonaAsync(identification);
            
            if (success)
            {
                TempData["Success"] = "Persona eliminada exitosamente.";
            }
            else
            {
                TempData["Error"] = "Error al eliminar la persona.";
            }

            return RedirectToAction("Index");
        }

        // ==================== VEHICULOS ====================

        public async Task<IActionResult> AllVehicles()
        {
            var vehicles = await _apiService.GetAllVehiclesAsync();
            return View(vehicles);
        }

        [HttpPost]
        public async Task<IActionResult> SearchVehicleByPlate(string plate)
        {
            if (string.IsNullOrWhiteSpace(plate))
            {
                TempData["Error"] = "Por favor ingrese una placa válida.";
                return RedirectToAction("AllVehicles");
            }

            var vehicle = await _apiService.ObtengaListaDeVehiculoPorPlaca(plate.Trim());
            
            if (vehicle == null)
            {
                TempData["Error"] = $"No se encontró ningún vehículo con la placa: {plate}";
            }
            else
            {
                TempData["VehicleResult"] = System.Text.Json.JsonSerializer.Serialize(vehicle);
                TempData["Success"] = $"Vehículo encontrado: {vehicle.Make} {vehicle.Model} ({vehicle.Year})";
            }

            return RedirectToAction("AllVehicles");
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle(string plate, string make, string model, int year, int ownerIdentification)
        {
            var vehicle = new Vehicles
            {
                Plate = plate,
                Make = make,
                Model = model,
                Year = year,
                PersonIdentification = ownerIdentification
            };

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

        public async Task<IActionResult> EditVehicle(string plate)
        {
            var vehicle = await _apiService.ObtengaListaDeVehiculoPorPlaca(plate);
            
            if (vehicle == null)
            {
                TempData["Error"] = "Vehículo no encontrado.";
                return RedirectToAction("AllVehicles");
            }

            return View(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVehicle(string plate, string make, string model, int year)
        {
            var vehicle = new Vehicles
            {
                Plate = plate,
                Make = make,
                Model = model,
                Year = year
            };

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

        [HttpPost]
        public async Task<IActionResult> DeleteVehicle(string plate)
        {
            var success = await _apiService.EliminarVehiculoAsync(plate);
            
            if (success)
            {
                TempData["Success"] = "Vehículo eliminado exitosamente.";
            }
            else
            {
                TempData["Error"] = "Error al eliminar el vehículo.";
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
