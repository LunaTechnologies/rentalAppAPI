using Microsoft.AspNetCore.Mvc;
using rentalAppAPI.BLL.Interfaces;
using rentalAppAPI.DAL.Models;

namespace rentalAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalTypeController : ControllerBase
    {
        private readonly IRentalTypeManager _rentalTypeManager;
        public RentalTypeController(IRentalTypeManager rentalTypeManager)
        {
            _rentalTypeManager = rentalTypeManager;
        }

        [HttpPost("CreateRentalType")]
        public async Task<IActionResult> CreateRentalType(RentalTypeModel rentalTypeModel)
        {
            int result = await _rentalTypeManager.CreateRentalType(rentalTypeModel);

            if (result == 1)
                return Ok("Succes");
            else if (result == 0)
                return BadRequest("Rental Type already exists");
            else
                return BadRequest("Error");

        }

        [HttpDelete("DeleteRentalType")]
        public async Task<IActionResult> DeleteRentalTypeById(int id)
        {
            bool result = await _rentalTypeManager.DeleteRentalTypeById(id);
            if (result == true)
            {
                return Ok("success");
            }
            else
            {
                return BadRequest("RentalType doesn't exist");
            }
        }

        [HttpGet("GetAllRentalTypes")]
        public async Task<IActionResult> GetAllRentalTypes( )
        {
            var result = await _rentalTypeManager.GetAllRentalTypes();
            return Ok(result);
        }

        [HttpPut("UpdateRentalType")]
        public async Task<IActionResult> UpdateRentalTypeById(int id, string newType)
        {
            var result = await _rentalTypeManager.UpdateRentalTypeById(id, newType);
            if (result == true)
            {
                return Ok("succes");
            }
            return BadRequest("No rentalType to update");
        }
    }
}
