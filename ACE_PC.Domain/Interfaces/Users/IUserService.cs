using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Interfaces.Users
{
    public interface IUserService
    {
        Task<ResultModel<UserResponse>> GetUserByEmail(string email);
        Task<ResultModel<UserResponse>> GetUserById(int id);

        Task<ResultModel<UsersResposne>> GetAllUses();

        Task<ResultModel<UsersResposne>> GetAllUses(UserPaginationRequest request);
        Task<ResultModel<UsersResposne>> GetAllUses(UserPaginationRequest paginationRequest, UserSearchRequest request);

        Task<ResultModel<UserDeleteResponse>> DeleteAsync(int id);
    }
}
