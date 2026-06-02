using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Users;
using ACE_PC.Domain.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;

namespace ACE_PC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        //GetByEmail
        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var response = await _userService.GetUserByEmail(email);
            if (response.IsError)
            {
                if (response.ResponseType == EnumResponseType.Pending)
                {
                    return StatusCode(102, response);
                }
                if (response.ResponseType == EnumResponseType.ValidationError)
                {
                    return StatusCode(400, response);
                }
                if (response.ResponseType == EnumResponseType.SystemError)
                {
                    return StatusCode(500,response);
                }
            }
            return Ok(response);
        }


        //GetAllUsers
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUses();
            if (response.IsError)
            {
                if (response.ResponseType == EnumResponseType.Pending)
                {
                    return StatusCode(102, response);
                }
                if (response.ResponseType == EnumResponseType.ValidationError)
                {
                    return StatusCode(400, response);
                }
                if (response.ResponseType == EnumResponseType.SystemError)
                {
                    return StatusCode(500, response);
                }
            }
            return Ok(response);
        }


        //GetAllUsersWithPagination
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery]UserPaginationRequest paginationRequest)
        {
            var response = await _userService.GetAllUses(paginationRequest);
            if (response.IsError)
            {
                if (response.ResponseType == EnumResponseType.Pending)
                {
                    return StatusCode(102, response);
                }
                if (response.ResponseType == EnumResponseType.ValidationError)
                {
                    return StatusCode(400, response);
                }
                if (response.ResponseType == EnumResponseType.SystemError)
                {
                    return StatusCode(500, response);
                }
            }
            return Ok(response);
        }

        //GetAllUsersWithPagination
        [HttpGet("search")]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserPaginationRequest paginationRequest, [FromQuery]UserSearchRequest request )
        {
            var response = await _userService.GetAllUses(paginationRequest,request);
            if (response.IsError)
            {
                if (response.ResponseType == EnumResponseType.Pending)
                {
                    return StatusCode(102, response);
                }
                if (response.ResponseType == EnumResponseType.ValidationError)
                {
                    return StatusCode(400, response);
                }
                if (response.ResponseType == EnumResponseType.SystemError)
                {
                    return StatusCode(500, response);
                }
            }
            return Ok(response);
        }

        //GetUserById
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById([FromRoute]int id)
        {
            var response = await _userService.GetUserById(id);
            if (response.IsError)
            {
                if (response.ResponseType == EnumResponseType.Pending)
                {
                    return StatusCode(102, response);
                }
                if (response.ResponseType == EnumResponseType.ValidationError)
                {
                    return StatusCode(400, response);
                }
                if (response.ResponseType == EnumResponseType.SystemError)
                {
                    return StatusCode(500, response);
                }
            }
            return Ok(response);
        }

        //DeleteUser
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var response = await _userService.DeleteAsync(id);
            if (response.IsError)
            {
                if (response.ResponseType == EnumResponseType.Pending)
                {
                    return StatusCode(102, response);
                }
                if (response.ResponseType == EnumResponseType.ValidationError)
                {
                    return StatusCode(400, response);
                }
                if (response.ResponseType == EnumResponseType.SystemError)
                {
                    return StatusCode(500, response);
                }
            }
            return Ok(response);
        }
    }
}
