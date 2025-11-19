using Microsoft.AspNetCore.Mvc;
using PersonVehicleApi.BL;
using PersonVehicleApi.Model.Dtos;

namespace PersonVehicleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly OwnersBL _ownersBL;

        public OwnersController(OwnersBL ownersBL)
        {
            _ownersBL = ownersBL;
        }

        // GET: api/owners
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _ownersBL.GetAllVehiclesWithOwnerAsync();
            return Ok(data);
        }

        // GET: api/owners/{plate}
        [HttpGet("{plate}")]
        public async Task<IActionResult> GetByPlate(string plate)
        {
            var vehicle = await _ownersBL.GetVehicleWithOwnerAsync(plate);

            if (vehicle == null)
                return NotFound(new { Message = "Vehicle not found" });

            return Ok(vehicle);
        }

        // PUT: api/owners/{plate}/change-owner
        [HttpPut("{plate}/change-owner")]
        public async Task<IActionResult> ChangeOwner(string plate, [FromBody] ChangeOwnerDto dto)
        {
            var result = await _ownersBL.UpdateVehicleOwnerAsync(plate, dto.NewOwnerIdentification);

            if (!result.Success)
                return BadRequest(new { Message = result.Message });

            return Ok(new { Message = result.Message });
        }
    }
}

