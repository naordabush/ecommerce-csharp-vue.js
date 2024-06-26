﻿using Ecommerce.API.DTO;
using Ecommerce.API.Services.Classes;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _userService;

        public UsersController(UsersService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<READ_USER_DTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<READ_USER_DTO>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<WRITE_USER_DTO>> Register(WRITE_USER_DTO userDto)
        {
            try
            {
                var userAdded = await _userService.register(userDto);
                if (userAdded)
                {
                    return Ok(userDto); // User added successfully
                }
                else
                {
                    return BadRequest($"Username: \"{userDto.USER_USERNAME}\" is already exists");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add user: {ex.Message} Inner Exception: {ex.InnerException?.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<READ_USER_DTO>> UpdateUser(READ_USER_DTO updatedUserDTO)
        {
            try
            {
                await _userService.UpdateUser(updatedUserDTO);
                return Ok(updatedUserDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update user: {ex.Message} Inner Exception: {ex.InnerException?.Message}");
            }
        }

        [HttpDelete("deletebyid/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete user: {ex.Message} Inner Exception: {ex.InnerException?.Message}");
            }
        }

        [HttpGet("getbyusername/{username}")]
        public async Task<ActionResult<READ_USER_DTO>> FindUserByUsername(string username)
        {
            var user = await _userService.FindUserByUsername(username);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LOGIN_REQUEST_DTO loginRequest)
        {
            var (success, token) = await _userService.Login(loginRequest.Username, loginRequest.Password);

            if (success)
            {
                return Ok(token);
            }
            else
            {
                return BadRequest("Username or password is incorrect");
            }
        }
    }
}
