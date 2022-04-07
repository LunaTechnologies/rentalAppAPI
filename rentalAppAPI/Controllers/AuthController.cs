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

            if (result)
                return Ok("Registered");
            else return BadRequest("Not registered");
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
