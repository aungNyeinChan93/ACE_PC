using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Auth;
using ACE_PC.Domain.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ACE_PC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        //Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
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
