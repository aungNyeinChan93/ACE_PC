using ACE_PC.Database.Data;
using ACE_PC.Domain.Dtos.Quotes;
using ACE_PC.Domain.Entity;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Quotes;
using ACE_PC.Domain.Models.Quotes;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.BL.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly AppDbContext _context;

        public QuoteService(AppDbContext context)
        {
            _context = context;
        }

        //CreateAsync
        public async Task<ResultModel<CreateQuoteResponse>> CreateAsync(CreateQuoteRequest request)
        {
            var responseModel = new ResultModel<CreateQuoteResponse>();

            var newQuote = new Quote
            {
                Title = request.Title,
                Content = request.Content,
                CategoryId = request.CategoryId,
                UserId = request.UserId,
            };

            await _context.Quotes.AddAsync(newQuote);
            var result = await _context.SaveChangesAsync();

            responseModel = result <= 0
                ? ResultModel<CreateQuoteResponse>.SystemError(500, "Create Quote Fail!")
                : ResultModel<CreateQuoteResponse>
                .Success(200, "Create Success", new CreateQuoteResponse { Quote = newQuote });

            return responseModel;
        }


        //GetAll
        public async Task<ResultModel<QuotesResponse>> GetAllAsync()
        {
            var responseModel = new ResultModel<QuotesResponse>();

            var quotes = await _context.Quotes
                .Include(q => q.User)!
                .Include(q => q.Category)!
                .Include(q => q.Likes)!.ThenInclude(l => l.User)!
                .Include(q => q.Comments)!.ThenInclude(c => c.User)
                //.AsQueryable()
                .Select(q => new QuotesDto
                {
                    Id = q.QuoteId,
                    Title = q.Title,
                    Content = q.Content,
                    Author = q.User!.Name,
                    Category = q.Category!.Name,
                    CreatedAt = q.CreatedAt,
                    Likes = q.Likes!.Select(l => new LikeDto
                    {
                        UserId = l.UserId,
                        UserName = l.User!.Name
                    }).ToList(),
                    Comments = q.Comments!
                    .Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        Content = c.Body,
                        UserName = c.User!.Name
                    })
                    .OrderByDescending(c => c.Id) // or c.CreatedOn
                    .ToList()
                })
                .OrderByDescending(q=>q.CreatedAt)
                .ToListAsync();


            if (quotes is null || quotes.Count < 0)
            {
                responseModel = ResultModel<QuotesResponse>.ValidationError(400, "Quotes Not Found!");
                goto skip;
            }

            var data = new QuotesResponse
            {
                Quotes = quotes
            };

            responseModel = ResultModel<QuotesResponse>.Success(200, "Get All Quotes", data);
        skip:
            return responseModel;
        }


        //GetAllAsync (Pagination)
        public async Task<ResultModel<QuotesResponse>> GetAllAsync(QuotePaginationRequest request)
        {
            var responseModel = new ResultModel<QuotesResponse>();


            //pagination
            var totalQuotes = await _context.Quotes.CountAsync();
            var pageCount = request.PageCount;
            var pageNumber = request.PageNumber;
            var skip = (pageNumber - 1) * pageCount;
            var totalPage = (int)Math.Ceiling(totalQuotes / (double)pageCount);

            var paginationResult = new QuotesPaginationResult
            {
                ItemCount = totalQuotes,
                PageCount = pageCount,
                PageNumber = pageNumber,
                TotalPage = totalPage,
            };

            var quotes = await _context.Quotes
                .Include(q => q.User)!
                .Include(q => q.Category)!
                .Include(q => q.Likes)!.ThenInclude(l => l.User)!
                .Include(q => q.Comments)!.ThenInclude(c => c.User)
                .Skip(skip)
                .Take(pageCount)
                .Select(q => new QuotesDto
                {
                    Id = q.QuoteId,
                    Content = q.Content,
                    Title = q.Title,
                    Author = q.User!.Name,
                    Category = q.Category!.Name,
                    CreatedAt = q.CreatedAt,
                    Likes = q.Likes!.Select(l => new LikeDto
                    {
                        UserId = l.UserId,
                        UserName = l.User!.Name
                    }).ToList(),
                    Comments = q.Comments!
                    .OrderByDescending(c => c.CommentId)
                    .Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        Content = c.Body,
                        UserName = c.User!.Name
                    }).ToList()
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();


            if (quotes is null || quotes.Count < 0)
            {
                responseModel = ResultModel<QuotesResponse>.ValidationError(400, "Quotes Not Found!");
                goto skip;
            }

            var data = new QuotesResponse
            {
                Quotes = quotes,
                PaginationResult = paginationResult
            };

            responseModel = ResultModel<QuotesResponse>.Success(200, "Get All Quotes", data);
        skip:
            return responseModel;
        }


        //GetAllAsync (pagination ,Search)
        public async Task<ResultModel<QuotesResponse>> GetAllAsync(
            [FromQuery] QuotePaginationRequest paginationRequest,
            [FromQuery] QuoteSearchRequest searchRequest)
        {
            var responseModel = new ResultModel<QuotesResponse>();

            // Ensure we provide default instantiation if query didn't populate them
            paginationRequest ??= new QuotePaginationRequest();
            searchRequest ??= new QuoteSearchRequest();


            //pagination
            var totalQuotes = await _context.Quotes.CountAsync();
            var pageCount = paginationRequest.PageCount;
            var pageNumber = paginationRequest.PageNumber;
            var skip = (pageNumber - 1) * pageCount;
            var totalPage = (int)Math.Ceiling(totalQuotes / (double)pageCount);

            var paginationResult = new QuotesPaginationResult
            {
                ItemCount = totalQuotes,
                PageCount = pageCount,
                PageNumber = pageNumber,
                TotalPage = totalPage,
            };


            // query
            var query = _context.Quotes
                .Include(q => q.Category)
                .Include(q => q.User)
                .Include(q => q.Likes)!.ThenInclude(l => l.User)
                .Include(q => q.Comments)!.ThenInclude(c => c.User)
                .AsQueryable();

            //if (searchRequest.QuoteTitle is not null)
            //{
            //    query = query.Where(q => q.Title.Contains(searchRequest.QuoteTitle));
            //}
            //if (searchRequest.AuthorName is not null)
            //{
            //    query = query.Where(q => q.User!.Name.Contains(searchRequest.AuthorName));
            //}

            if (!string.IsNullOrWhiteSpace(searchRequest.QuoteTitle) || !string.IsNullOrWhiteSpace(searchRequest.AuthorName))
            {
                query = query.Where(q =>
                    (searchRequest.QuoteTitle != null && q.Title.Contains(searchRequest.QuoteTitle)) ||
                    (searchRequest.AuthorName != null && q.User!.Name.Contains(searchRequest.AuthorName))
                );
            }

            if (searchRequest.AuthorId >= 1)
            {
                query = query.Where(q => q.User!.UserId == searchRequest.AuthorId);
            }

            if (searchRequest.CategoryName is not null)
            {
                query = query.Where(q => q.Category!.Name.Contains(searchRequest.CategoryName));
            }


            if (searchRequest.CategoryId >= 1)
            {
                query = query.Where(q => q.CategoryId == searchRequest.CategoryId);
            }

            var quotes = await query
                .Skip(skip)
                .Take(pageCount)
                .Select(q => new QuotesDto
                {
                    Id = q.QuoteId,
                    Title = q.Title,
                    Content = q.Content,
                    Author = q.User!.Name,
                    Category = q.Category!.Name,
                    CreatedAt = q.CreatedAt,
                    Likes = q.Likes!.Select(l => new LikeDto
                    {
                        UserId = l.UserId,
                        UserName = l.User!.Name
                    }).ToList(),
                    Comments = q.Comments!
                    .OrderByDescending(c => c.CommentId) // or c.CreatedOn
                    .Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        Content = c.Body,
                        UserName = c.User!.Name
                    }).ToList()
                })
                .OrderByDescending(q=>q.CreatedAt)
                .ToListAsync();

            //if (quotes is null || quotes.Count <= 0)
            //{
            //    responseModel = ResultModel<QuotesResponse>.ValidationError(400, "Quotes Not Found!");
            //    goto skip;
            //}

            var data = new QuotesResponse
            {
                Quotes = quotes,
                PaginationResult = paginationResult
            };

            responseModel = ResultModel<QuotesResponse>.Success(200, "Get All Quotes", data);
            //skip:
            return responseModel;
        }


        //Update Quote
        public async Task<ResultModel<UpdateQuoteResponse>> UpdateAsync(int id, UpdateQuoteRequest request)
        {
            var responseModel = new ResultModel<UpdateQuoteResponse>();

            var updateQuote = await _context.Quotes.AsNoTracking()
                .FirstOrDefaultAsync(q => q.QuoteId == id);

            if (updateQuote is null)
            {
                responseModel = ResultModel<UpdateQuoteResponse>.ValidationError(400, "Update Quote not found!");
                goto skip;
            }

            updateQuote.Title = request.Title;
            updateQuote.Content = request.Content;
            updateQuote.CategoryId = request.CategoryId;
            //updateQuote.User = request.User;
            //updateQuote.Category = request.Category;


            _context.Entry(updateQuote).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();

            responseModel = result >= 1
                ? ResultModel<UpdateQuoteResponse>
                .Success(200, "Update Success", new UpdateQuoteResponse { Quote = updateQuote })
                : ResultModel<UpdateQuoteResponse>.SystemError(500, "Update Fail");

        skip:
            return responseModel;

        }

        // Delete
        public async Task<ResultModel<DeleteQuoteResponse>> DeleteAsync(int id)
        {
            var responseModel = new ResultModel<DeleteQuoteResponse>();
            var quote = await _context.Quotes.AsNoTracking().FirstOrDefaultAsync(q => q.QuoteId == id);

            if (quote is null)
            {
                responseModel = ResultModel<DeleteQuoteResponse>.ValidationError(400, "Quote Not Found!");
                goto skip;
            }

            _context.Quotes.Remove(quote);

            var result = await _context.SaveChangesAsync();

            responseModel = result >= 1
                ? ResultModel<DeleteQuoteResponse>
                .Success(200, "Delete Success", new DeleteQuoteResponse { Quote = quote })
                : ResultModel<DeleteQuoteResponse>.SystemError(500, "Delete Fail");

        skip:
            return responseModel;

        }


        //GetByID
        public async Task<ResultModel<QuoteResponse>> GetByIdAsync(int id)
        {
            var responseModel = new ResultModel<QuoteResponse>();
            var quote = await _context.Quotes.AsNoTracking()
                .Include(q => q.Category)
                .Include(q => q.User)
                .Include(q => q.Comments)!.ThenInclude(c => c.User)
                .Include(q => q.Likes)!.ThenInclude(l => l.User)
                .Where(q => q.QuoteId == id)
                .Select(q => new QuotesDto
                {
                    Id = q.QuoteId,
                    Title = q.Title,
                    Content = q.Content,
                    Author = q.User!.Name,
                    Category = q.Category!.Name,
                    AuthorId = q.UserId,
                    CategoryId = q.CategoryId,
                    CreatedAt = q.CreatedAt,
                    Likes = q.Likes!.Select(l => new LikeDto
                    {
                        UserId = l.UserId,
                        UserName = l.User!.Name
                    }).ToList(),
                    Comments = q.Comments!
                    .OrderByDescending(c => c.CommentId) // or c.CreatedOn
                    .Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        Content = c.Body,
                        UserName = c.User!.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (quote is null)
            {
                responseModel = ResultModel<QuoteResponse>.ValidationError(400, "Quote Not Found!");
                goto skip;
            }


            var data = new QuoteResponse
            {
                QuoteDto = quote!
            };

            responseModel = ResultModel<QuoteResponse>.Success(200, "Get Quote", data);

        skip:
            return responseModel;

        }

        //GetByAuthorIdAsync
        public async Task<ResultModel<QuotesResponse>> GetByAuthorIdAsync(int id, QuotePaginationRequest? pagination = null)
        {
            var responseModel = new ResultModel<QuotesResponse>();

            //pagination
            var pageNumber = pagination?.PageNumber ?? 1;
            var pageCount = pagination?.PageCount ?? 10;
            var skip = (pageNumber - 1) * pageCount;
            var itemCount = await _context.Quotes
                            .Include(q => q.User)
                            .Where(q => q.UserId == id)
                            .CountAsync();

            var quotes = await _context.Quotes.AsNoTracking()
                .Include(q => q.Category)
                .Include(q => q.User)
                .Include(q => q.Comments)!.ThenInclude(c => c.User)
                .Include(q => q.Likes)!.ThenInclude(l => l.User)
                .Where(q => q.UserId == id)
                .Select(q => new QuotesDto
                {
                    Id = q.QuoteId,
                    Title = q.Title,
                    Content = q.Content,
                    Author = q.User!.Name,
                    Category = q.Category!.Name,
                    AuthorId = q.UserId,
                    CategoryId = q.CategoryId,
                    CreatedAt = q.CreatedAt,
                    Likes = q.Likes!.Select(l => new LikeDto
                    {
                        UserId = l.UserId,
                        UserName = l.User!.Name
                    }).ToList(),
                    Comments = q.Comments!
                    .OrderByDescending(c => c.CommentId) // or c.CreatedOn
                    .Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        Content = c.Body,
                        UserName = c.User!.Name
                    }).ToList()
                })
                .Skip(skip)
                .Take(pageCount)
                .OrderByDescending(q=>q.CreatedAt)
                .ToListAsync();


            if (quotes is null)
            {
                responseModel = ResultModel<QuotesResponse>.ValidationError(400, "Quote Not Found!");
                goto skip;
            }

            var totalPage = (int)Math.Ceiling(itemCount / (double)pageCount);

            var paginationResult = new QuotesPaginationResult
            {
                ItemCount = itemCount,
                PageCount = pageCount,
                PageNumber = pageNumber,
                TotalPage = totalPage
            };

            var data = new QuotesResponse
            {
                Quotes = quotes,
                PaginationResult = paginationResult,
            };

            responseModel = ResultModel<QuotesResponse>.Success(200, "Get Quote", data);

        skip:
            return responseModel;
        }
    }
}
