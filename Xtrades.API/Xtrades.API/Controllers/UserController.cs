using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xtrades.BLL.Interfaces;
using Xtrades.BLL.Requests;
using Xtrades.BLL.Responses;
using Xtrades.DAL.Entities;

namespace Xtrades.API.Controllers
{

    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                var response = new ApiResponse<IEnumerable<User>>(true, users);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IEnumerable<User>>(false, errorMessage: ex.Message);
                return StatusCode(500, response);
            }
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    var notFoundResponse = new ApiResponse<User>(false, errorMessage: $"User with ID {id} not found");
                    return NotFound(notFoundResponse);
                }

                var successResponse = new ApiResponse<User>(true, user);
                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<User>(false, errorMessage: ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest newUser)
        {
            try
            {
                // Перевірка формату телефону
                if (!Regex.IsMatch(newUser.Phone, @"^380\d{9}$"))
                {
                    var errorResponse = new ApiResponse<User>(false, errorMessage: "Invalid phone format. Should be 380XXXXXXXXX");
                    return BadRequest(errorResponse);
                }

                // Перевірка формату електронної пошти
                if (!Regex.IsMatch(newUser.Email, @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$"))
                {
                    var errorResponse = new ApiResponse<User>(false, errorMessage: "Invalid email format.");
                    return BadRequest(errorResponse);
                }
                var user = _mapper.Map<User>(newUser);

                await _userService.CreateUserAsync(user);
                var createdResponse = new ApiResponse<User>(true, user);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, createdResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<User>(false, errorMessage: ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRequest updatedUser)
        {
            try
            {
                // Перевірка формату телефону
                if (!Regex.IsMatch(updatedUser.Phone, @"^380\d{9}$"))
                {
                    var errorResponse = new ApiResponse<object>(false, errorMessage: "Invalid phone format. Should be 380XXXXXXXXX");
                    return BadRequest(errorResponse);
                }

                // Перевірка формату електронної пошти
                if (!Regex.IsMatch(updatedUser.Email, @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$"))
                {
                    var errorResponse = new ApiResponse<object>(false, errorMessage: "Invalid email format.");
                    return BadRequest(errorResponse);
                }
                var user = _mapper.Map<User>(updatedUser);

                await _userService.UpdateUserAsync(id, user);
                var successResponse = new ApiResponse<object>(true);
                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<object>(false, errorMessage: ex.Message);
                return StatusCode(500, errorResponse);
            }
        }


        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                var successResponse = new ApiResponse<object>(true);
                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<object>(false, errorMessage: ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

    }


}
