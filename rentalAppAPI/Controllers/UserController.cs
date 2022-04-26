using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rentalAppAPI.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rentalAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [Authorize("Admin")]
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> getAllUsers()
        {
            return Ok(await _userManager.GetAllUsers());
        }

    }
}
