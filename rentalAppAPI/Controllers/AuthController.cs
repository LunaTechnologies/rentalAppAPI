using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rentalAppAPI.BLL.Interfaces;
using rentalAppAPI.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rentalAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        
        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _authManager.Register(model);

            switch (result)
            {
                case 1:
                    return Ok("Registered");
                case 2:
                    return BadRequest("Email already used");
                case 3:
                    return BadRequest("Username already used");
                case 0:
                    return BadRequest("Error at register");
                default:
                    return BadRequest("Error at register");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _authManager.Login(model);

            return Ok(result);
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshModel model)
        {
            var result = await _authManager.Refresh(model);

            return Ok(result);
        }

    }
}
