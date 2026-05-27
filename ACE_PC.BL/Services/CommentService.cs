using ACE_PC.Database.Data;
using ACE_PC.Domain.Dtos.Quotes;
using ACE_PC.Domain.Entity;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Comments;
using ACE_PC.Domain.Models.Comments;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.BL.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultModel<CreateCommentResponse>> CreateAsync(CreateCommentRequest request)
        {
            var responseModel = new ResultModel<CreateCommentResponse>();
            var newComment = new Comment
            {
                Body = request.Body,
                QuoteId = request.QuoteId,
                UserId = request.UserId,
            };
            await _context.Comments.AddAsync(newComment);

            var result = await _context.SaveChangesAsync();



            var data = new CreateCommentResponse
            {
                Comment = new CommentDto
                {
                    Id = newComment.CommentId,
                    Content = newComment.Body,
                    //UserName = newComment.User!.Name,
                }
            };

            responseModel = result >= 1
                ? ResultModel<CreateCommentResponse>.Success(201, "Comment Create success!", data)
                : ResultModel<CreateCommentResponse>.SystemError(500, "Comment Create Fail");

            return responseModel;
        }

        public Task<ResultModel<CommentsResponse>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
