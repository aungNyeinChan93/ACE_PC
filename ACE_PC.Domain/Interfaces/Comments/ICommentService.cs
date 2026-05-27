using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Comments;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Interfaces.Comments
{
    public interface ICommentService
    {
        Task<ResultModel<CommentsResponse>> GetAllAsync();
        Task<ResultModel<CreateCommentResponse>> CreateAsync(CreateCommentRequest request);
    }
}
