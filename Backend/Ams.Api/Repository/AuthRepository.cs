using Ams.Api.Dto;
using Ams.Api.Dto.Requests;
using Ams.Api.Helper;
using Ams.Api.IRepository;
using Ams.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Ams.Api.Context;

namespace Ams.Api.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HealthCareDbContext _context;
        private readonly AuthHelper _authHelper;

        public AuthRepository(HealthCareDbContext context, AuthHelper authHelper)
        {
            _context = context;
            _authHelper = authHelper;
        }
        public async Task<CommonResponseDto> Register(RegisterDto model)
        {
            try
            {
                var response = new CommonResponseDto();
                if (await _context.Users.AnyAsync(u => u.Name == model.Name))
                {
                    response.Message = "Username already exists";
                    response.StatusCode = 500;
                    response.Succeed = false;
                    return response;
                }
                var user = new User
                {
                    Id = Ulid.NewUlid(),
                    Name = model.Name,
                    Password = _authHelper.HashPassword(model.Password),
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                var token = _authHelper.GenerateJwtToken(user);

                response.Message = "User Registered Successfully";
                response.StatusCode = 200;
                response.Succeed = true;
                response.Data = new
                {
                    Token = token,
                };
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> Login(LoginDto model)
        {
            var msg = new CommonResponseDto();
            var user = await _context.Users.Where(a => a.Name.ToLower() == model.Name.ToLower()).FirstOrDefaultAsync();
            try
            {
                if (user == null)
                {
                    msg.Succeed = false;
                    msg.StatusCode = StatusCodes.Status404NotFound;
                    msg.Message = "User Not Found " + model.Name;
                }
                else
                {
                    bool isVerified = _authHelper.VerifyPassword(model.Password, user.Password);
                    if (isVerified)
                    {
                        var jwtToken = _authHelper.GenerateJwtToken(user);
                        msg.Succeed = true;
                        msg.StatusCode = StatusCodes.Status200OK;
                        msg.Data = jwtToken;
                        msg.Message = "Logged in Successfully";
                    }
                    else
                    {
                        msg.Succeed = false;
                        msg.StatusCode = StatusCodes.Status400BadRequest;
                        msg.Message = "Login failed";

                    }
                }

                return msg;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
