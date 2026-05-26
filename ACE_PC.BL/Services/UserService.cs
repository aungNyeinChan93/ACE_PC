using ACE_PC.Database.Data;
using ACE_PC.Domain.Dtos.Users;
using ACE_PC.Domain.Entity;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Users;
using ACE_PC.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.BL.Services
{
    public class UserService : IUserService
    {

        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultModel<UsersResposne>> GetAllUses()
        {
            var responseModel = new ResultModel<UsersResposne>();
            var users = await _context.Users.AsNoTracking()
                .Include(u => u.Role)!
                .Include(u => u.Quotes)!
                .Include(u => u.Comments)!
                .Include(u => u.Likes)!.ThenInclude(l => l.Quote)!
                .Include(u => u.Likes)!.ThenInclude(l => l.User)!
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    Quotes = u.Quotes,
                    Comments = u.Comments,
                    Likes = u.Likes,
                    LikeQuotes = u.Likes!.Select(l => l.Quote).ToList()!
                })
                .ToListAsync();

            if (users is null || users.Count <=0)
            {
                responseModel = ResultModel<UsersResposne>.ValidationError(400,"Users Not found!");
                goto skip;
            }

            var data = new UsersResposne
            {
                UserDto = users
            };

            responseModel = ResultModel<UsersResposne>.Success(200,"Get All Uses");

            

        skip:
            return responseModel;
        }

        public async Task<ResultModel<UserResponse>> GetUserByEmail(string email)
        {
            var responseModel =new ResultModel<UserResponse>();

            var user = await _context.Users.AsNoTracking()
                .Include(u=> u.Role)
                .Select(u=> new User
                {
                    Email = u.Email,
                    Name = u.Name,
                    RoleId = u.RoleId,
                    Role = u.Role,
                })
                .FirstOrDefaultAsync(u=>u.Email == email);

            if (user is null)
            {
                responseModel = ResultModel<UserResponse>.ValidationError(404, "user not found!");
                goto skip;
            }

            var data = new UserResponse { User = user };
            responseModel = ResultModel<UserResponse>.Success(200, "User found!", data);

        skip:
            return responseModel;
        }
    }
}
