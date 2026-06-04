using ACE_PC.Database.Data;
using ACE_PC.Domain.Dtos.Quotes;
using ACE_PC.Domain.Dtos.Users;
using ACE_PC.Domain.Entity;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Users;
using ACE_PC.Domain.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using Dapper;

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
            return await ExecuteDapperUsersQueryAsync(1, 0, isPaged: false);
        }

        //GetAllUsersWithPagination
        public async Task<ResultModel<UsersResposne>> GetAllUses(UserPaginationRequest pagination)
        {
            return await ExecuteDapperUsersQueryAsync(pagination.PageNumber, pagination.PageCount, isPaged: true);
        }



        // GetUserByEmail
        public async Task<ResultModel<UserResponse>> GetUserByEmail(string email)
        {
            var responseModel = new ResultModel<UserResponse>();

            var user = await _context.Users.AsNoTracking()
                .Include(u => u.Role)!
                .Include(u => u.Quotes)!
                .Include(u => u.Comments)!.ThenInclude(c => c.Quote)
                .Include(u => u.Comments)!.ThenInclude(c => c.User)
                .Include(u => u.Likes)!.ThenInclude(l => l.Quote)!.ThenInclude(q => q!.User)!
                .Include(u => u.Likes)!.ThenInclude(l => l.User)!
                .Where(u => u.Email == email)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    Email = u.Email,
                    Role = u.Role,
                    Quotes = u.Quotes!.Select(q => new QuotesDto
                    {
                        Author = q.User!.Name,
                        AuthorId = q.UserId,
                        Category = q.Category!.Name,
                        CategoryId = q.UserId,
                        Comments = q.Comments!.Select(c => new CommentDto
                        {
                            Content = c.Body,
                            Id = c.CommentId,
                            UserName = c.User!.Name,
                        }).ToList(),
                        Content = q.Content,
                        CreatedAt = q.CreatedAt,
                        Id = q.QuoteId,
                        Likes = q.Likes!.Select(l => new LikeDto
                        {
                            UserId = l.UserId,
                            UserName = l.User!.Name,
                        }).ToList(),
                        Title = q.Title
                    }).ToList(),
                    Password = u.Password,
                    Comments = u.Comments!.Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        Content = c.Body,
                        UserName = c.User!.Name
                    }).OrderByDescending(u => u.Id).ToList(),
                    Likes = u.Likes!.Select(l => new LikeDto
                    {
                        UserId = l.UserId,
                        UserName = l.User!.Name,
                    }).ToList(),
                    LikeQuotes = u.Likes!.Select(l => new Quote
                    {
                        Title = l.Quote!.Title
                    }).ToList(),
                    Name = u.Name,
                })
                .FirstOrDefaultAsync();

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
                    Quotes = u.Quotes!.Select(q => new QuotesDto
                    {
                        Author = q.User!.Name,
                        AuthorId = q.UserId,
                        Category = q.Category!.Name,
                        CategoryId = q.UserId,
                        Comments = q.Comments!.Select(c => new CommentDto
                        {
                            Content = c.Body,
                            Id = c.CommentId,
                            UserName = c.User!.Name,
                        }).ToList(),
                        Content = q.Content,
                        CreatedAt = q.CreatedAt,
                        Id = q.QuoteId,
                        Likes = q.Likes!.Select(l => new LikeDto
                        {
                            UserId = l.UserId,
                            UserName = l.User!.Name,
                        }).ToList(),
                        Title = q.Title
                    }).ToList(),
                    Password = u.Password,
                    Comments = u.Comments!.Select(c => new CommentDto
                    {
                        Id = c.CommentId,
                        Content = c.Body,
                        UserName = c.User!.Name
                    }).OrderByDescending(u => u.Id).ToList(),
                    Likes = u.Likes!.Select(l => new LikeDto
                    {
                        UserId = l.UserId,
                        UserName = l.User!.Name,
                    }).ToList(),
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
            return await ExecuteDapperUsersQueryAsync(paginationRequest.PageNumber, paginationRequest.PageCount, isPaged: true, request);
        }


        //Update User
        public async Task<ResultModel<UserEditResponse>> UpdateUserAsync(int id, UserEditRequest request)
        {
            var responseModel = new ResultModel<UserEditResponse>();

            var updateUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);

            if (updateUser is null)
            {
                responseModel = ResultModel<UserEditResponse>.ValidationError(404, "User not found!");
                goto skip;
            }

            updateUser.Name = request.Name;
            updateUser.Email = request.Email;
            updateUser.RoleId = request.RoleId;

            if (!string.IsNullOrEmpty(request.Password))
            {
                var hashPassword = new PasswordHasher<User>().HashPassword(updateUser, request.Password);
                updateUser.Password = hashPassword;
            }

            _context.Entry(updateUser).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();

            responseModel = result >= 1
                ? ResultModel<UserEditResponse>.Success(200, "User Update Success")
                : ResultModel<UserEditResponse>.SystemError(500, "Invalid Request");

        skip:
            return responseModel;
        }

        private class CommentDtoRaw
        {
            public int Id { get; set; }
            public string Content { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public int UserId { get; set; }
            public int QuoteId { get; set; }
        }

        private class LikeDtoRaw
        {
            public int UserId { get; set; }
            public string UserName { get; set; } = string.Empty;
            public int QuoteId { get; set; }
            public string QuoteTitle { get; set; } = string.Empty;
        }

        private async Task<ResultModel<UsersResposne>> ExecuteDapperUsersQueryAsync(
            int pageNumber,
            int pageCount,
            bool isPaged,
            UserSearchRequest? searchRequest = null)
        {
            var responseModel = new ResultModel<UsersResposne>();

            pageNumber = pageNumber > 0 ? pageNumber : 1;
            pageCount = pageCount > 0 ? pageCount : 10;
            var skip = (pageNumber - 1) * pageCount;

            var connection = _context.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }

            var whereClauses = new List<string>();
            var parameters = new DynamicParameters();

            if (searchRequest != null)
            {
                if (!string.IsNullOrEmpty(searchRequest.Name))
                {
                    whereClauses.Add("u.Name LIKE @Name");
                    parameters.Add("Name", $"%{searchRequest.Name}%");
                }
                if (!string.IsNullOrEmpty(searchRequest.Email))
                {
                    whereClauses.Add("u.Email LIKE @Email");
                    parameters.Add("Email", $"%{searchRequest.Email}%");
                }
            }

            string whereClause = whereClauses.Count > 0
                ? "WHERE " + string.Join(" OR ", whereClauses)
                : "";

            var countSql = $"SELECT COUNT(*) FROM Users u {whereClause}";
            var userCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);
            var totalPage = isPaged ? (int)Math.Ceiling(userCount / (double)pageCount) : 1;

            var paginationResult = new UserPaginationResut
            {
                ItemCount = userCount,
                PageCount = isPaged ? pageCount : userCount,
                PageNumber = isPaged ? pageNumber : 1,
                TotalPage = totalPage,
            };

            var usersSqlBuilder = new StringBuilder();
            usersSqlBuilder.AppendLine(@"
                SELECT u.UserId, u.Name, u.Password, u.Email, u.RoleId, r.RoleId, r.Name
                FROM Users u
                LEFT JOIN Roles r ON u.RoleId = r.RoleId");

            if (!string.IsNullOrEmpty(whereClause))
            {
                usersSqlBuilder.AppendLine(whereClause);
            }

            usersSqlBuilder.AppendLine("ORDER BY u.UserId");

            if (isPaged)
            {
                usersSqlBuilder.AppendLine("OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY");
                parameters.Add("Skip", skip);
                parameters.Add("Take", pageCount);
            }

            var users = (await connection.QueryAsync<UserDto, Role, UserDto>(
                usersSqlBuilder.ToString(),
                (user, role) =>
                {
                    user.Role = role;
                    user.Quotes = new List<QuotesDto>();
                    user.Comments = new List<CommentDto>();
                    user.Likes = new List<LikeDto>();
                    user.LikeQuotes = new List<Quote>();
                    return user;
                },
                parameters,
                splitOn: "RoleId"
            )).ToList();

            if (users is null || users.Count <= 0)
            {
                responseModel = ResultModel<UsersResposne>.ValidationError(400, "Users Not found!");
                return responseModel;
            }

            var userIds = users.Select(u => u.UserId).ToList();

            var quotesSql = @"
                SELECT q.QuoteId AS Id, q.Title, q.Content, q.CreatedAt, q.UserId AS AuthorId, 
                       c.Name AS Category, q.CategoryId, u.Name AS Author
                FROM Quotes q
                LEFT JOIN Categories c ON q.CategoryId = c.CategoryId
                LEFT JOIN Users u ON q.UserId = u.UserId
                WHERE q.UserId IN @UserIds";

            var quotes = (await connection.QueryAsync<QuotesDto>(quotesSql, new { UserIds = userIds })).ToList();

            if (quotes.Count > 0)
            {
                var quoteIds = quotes.Select(q => q.Id).ToList();

                var quoteComments = (await connection.QueryAsync<CommentDtoRaw>(
                    @"SELECT c.CommentId AS Id, c.Body AS Content, c.QuoteId, u.Name AS UserName
                      FROM Comments c
                      LEFT JOIN Users u ON c.UserId = u.UserId
                      WHERE c.QuoteId IN @QuoteIds",
                    new { QuoteIds = quoteIds }
                )).ToList();

                var quoteLikes = (await connection.QueryAsync<LikeDtoRaw>(
                    @"SELECT l.QuoteId, l.UserId, u.Name AS UserName
                      FROM Likes l
                      LEFT JOIN Users u ON l.UserId = u.UserId
                      WHERE l.QuoteId IN @QuoteIds",
                    new { QuoteIds = quoteIds }
                )).ToList();

                var commentsByQuote = quoteComments.GroupBy(c => c.QuoteId).ToDictionary(g => g.Key, g => g.ToList());
                var likesByQuote = quoteLikes.GroupBy(l => l.QuoteId).ToDictionary(g => g.Key, g => g.ToList());

                foreach (var quote in quotes)
                {
                    if (commentsByQuote.TryGetValue(quote.Id, out var commentsList))
                    {
                        quote.Comments = commentsList.Select(c => new CommentDto
                        {
                            Id = c.Id,
                            Content = c.Content,
                            UserName = c.UserName
                        }).ToList();
                    }
                    if (likesByQuote.TryGetValue(quote.Id, out var likesList))
                    {
                        quote.Likes = likesList.Select(l => new LikeDto
                        {
                            UserId = l.UserId,
                            UserName = l.UserName
                        }).ToList();
                    }
                }
            }

            var userComments = (await connection.QueryAsync<CommentDtoRaw>(
                @"SELECT c.CommentId AS Id, c.Body AS Content, c.UserId, u.Name AS UserName
                  FROM Comments c
                  LEFT JOIN Users u ON c.UserId = u.UserId
                  WHERE c.UserId IN @UserIds",
                new { UserIds = userIds }
            )).ToList();

            var userLikes = (await connection.QueryAsync<LikeDtoRaw>(
                @"SELECT l.UserId, l.QuoteId, u.Name AS UserName, q.Title AS QuoteTitle
                  FROM Likes l
                  LEFT JOIN Users u ON l.UserId = u.UserId
                  LEFT JOIN Quotes q ON l.QuoteId = q.QuoteId
                  WHERE l.UserId IN @UserIds",
                new { UserIds = userIds }
            )).ToList();

            var quotesByUser = quotes.GroupBy(q => q.AuthorId).ToDictionary(g => g.Key, g => g.ToList());
            var commentsByUser = userComments.GroupBy(c => c.UserId).ToDictionary(g => g.Key, g => g.ToList());
            var likesByUser = userLikes.GroupBy(l => l.UserId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var user in users)
            {
                if (quotesByUser.TryGetValue(user.UserId, out var userQuotesList))
                {
                    user.Quotes = userQuotesList;
                }
                if (commentsByUser.TryGetValue(user.UserId, out var userCommentsList))
                {
                    user.Comments = userCommentsList.Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        UserName = c.UserName
                    }).OrderByDescending(c => c.Id).ToList();
                }
                if (likesByUser.TryGetValue(user.UserId, out var userLikesList))
                {
                    user.Likes = userLikesList.Select(l => new LikeDto
                    {
                        UserId = l.UserId,
                        UserName = l.UserName
                    }).ToList();

                    user.LikeQuotes = userLikesList.Select(l => new Quote
                    {
                        Title = l.QuoteTitle
                    }).ToList();
                }
            }

            var data = new UsersResposne
            {
                UserDto = users,
                PaginationResult = isPaged ? paginationResult : null
            };

            responseModel = ResultModel<UsersResposne>.Success(200, "Get Users Success", data);
            return responseModel;
        }
    }
}
