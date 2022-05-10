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
        public async Task<IActionResult> RemoveUser(UserNameModel username)
        { 
            Boolean result = await _userManager.removeUser(username.userName);
            if (result == true)
            {
                return Ok("Successfully removed : " + username.userName);
            }
            else
            {
                return BadRequest("Username does not exist");
            }
        }

    }
}
