using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rentalAppAPI.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rentalAppAPI.BLL.Models;
using rentalAppAPI.DAL.Models;

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
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userManager.GetAllUsers());
        }

        [Authorize("Admin")]
        [HttpDelete("removeUser")]
        public async Task<IActionResult> RemoveUser(String username)
        { 
            Boolean result = await _userManager.removeUser(username);
            if (result == true)
            {
                return Ok("success");
            }
            else
            {
                return BadRequest("Username does not exist");
            }
        }

        [HttpGet("emailExist")]
        public async Task<IActionResult> EmailExist(String email)
        {
            Boolean result = await _userManager.emailExist(email);
            if (result == false)
            {
                return Ok("false");
            }
            else
            {
                return Ok("true");
            }
        }

        [HttpGet("usernameExist")]
        public async Task<IActionResult> UsernameExist(String username)
        {
            Boolean result = await _userManager.usernameExist(username);
            if (result == false)
            {
                return Ok("false");
            }
            else
            {
                return Ok("true");
            }
        }

    }
}
