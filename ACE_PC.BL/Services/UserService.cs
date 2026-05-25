using ACE_PC.Database.Data;
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
