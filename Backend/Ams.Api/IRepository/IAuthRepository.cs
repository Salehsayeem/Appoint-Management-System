using Ams.Api.Dto;
using Ams.Api.Dto.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Ams.Api.IRepository
{
    public interface IAuthRepository
    {
        public Task<CommonResponseDto> Register([FromBody] RegisterDto model);
        public Task<CommonResponseDto> Login([FromBody] LoginDto model);
    }
}
