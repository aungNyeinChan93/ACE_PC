using ACE_PC.Database.Data;
using ACE_PC.Domain.Entity;
using ACE_PC.Domain.Helpers.ReqResHelper;
using ACE_PC.Domain.Interfaces.Auth;
using ACE_PC.Domain.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ACE_PC.BL.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResultModel<RegisterResponse>> RegisterAsync(RegisterRequest request)
        {

            var responseModel = new ResultModel<RegisterResponse>();
            var isUserExist = await _context.Users.AsNoTracking().AnyAsync(u => u.Email == request.Email);
            if (isUserExist)
            {
                responseModel = ResultModel<RegisterResponse>.ValidationError(400, "Already User Exist");
                goto skip;
            }

            var newUser = new User
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password,
                RoleId = request.RoleId,
            };

            var hashPassword = new PasswordHasher<User>().HashPassword(newUser, newUser.Password);

            if (string.IsNullOrEmpty(hashPassword))
            {
                responseModel = ResultModel<RegisterResponse>.ValidationError(400, "Password Hashing Fail");
                goto skip;
            }

            newUser.Password = hashPassword;
            await _context.Users.AddAsync(newUser);
            var result = await _context.SaveChangesAsync();

            responseModel = result >= 1
                ? ResultModel<RegisterResponse>.Success(201, "Register Success", new RegisterResponse
                {
                    Email = newUser.Email,
                    Name = newUser.Name,
                })
                : ResultModel<RegisterResponse>.SystemError(500,"Register Fail");

        skip:
            return responseModel;
        }

        public async Task<ResultModel<LoginResponse>> LoginAsync(LoginRequest request)
        {

            var responseModel = new ResultModel<LoginResponse>();

            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(user=>user.Email == request.Email);

            if (user is null)
            {
                responseModel = ResultModel<LoginResponse>.ValidationError(400, "user not found");
                goto skip;
            }

            var checkPassword = new PasswordHasher<User>().VerifyHashedPassword(user,user.Password,request.Password);
            if (checkPassword == PasswordVerificationResult.Failed)
            {
                responseModel = ResultModel<LoginResponse>.ValidationError(400, "Credential Invalid or Wrong!");
                goto skip;
            }

            var token = await this.GenerateToken(user);

            if (string.IsNullOrEmpty(token))
            {
                responseModel = ResultModel<LoginResponse>.ValidationError(400, "Token Invalid!");
                goto skip;
            }

            var data = new LoginResponse
            {
                Token = token,
                User = user,
            };

            responseModel = ResultModel<LoginResponse>.Success(200, "Loginsuccess", data);

        skip:
            return responseModel;

        }

        public async Task<string> GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.Name),
            };

            var role = await _context.Roles.AsNoTracking()
                .FirstOrDefaultAsync(r=>r.RoleId == user.RoleId);

            if(role is not null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:secret")!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                    claims:claims,
                    signingCredentials:credentials,
                    expires :DateTime.Now.AddDays(1)
                );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return token;
        }
    }
}
