using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xtrades.BLL.Interfaces;
using Xtrades.BLL.Requests;
using Xtrades.BLL.Responses;
using Xtrades.DAL.Entities;

namespace Xtrades.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IMapper _mapper;

        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        [HttpGet("GetAllGroups")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Group>>>> GetAllGroups()
        {
            try
            {
                var groups = await _groupService.GetAllGroupsAsync();
                var response = new ApiResponse<IEnumerable<Group>>(true, groups);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<IEnumerable<Group>>(false, errorMessage: ex.Message);
                return StatusCode(500, response);
            }
        }

        [HttpGet("GetGroup{id}")]
        public async Task<ActionResult<ApiResponse<Group>>> GetGroupById(int id)
        {
            try
            {
                var group = await _groupService.GetGroupByIdAsync(id);
                if (group == null)
                {
                    var notFoundResponse = new ApiResponse<Group>(false, errorMessage: $"Group with ID {id} not found");
                    return NotFound(notFoundResponse);
                }

                var successResponse = new ApiResponse<Group>(true, group);
                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<Group>(false, errorMessage: ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("CreateGroup")]
        public async Task<ActionResult<ApiResponse<Group>>> CreateGroup(int userId, [FromBody] GroupRequest newGroup)
        {
            try
            {
                var group = _mapper.Map<Group>(newGroup);
                group = await _groupService.CreateGroupAsync(userId, group);
                var createdResponse = new ApiResponse<Group>(true, group);
                return CreatedAtAction(nameof(GetGroupById), new { id = group.Id }, createdResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<Group>(false, errorMessage: ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("UpdateGroup/{id}")]
        public async Task<ActionResult<ApiResponse<Group>>> UpdateGroup(int id, [FromBody] GroupRequest updatedGroup)
        {
            try
            {
                var group = _mapper.Map<Group>(updatedGroup);
                await _groupService.UpdateGroupAsync(id, group);
                var successResponse = new ApiResponse<Group>(true);
                return Ok(successResponse);
            }
            catch (InvalidOperationException ex)
            {
                var notFoundResponse = new ApiResponse<Group>(false, errorMessage: ex.Message);
                return NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<Group>(false, errorMessage: ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("DeleteGroup/{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteGroup(int id)
        {
            try
            {
                await _groupService.DeleteGroupAsync(id);
                var successResponse = new ApiResponse<object>(true);
                return Ok(successResponse);
            }
            catch (InvalidOperationException ex)
            {
                var notFoundResponse = new ApiResponse<object>(false, errorMessage: ex.Message);
                return NotFound(notFoundResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<object>(false, errorMessage: ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPost("AddForGroup/{groupId}/User/{userId}")]
        public async Task<ActionResult<ApiResponse<object>>> AddUserToGroup(int groupId, int userId)
        {
            try
            {
                await _groupService.AddUserToGroup(userId, groupId);
                var successResponse = new ApiResponse<object>(true);
                return Ok(successResponse);
            }
            catch (InvalidOperationException ex)
            {
                var badRequestResponse = new ApiResponse<object>(false, errorMessage: ex.Message);
                return BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<object>(false, errorMessage: ex.Message);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpDelete("DeleteFromGroup/{groupId}/User/{userId}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteUserFromGroup(int groupId, int userId)
        {
            try
            {
                await _groupService.DeleteUserFromGroup(userId, groupId);
                var successResponse = new ApiResponse<object>(true);
                return Ok(successResponse);
            }
            catch (InvalidOperationException ex)
            {
                var badRequestResponse = new ApiResponse<object>(false, errorMessage: ex.Message);
                return BadRequest(badRequestResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<object>(false, errorMessage: ex.Message);
                return StatusCode(500, errorResponse);
            }
        }
    }
}
