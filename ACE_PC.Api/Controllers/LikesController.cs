using ACE_PC.Domain.Interfaces.Likes;
using ACE_PC.Domain.Models.Likes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ACE_PC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {

        private readonly ILikeService _likeService;

        public LikesController(ILikeService likeService)
        {
            this._likeService = likeService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLike([FromBody] LikeCreateRequest request)
        {
            var response = await _likeService.CreateAsync(request);
            if (response.IsError)
            {
                if (response.ResponseType == Domain.Helpers.ReqResHelper.EnumResponseType.Pending)
                {
                    return StatusCode(102,response);
                }
                if (response.ResponseType == Domain.Helpers.ReqResHelper.EnumResponseType.ValidationError)
                {
                    return StatusCode(400, response);
                }
                if (response.ResponseType == Domain.Helpers.ReqResHelper.EnumResponseType.SystemError)
                {
                    return StatusCode(500, response);
                }
            }
            return Ok(response);
        }

        //ToogleLike
        [HttpPost("toogle")]
        public async Task<IActionResult> LikeToogle([FromBody]LikeCreateRequest request)
        {
            var response = await _likeService.ToogleLikeAsync(request);
            if (response.IsError)
            {
                if (response.ResponseType == Domain.Helpers.ReqResHelper.EnumResponseType.Pending)
                {
                    return StatusCode(102, response);
                }
                if (response.ResponseType == Domain.Helpers.ReqResHelper.EnumResponseType.ValidationError)
                {
                    return StatusCode(400, response);
                }
                if (response.ResponseType == Domain.Helpers.ReqResHelper.EnumResponseType.SystemError)
                {
                    return StatusCode(500, response);
                }
            }
            return Ok(response);
        }
    }
}
