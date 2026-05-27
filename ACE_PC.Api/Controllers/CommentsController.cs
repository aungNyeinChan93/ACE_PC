using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Comments;
using ACE_PC.Domain.Models.Comments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ACE_PC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }


        //Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateCommentRequest request)
        {
            var response = await _commentService.CreateAsync(request);
            if (response.IsError)
            {
                if (response.ResponseType == EnumResponseType.ValidationError)
                {
                    return StatusCode(400,response);
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
