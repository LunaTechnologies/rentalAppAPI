using Microsoft.AspNetCore.Mvc;
using rentalAppAPI.BLL.Interfaces;
using rentalAppAPI.DAL.Models;

namespace rentalAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public ServiceController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("CreateService")]
        public async Task<IActionResult> CreateService(ServiceModel serviceModel)
        {
            string result = await _serviceManager.CreateService(serviceModel);

            if (result.Length == 15)
                return Ok(result);

            return BadRequest("Error at creating");

        }

        [HttpDelete("DeteleService")]
        public async Task<IActionResult> DeleteServiceByIdentificationString(string IdentificationString)
        {
            bool result = await _serviceManager.DeleteServiceByIdentificationString(IdentificationString);
            if (result == true)
            {
                return Ok("success");
            }
            else
            {
                return BadRequest("Service doesn't exist");
            }
        }

        [HttpGet("GetServiceByIdentificationString")]
        public async Task<IActionResult> GetServiceByIdentificationString(string IdentificationString)
        {
            var result = await _serviceManager.GetServiceByIdentificationString(IdentificationString);
            if (result.Username != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Service doesn't exist");
            }
        }
    }
}