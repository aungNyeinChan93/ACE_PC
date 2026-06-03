using ACE_PC.Database.Migrations;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Quotes;
using ACE_PC.Domain.Models.Quotes;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ClientModel.Primitives;

namespace ACE_PC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuotesController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }


        //create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQuoteRequest request)
        {
            var response = await _quoteService.CreateAsync(request);
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


        //all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _quoteService.GetAllAsync();

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


        //with pagination
        [HttpGet]
        public async Task<IActionResult> GetAllByPagination([FromQuery] QuotePaginationRequest request)
        {
            var response = await _quoteService.GetAllAsync(request ?? null);

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

        // search
        [HttpGet("search")]
        public async Task<IActionResult> GetAllQuoteWithSearch(
            [FromQuery] QuotePaginationRequest paginationRequest,
            [FromQuery] QuoteSearchRequest searchRequest)
        {
            var response = await _quoteService.GetAllAsync(paginationRequest,searchRequest);

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

        //update
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute]int id,UpdateQuoteRequest request)
        {
            var response = await _quoteService.UpdateAsync(id,request);
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

        //delete
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var response = await _quoteService.DeleteAsync(id);
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

        //GetById
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _quoteService.GetByIdAsync(id);
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

        //GetByAuthorId
        [HttpGet("author/{id:int}")]
        public async Task<IActionResult> GetByAuthorId([FromRoute]int id, [FromQuery]QuotePaginationRequest pagination)
        {
            var response = await _quoteService.GetByAuthorIdAsync(id, pagination);
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
