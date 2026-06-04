using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Likes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Interfaces.Likes
{
    public interface ILikeService
    {

        Task<ResultModel<LikeCreateResponse>> CreateAsync(LikeCreateRequest requests);
        Task<ResultModel<LikeCreateResponse>> ToogleLikeAsync(LikeCreateRequest requests);
    }
}
