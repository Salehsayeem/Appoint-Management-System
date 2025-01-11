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
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        /// All the values in the request body cannot be null.
        ///
        /// Sample request:
        /// 
        ///     POST /CreateAppointment
        ///     {
        ///         "patientName": "string",
        ///         "patientContact": "string",
        ///         "appointmentDateTime": "2025-01-11T10:00:00",
        ///         "doctorId": 1
        ///     }
        /// </remarks>
        /// <param name="model">The details of the appointment to create.</param>
        /// <returns>A response indicating the result of the appointment creation.</returns>
        [HttpPost("CreateAppointment")]
        public async Task<CommonResponseDto> CreateAppointment(CreateAppointmentRequestsDto model)
        {
            try
            {
                var data = await _appointmentRepository.CreateAppointment(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        /// <summary>
        /// Updates an existing appointment.
        /// </summary>
        ///
        /// <remarks>
        /// All the values in the request body cannot be null.
        ///
        /// Sample request:
        /// 
        ///     PUT /UpdateAppointment
        ///     {
        ///         "id": 1,
        ///         "patientName": "string",
        ///         "patientContact": "string",
        ///         "appointmentDateTime": "2025-01-11T10:00:00",
        ///         "doctorId": 1
        ///     }
        /// </remarks>
        /// <param name="model">The updated details of the appointment.</param>
        /// <returns>A response indicating the result of the appointment update.</returns>
        [HttpPut("UpdateAppointment")]
        public async Task<CommonResponseDto> UpdateAppointment(UpdateAppointmentRequestsDto model)
        {
            try
            {
                var data = await _appointmentRepository.UpdateAppointment(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Retrieves all appointments based on optional filters.
        /// </summary>
        ///
        /// <remarks>
        /// You can filter the appointments by search keyword or doctor ID. Pagination parameters are mandatory.
        ///
        /// Sample request:
        /// 
        ///     GET /GetAllAppointment?search=John&pageNo=1&pageSize=10
        ///
        /// </remarks>
        /// <param name="search">The search keyword for filtering appointments by patient name (optional).</param>
        /// <param name="doctorId">The ID of the doctor to filter appointments by (optional).</param>
        /// <param name="pageNo">The page number for pagination.</param>
        /// <param name="pageSize">The number of items per page for pagination.</param>
        /// <returns>A response containing the list of filtered appointments with pagination metadata.</returns>

        [HttpGet("GetAllAppointment")]
        public async Task<CommonResponseDto> GetAllAppointment(string? search, int? doctorId, int pageNo, int pageSize)
        {
            try
            {
                var data = await _appointmentRepository.GetAllAppointment(search, doctorId, pageNo, pageSize);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// Retrieves details of a specific appointment by its ID.
        /// </summary>
        ///
        /// <remarks>
        /// Provide the appointment ID to retrieve its details.
        ///
        /// Sample request:
        /// 
        ///     GET /GetAppointmentById?id=1
        ///
        /// </remarks>
        /// <param name="id">The ID of the appointment to retrieve.</param>
        /// <returns>A response containing the details of the appointment.</returns>
        [HttpGet("GetAppointmentById")]
        public async Task<CommonResponseDto> GetAppointmentById(int id)
        {
            try
            {
                var data = await _appointmentRepository.GetAppointmentById(id);
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
        /// <summary>
        /// Deletes an appointment by its ID.
        /// </summary>
        ///
        /// <remarks>
        /// Provide the appointment ID to delete it.
        ///
        /// Sample request:
        /// 
        ///     DELETE /DeleteAppointment?id=1
        ///
        /// </remarks>
        /// <param name="id">The ID of the appointment to delete.</param>
        /// <returns>A response indicating the result of the deletion.</returns>
        [HttpDelete("DeleteAppointment")]
        public async Task<CommonResponseDto> DeleteAppointment(int id)
        {
            try
            {
                var data = await _appointmentRepository.DeleteAppointment(id);
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
