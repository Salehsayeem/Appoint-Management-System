using Ams.Api.Dto.Requests;
using Ams.Api.Dto;
using Ams.Api.Helper;
using Ams.Api.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ams.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// You can Register here
        /// </summary>

        /// <remarks>
        /// All the values in the request body can not be null.
        /// 
        ///  
        /// Sample request:
        ///
        ///     POST /Register
        ///     {
        ///        "name": "string",
        ///        "password": "string"
        ///     }
        /// </remarks>
        /// <param name="request"></param>
        [HttpPost("Register")]
        public async Task<CommonResponseDto> Register(RegisterDto model)
        {
            try
            {
                var validationResult = CommonHelper.ValidateNameAndPassword(model.Name, model.Password);
                if (validationResult != null)
                {
                    return validationResult;
                }
                var data = await _repo.Register(model);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// You can Login here
        /// </summary>

        /// <remarks>
        /// All the values in the request body can not be null.
        /// 
        ///  
        /// Sample request:
        ///
        ///     POST /Login
        ///     {
        ///        "name": "string",
        ///        "password": "string"
        ///     }
        /// </remarks>
        /// <param name="request"></param>
        [HttpPost("Login")]
        public async Task<CommonResponseDto> Login(LoginDto model)
        {
            try
            {
                var validationResult = CommonHelper.ValidateNameAndPassword(model.Name, model.Password);
                if (validationResult != null)
                {
                    return validationResult;
                }
                var data = await _repo.Login(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
