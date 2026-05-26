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

        Task<ResultModel<UsersResposne>> GetAllUses();
    }
}
