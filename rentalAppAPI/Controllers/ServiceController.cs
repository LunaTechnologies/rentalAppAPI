﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
        
        [Authorize("User")]
        [HttpPost("CreateService")]
        [RequestSizeLimit(80 * 1024 * 1024)] // 80MB limit/request
        public async Task<IActionResult> CreateService(ICollection<IFormFile> pictures, [FromForm]ServiceModelCreate serviceModel)
        {
            string userName = User.FindFirst(ClaimTypes.Name)?.Value;
            string result = await _serviceManager.CreateService(pictures, serviceModel, userName);
            if (result.Length == 15)
                return Ok(result);
        
            if (result == "format not accepted")
                return BadRequest(result);
            if (result == "Image(s) too large (6MB/picture limit)")
                return BadRequest(result);
            if (result == "minimum number of pictures is 1")
                return BadRequest(result);
            
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

        [HttpGet("SearchServices")]
        public async Task<IActionResult> SearchServices(string Title)
        {
            if (Title.Length < 3)
            {
                return BadRequest("The service must contain least 3 characters");
            }
            List<ThumbnailServiceModel> thumbnailServiceModels = await _serviceManager.SearchServices(Title);
            if (thumbnailServiceModels.Count > 0)
            {
                return Ok(thumbnailServiceModels);
            }
            else
            {
                return BadRequest("No items found");
            }   
        }
        
        [HttpGet("RandomServices")]
        public async Task<IActionResult> SearchServices(int NumberOfServices)
        {
            if (NumberOfServices > 10 || NumberOfServices < 1)
            {
                return BadRequest("Number of services returned must be between 1 and 10");
            }
            List<ThumbnailServiceModel> thumbnailServiceModels = await _serviceManager.RandomServices(NumberOfServices);
            if (thumbnailServiceModels.Count > 0)
            {
                return Ok(thumbnailServiceModels);
            }
            else
            {
                return BadRequest("No items found");
            }
        }
    }
}