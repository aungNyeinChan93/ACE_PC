using ACE_PC.Domain.Entity;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Models.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<ResultModel<RegisterResponse>> RegisterAsync(RegisterRequest request);

        Task<ResultModel<LoginResponse>> LoginAsync(LoginRequest request);

        Task<string> GenerateToken(User user);

    }
}
