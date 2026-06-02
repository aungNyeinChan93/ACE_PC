using ACE_PC.Database.Data;
using ACE_PC.Domain.Dtos.Quotes;
using ACE_PC.Domain.Dtos.Users;
using ACE_PC.Domain.Entity;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Users;
using ACE_PC.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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


        //DeleteUser
        public async Task<ResultModel<UserDeleteResponse>> DeleteAsync(int id)
        {
            var responseModel = new ResultModel<UserDeleteResponse>();

            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user is null)
            {
                responseModel = ResultModel<UserDeleteResponse>.ValidationError(404, "User Not found!");
                goto skip;
            }

            _context.Users.Remove(user);
            _context.Entry(user).State = EntityState.Deleted;
            var result = await _context.SaveChangesAsync();

            responseModel = result >= 1
                ? ResultModel<UserDeleteResponse>.Success(200, "User Delete Success!",
                new UserDeleteResponse { UserId = user.UserId, isDeleteSuccess = true })
                : ResultModel<UserDeleteResponse>.SystemError(500, "Delete Fail");


        skip:
            return responseModel;
        }

        //GetAllUsers
        public async Task<ResultModel<UsersResposne>> GetAllUses()
        {
            var responseModel = new ResultModel<UsersResposne>();

            var users = await _context.Users
                .Include(u => u.Role)!
                .Include(u => u.Quotes)!
                .Include(u => u.Comments)!.ThenInclude(c => c.Quote)
                .Include(u => u.Comments)!.ThenInclude(c => c.User)
                .Include(u => u.Likes)!.ThenInclude(l => l.Quote)!.ThenInclude(q => q!.User)!
                .Include(u => u.Likes)!.ThenInclude(l => l.User)!
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    Quotes = u.Quotes,
                    Comments = u.Comments!.Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        Content = c.Body,
                        UserName = c.User!.Name
                    }).ToList(),
                    Likes = u.Likes!.ToList(),
                    LikeQuotes = u.Likes!.Select(l => l.Quote).ToList()!
                })
                .ToListAsync();

            if (users is null || users.Count <= 0)
            {
                responseModel = ResultModel<UsersResposne>.ValidationError(400, "Users Not found!");
                goto skip;
            }

            var data = new UsersResposne
            {
                UserDto = users
            };

            responseModel = ResultModel<UsersResposne>.Success(200, "Get All Uses", data);



        skip:
            return responseModel;
        }

        //GetAllUsersWithPagination
        public async Task<ResultModel<UsersResposne>> GetAllUses(UserPaginationRequest pagination)
        {
            var responseModel = new ResultModel<UsersResposne>();

            var userCount = await _context.Users.CountAsync();
            var pageNumber = pagination.PageNumber;
            var pageCount = pagination.PageCount;
            var skip = (pageNumber - 1) * pageCount;
            var totalPage = (int)Math.Ceiling(userCount / (double)pageCount);

            var paginationResult = new UserPaginationResut
            {
                ItemCount = userCount,
                PageCount = pageCount,
                PageNumber = pageNumber,
                TotalPage = totalPage,
            };

            var users = await _context.Users
                .Include(u => u.Role)!
                .Include(u => u.Quotes)!
                .Include(u => u.Comments)!.ThenInclude(c => c.Quote)
                .Include(u => u.Comments)!.ThenInclude(c => c.User)
                .Include(u => u.Likes)!.ThenInclude(l => l.Quote)!.ThenInclude(q => q!.User)!
                .Include(u => u.Likes)!.ThenInclude(l => l.User)!
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    Quotes = u.Quotes,
                    Comments = u.Comments!.Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        Content = c.Body,
                        UserName = c.User!.Name
                    }).ToList(),
                    Likes = u.Likes!.ToList(),
                    LikeQuotes = u.Likes!.Select(l => l.Quote).ToList()!
                })
                .Skip(skip)
                .Take(pageCount)
                .ToListAsync();

            if (users is null || users.Count <= 0)
            {
                responseModel = ResultModel<UsersResposne>.ValidationError(400, "Users Not found!");
                goto skip;
            }

            var data = new UsersResposne
            {
                UserDto = users,
                PaginationResult = paginationResult
            };

            responseModel = ResultModel<UsersResposne>.Success(200, "Get All Uses", data);



        skip:
            return responseModel;
        }



        // GetUserByEmail
        public async Task<ResultModel<UserResponse>> GetUserByEmail(string email)
        {
            var responseModel = new ResultModel<UserResponse>();

            var user = await _context.Users.AsNoTracking()
                .Include(u => u.Role)
                .Include(u => u.Quotes)!.ThenInclude(q => q.Likes)
                .Include(u => u.Quotes)!.ThenInclude(q => q.Category)
                .Include(u => u.Comments)!.ThenInclude(c => c.Quote)
                .Include(u => u.Comments)!.ThenInclude(c => c.User)
                .Include(u => u.Likes)!.ThenInclude(l => l.User)
                .Include(u => u.Likes)!.ThenInclude(l => l.Quote)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Role = u.Role,
                    Quotes = u.Quotes,
                    Comments = u.Comments!.Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        Content = c.Body,
                        UserName = c.User!.Name
                    }).ToList(),
                    Likes = u.Likes!.ToList(),
                    LikeQuotes = u.Likes!.Select(l => new Quote
                    {
                        Title = l.Quote!.Title
                    }).ToList(),
                    Name = u.Name,
                })
                .FirstOrDefaultAsync(u => u.Email == email);

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

        //GetUserById
        public async Task<ResultModel<UserResponse>> GetUserById(int id)
        {
            var responseModel = new ResultModel<UserResponse>();

            var user = await _context.Users.AsNoTracking()
                .Include(u => u.Role)!
                .Include(u => u.Quotes)!
                .Include(u => u.Comments)!.ThenInclude(c => c.Quote)
                .Include(u => u.Comments)!.ThenInclude(c => c.User)
                .Include(u => u.Likes)!.ThenInclude(l => l.Quote)!.ThenInclude(q => q!.User)!
                .Include(u => u.Likes)!.ThenInclude(l => l.User)!
                .Where(u => u.UserId == id)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Role = u.Role,
                    Quotes = u.Quotes,
                    Comments = u.Comments!.Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        Content = c.Body,
                        UserName = c.User!.Name
                    }).ToList(),
                    Likes = u.Likes!.ToList(),
                    LikeQuotes = u.Likes!.Select(l => new Quote
                    {
                        Title = l.Quote!.Title
                    }).ToList(),
                    Name = u.Name,
                })
                .FirstOrDefaultAsync();

            if (user is null)
            {
                responseModel = ResultModel<UserResponse>.ValidationError(404, "User Not Found!");
                goto skip;
            }

            var data = new UserResponse { User = user };
            responseModel = ResultModel<UserResponse>.Success(200, "User found!", data);

        skip:
            return responseModel;
        }

        //GetUsersByNameAnd
        public async Task<ResultModel<UsersResposne>> GetAllUses(
            UserPaginationRequest paginationRequest, UserSearchRequest request)
        {
            var responseModel = new ResultModel<UsersResposne>();

            var userCount = await _context.Users.CountAsync();
            var pageNumber = paginationRequest.PageNumber;
            var pageCount = paginationRequest.PageCount;
            var skip = (pageNumber - 1) * pageCount;
            var totalPage = (int)Math.Ceiling(userCount / (double)pageCount);

            var paginationResult = new UserPaginationResut
            {
                ItemCount = userCount,
                PageCount = pageCount,
                PageNumber = pageNumber,
                TotalPage = totalPage,
            };

            var usersQuery = _context.Users
                .Include(u => u.Role)
                .Include(u => u.Quotes)!.ThenInclude(q => q.Likes)
                .Include(u => u.Quotes)!.ThenInclude(q => q.Category)
                .Include(u => u.Comments)!.ThenInclude(c => c.Quote)
                .Include(u => u.Comments)!.ThenInclude(c => c.User)
                .Include(u => u.Likes)!.ThenInclude(l => l.User)
                .Include(u => u.Likes)!.ThenInclude(l => l.Quote)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Name) || !string.IsNullOrEmpty(request.Email))
            {

                usersQuery = usersQuery.Where(u =>
                    (!string.IsNullOrEmpty(request.Name) && u.Name.Contains(request.Name))
                    ||
                    (!string.IsNullOrEmpty(request.Email) && u.Email.Contains(request.Email))
                );
            }

            var users = await usersQuery
                .Skip(skip)
                .Take(pageCount)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Role = u.Role,
                    Quotes = u.Quotes,
                    Comments = u.Comments!.Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        Content = c.Body,
                        UserName = c.User!.Name
                    }).ToList(),
                    Likes = u.Likes!.ToList(),
                    LikeQuotes = u.Likes!.Select(l => new Quote
                    {
                        Title = l.Quote!.Title
                    }).ToList(),
                    Name = u.Name,
                })
                .ToListAsync();

            //if (users is null || users.Count <= 0)
            //{
            //    responseModel = ResultModel<UsersResposne>.ValidationError(400, "Users Not found");
            //    goto skip;
            //}

            var data = new UsersResposne
            {
                PaginationResult = new UserPaginationResut
                {
                    ItemCount = users.Count,
                    PageCount = pageCount,
                    PageNumber = pageNumber,
                    TotalPage = totalPage
                },
                UserDto = users

            };

            responseModel = ResultModel<UsersResposne>.Success(200, "GetAll Users", data);

        skip:
            return responseModel;


        }
    }
}
