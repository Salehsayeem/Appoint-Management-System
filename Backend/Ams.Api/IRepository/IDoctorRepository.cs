using Ams.Api.Dto;
using Ams.Api.Dto.Requests;

namespace Ams.Api.IRepository
{
    public interface IDoctorRepository
    {
        public Task<CommonResponseDto> CreateUpdateDoctor(DoctorRequestDto obj);

        public Task<List<DoctorRequestDto>> GetAllDoctor(string? search);

        public Task<DoctorRequestDto> GetDoctorById(int id);

        public Task<CommonResponseDto> DeleteDoctor(int id);
    }
}
