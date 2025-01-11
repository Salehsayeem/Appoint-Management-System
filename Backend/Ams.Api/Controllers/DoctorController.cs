using Ams.Api.Dto;
using Ams.Api.Dto.Requests;
using Ams.Api.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ams.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }
        [HttpPost("CreateOrUpdateDoctor")]
        public async Task<CommonResponseDto> CreateOrUpdateDoctor(DoctorRequestDto model)
        {
            try
            {
                var data = await _doctorRepository.CreateUpdateDoctor(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpGet("GetAllDoctor")]
        public async Task<CommonResponseDto> GetAllDoctor(string? search)
        {
            try
            {
                var data = await _doctorRepository.GetAllDoctor(search);
                return new CommonResponseDto()
                {
                    Data = data,
                    Message = string.Empty,
                    StatusCode = 200,
                    Succeed = true
                };
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpGet("GetDoctorById")]
        public async Task<CommonResponseDto> GetDoctorById(int id)
        {
            try
            {
                var data = await _doctorRepository.GetDoctorById(id);
                return new CommonResponseDto()
                {
                    Data = data,
                    Message = string.Empty,
                    StatusCode = 200,
                    Succeed = true
                };
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpDelete("DeleteDoctor")]
        public async Task<CommonResponseDto> DeleteDoctor(int id)
        {
            try
            {
                var data = await _doctorRepository.DeleteDoctor(id);
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
