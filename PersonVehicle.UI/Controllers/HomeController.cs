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
            // Validación de identificación
            if (person.Identification <= 0)
            {
                TempData["Error"] = "La identificación debe ser mayor a 0.";
                return RedirectToAction("Index");
            }

            if (person.Identification.ToString().Length > 9)
            {
                TempData["Error"] = "La identificación no puede tener más de 9 dígitos.";
                return RedirectToAction("Index");
            }

            // Validación de nombre
            if (string.IsNullOrWhiteSpace(person.FirstName))
            {
                TempData["Error"] = "El nombre es requerido.";
                return RedirectToAction("Index");
            }

            if (person.FirstName.Length > 50)
            {
                TempData["Error"] = "El nombre no puede tener más de 50 caracteres.";
                return RedirectToAction("Index");
            }

            // Validar que el nombre solo contenga letras
            if (!System.Text.RegularExpressions.Regex.IsMatch(person.FirstName, @"^[A-Za-zÀ-ÿ\s]+$"))
            {
                TempData["Error"] = "El nombre solo puede contener letras y espacios.";
                return RedirectToAction("Index");
            }

            // Validación de apellido
            if (string.IsNullOrWhiteSpace(person.LastName))
            {
                TempData["Error"] = "El apellido es requerido.";
                return RedirectToAction("Index");
            }

            if (person.LastName.Length > 50)
            {
                TempData["Error"] = "El apellido no puede tener más de 50 caracteres.";
                return RedirectToAction("Index");
            }

            // Validar que el apellido solo contenga letras
            if (!System.Text.RegularExpressions.Regex.IsMatch(person.LastName, @"^[A-Za-zÀ-ÿ\s]+$"))
            {
                TempData["Error"] = "El apellido solo puede contener letras y espacios.";
                return RedirectToAction("Index");
            }

            // Validación de email
            if (string.IsNullOrWhiteSpace(person.Email))
            {
                TempData["Error"] = "El email es requerido.";
                return RedirectToAction("Index");
            }

            if (person.Email.Length > 100)
            {
                TempData["Error"] = "El email no puede tener más de 100 caracteres.";
                return RedirectToAction("Index");
            }

            // Validar formato de email
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
            if (!emailRegex.IsMatch(person.Email))
            {
                TempData["Error"] = "Por favor ingrese un email válido.";
                return RedirectToAction("Index");
            }

            // Validación de teléfono (8 dígitos)
            if (person.Phone <= 0)
            {
                TempData["Error"] = "El teléfono es requerido.";
                return RedirectToAction("Index");
            }

            var phoneString = person.Phone.ToString();
            if (phoneString.Length != 8)
            {
                TempData["Error"] = "El teléfono debe tener exactamente 8 dígitos.";
                return RedirectToAction("Index");
            }

            // Validación de salario
            if (person.Salario <= 0)
            {
                TempData["Error"] = "El salario debe ser mayor a 0.";
                return RedirectToAction("Index");
            }

            // Normalizar datos
            person.FirstName = person.FirstName.Trim();
            person.LastName = person.LastName.Trim();
            person.Email = person.Email.Trim().ToLower();

            var (success, message) = await _apiService.AgregarPersonaAsync(person);

            if (success)
            {
                TempData["Success"] = $"Persona {person.FullName} con identificación {person.Identification} creada exitosamente.";
            }
            else
            {
                TempData["Error"] = message ?? "No fue posible registrar la persona. Verifique que la identificación no exista ya en el sistema.";
            }

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
            // Validaciones de entrada
            if (string.IsNullOrWhiteSpace(plate))
            {
                TempData["Error"] = "La placa del vehículo es requerida.";
                return RedirectToAction("AllVehicles");
            }

            // Normalizar placa
            plate = plate.Trim().ToUpper();

            // Validar formato de placa: AAA-123 (3 letras, guion, 3 números)
            var plateRegex = new System.Text.RegularExpressions.Regex(@"^[A-Z]{3}-[0-9]{3}$");
            if (!plateRegex.IsMatch(plate))
            {
                TempData["Error"] = "La placa debe seguir el formato ABC-123 (3 letras, guion, 3 números).";
                return RedirectToAction("AllVehicles");
            }

            if (string.IsNullOrWhiteSpace(make))
            {
                TempData["Error"] = "La marca del vehículo es requerida.";
                return RedirectToAction("AllVehicles");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                TempData["Error"] = "El modelo del vehículo es requerido.";
                return RedirectToAction("AllVehicles");
            }

            if (year < 1900 || year > 2030)
            {
                TempData["Error"] = "El año debe estar entre 1900 y 2030.";
                return RedirectToAction("AllVehicles");
            }

            if (ownerIdentification <= 0)
            {
                TempData["Error"] = "La identificación del propietario debe ser mayor a 0.";
                return RedirectToAction("AllVehicles");
            }

            if (ownerIdentification.ToString().Length > 9)
            {
                TempData["Error"] = "La identificación del propietario no puede tener más de 9 dígitos.";
                return RedirectToAction("AllVehicles");
            }

            // Verificar si la persona existe antes de crear el vehículo
            var personExists = await _apiService.GetPersonByIdentificationAsync(ownerIdentification);
            if (personExists == null)
            {
                TempData["Error"] = $"No se encontró ninguna persona con la identificación {ownerIdentification}. Por favor verifique que la persona esté registrada en el sistema.";
                return RedirectToAction("AllVehicles");
            }

            var vehicle = new Vehicles
            {
                Plate = plate,
                Make = make.Trim(),
                Model = model.Trim(),
                Year = year,
                PersonIdentification = ownerIdentification
            };

            var success = await _apiService.AgregueNuevoVehiculo(vehicle);
            
            if (success)
            {
                TempData["Success"] = $"Vehículo {vehicle.Make} {vehicle.Model} con placa {vehicle.Plate} creado exitosamente y asignado a {personExists.FullName}.";
            }
            else
            {
                TempData["Error"] = "Error al crear el vehículo. Verifique que la placa no exista ya en el sistema.";
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

            return RedirectToAction("EditVehicle", new { plate = plate });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVehicleOwner(string plate, int newOwnerIdentification)
        {
            // Validación: identificación no puede ser 0 o negativa
            if (newOwnerIdentification <= 0)
            {
                TempData["Error"] = "Por favor ingrese una identificación válida mayor a 0.";
                return RedirectToAction("EditVehicle", new { plate = plate });
            }

            // Validación: identificación debe tener máximo 9 dígitos
            if (newOwnerIdentification.ToString().Length > 9)
            {
                TempData["Error"] = "La identificación no puede tener más de 9 dígitos.";
                return RedirectToAction("EditVehicle", new { plate = plate });
            }

            // Verificar si la persona existe en el sistema antes de intentar cambiar el propietario
            var personExists = await _apiService.GetPersonByIdentificationAsync(newOwnerIdentification);
            if (personExists == null)
            {
                TempData["Error"] = $"No se encontró ninguna persona con la identificación {newOwnerIdentification}. Por favor verifique que la persona esté registrada en el sistema.";
                return RedirectToAction("EditVehicle", new { plate = plate });
            }

            // Intentar cambiar el propietario
            var (success, message) = await _apiService.EditarVehiculoPropietarioAsync(plate, newOwnerIdentification);
            
            if (success)
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction("EditVehicle", new { plate = plate });
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
