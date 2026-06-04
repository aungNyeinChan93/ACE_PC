using ACE_PC.Database.Data;
using ACE_PC.Domain.Entity;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Likes;
using ACE_PC.Domain.Models.Likes;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ACE_PC.BL.Services
{
    public class Likeservice : ILikeService
    {
        private readonly AppDbContext _context;

        public Likeservice(AppDbContext context)
        {
            _context = context;
        }


        //CreateAsync
        public async Task<ResultModel<LikeCreateResponse>> CreateAsync(LikeCreateRequest requests)
        {
            var responseModel = new ResultModel<LikeCreateResponse>();

            var isExist = await _context.Likes.AsNoTracking()
                .AnyAsync(l => l.UserId == requests.UserId && l.QuoteId == requests.QuoteId);

            if (isExist)
            {
                responseModel = ResultModel<LikeCreateResponse>.ValidationError(400, "Like is Already Exist");
                goto skip;
            }

            var newLike = new Like
            {
                QuoteId = requests.QuoteId,
                UserId = requests.UserId,
            };

            await _context.Likes.AddAsync(newLike);

            var result = await _context.SaveChangesAsync();

            var userName = await _context.Users.AsNoTracking()
                .Where(u => u.UserId == newLike.UserId)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();

            var data = new LikeCreateResponse
            {
                LikeDto = new Domain.Dtos.Quotes.LikeDto
                {
                    UserId = newLike.UserId,
                    UserName = userName ?? "",
                }
            };

            responseModel = result >= 1
                ? ResultModel<LikeCreateResponse>.Success(201, "Like Success", data)
                : ResultModel<LikeCreateResponse>.SystemError(500, "Like fail");

        skip:
            return responseModel;

        }

        //ToogleLikeAsync
        public async Task<ResultModel<string>> ToogleLikeAsync(LikeCreateRequest requests)
        {
            var responseModel = new ResultModel<string>();

            var like = await _context.Likes.AsNoTracking()
              .FirstOrDefaultAsync(l => l.UserId == requests.UserId && l.QuoteId == requests.QuoteId);

            if (like is not null)
            {
                _context.Likes.Remove(like);
                var result = await _context.SaveChangesAsync();
                responseModel = result >= 1
                    ? ResultModel<string>.Success(200, "Like Delete Success", "Delete like")
                    : ResultModel<string>.ValidationError(500, "Fail");
                goto skip;
            }

            if (like is null)
            {
                var newLike = new Like
                {
                    QuoteId = requests.QuoteId,
                    UserId = requests.UserId,
                };

                await _context.Likes.AddAsync(newLike);

                var result = await _context.SaveChangesAsync();

                var userName = await _context.Users.AsNoTracking()
                    .Where(u => u.UserId == newLike.UserId)
                    .Select(x => x.Name)
                    .FirstOrDefaultAsync();

                responseModel = result >= 1
               ? ResultModel<string>.Success(201, "Like Success", "Create Like Success")
               : ResultModel<string>.SystemError(500, "Like Fail");

                goto skip;
            }

        skip:
            return responseModel;
        }


    }
}
