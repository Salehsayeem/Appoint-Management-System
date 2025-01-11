using Ams.Api.Dto.Requests;
using Ams.Api.Dto;

namespace Ams.Api.IRepository
{
    public interface IAppointmentRepository
    {
        public Task<CommonResponseDto> CreateAppointment(CreateAppointmentRequestsDto obj);
        public Task<CommonResponseDto> UpdateAppointment(UpdateAppointmentRequestsDto obj);

        public Task<CommonResponseDto> GetAllAppointment(string? search, int? doctorId, int pageNo, int pageSize);

        public Task<CommonResponseDto> GetAppointmentById(int id);

        public Task<CommonResponseDto> DeleteAppointment(int id);
    }
}
