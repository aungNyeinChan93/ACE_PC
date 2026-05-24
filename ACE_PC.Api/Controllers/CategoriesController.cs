using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Categories;
using ACE_PC.Domain.Models.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ClientModel.Primitives;
using System.Net.WebSockets;

namespace ACE_PC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            if (response.IsError)
            {
                if (response.ResponseType == EnumResponseType.Pending)
                {
                    return StatusCode(102, response);
                }
                if (response.ResponseType == EnumResponseType.ValidationError)
                {
                    return BadRequest(response);
                }
                if (response.ResponseType == EnumResponseType.SystemError)
                {
                    return StatusCode(500, response);
                }
            }

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBYId(int id)
        {
            var response = await _categoryService.GetOneByIdAsync(id);
            if (response.IsError)
            {
                if (response.ResponseType == EnumResponseType.Pending)
                {
                    return StatusCode(102, response);
                }
                if (response.ResponseType == EnumResponseType.ValidationError)
                {
                    return BadRequest(response);
                }
                if (response.ResponseType == EnumResponseType.SystemError)
                {
                    return StatusCode(500, response);
                }
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var response = await _categoryService.CreateCategory(request);
            if (response.IsError)
            {
                if (response.ResponseType == EnumResponseType.Pending)
                {
                    return StatusCode(102, response);
                }
                if (response.ResponseType == EnumResponseType.ValidationError)
                {
                    return BadRequest(response);
                }
                if (response.ResponseType == EnumResponseType.SystemError)
                {
                    return StatusCode(500, response);
                }
            }
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _categoryService.DeleteCategoryAsync(id);
            if (response.IsError)
            {
                if (response.ResponseType == EnumResponseType.Pending)
                {
                    return StatusCode(102, response);
                }
                if (response.ResponseType == EnumResponseType.ValidationError)
                {
                    return BadRequest(response);
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
