using Ams.Api.Context;
using Ams.Api.Dto;
using Ams.Api.Dto.Requests;
using Ams.Api.Helper;
using Ams.Api.IRepository;
using Ams.Api.Models;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Ams.Api.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HealthCareDbContext _context;
        private readonly IMemoryCache _cache;
        public AppointmentRepository(HealthCareDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<CommonResponseDto> CreateAppointment(CreateAppointmentRequestsDto obj)
        {
            try
            {
                var response = new CommonResponseDto();
                if (CommonHelper.FutureDateValidation(obj.AppointmentDateTime) == false)
                {
                    return new CommonResponseDto
                    {
                        Message = "The appointment date must be in the future.",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Succeed = false
                    };
                }

                if (CommonHelper.ValidateDoctorExists(obj.DoctorId, _context) == false)
                {
                    return new CommonResponseDto
                    {
                        Message = "Please check doctor Id",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Succeed = false
                    };
                }
                Appointment? alreadyExist = await _context.Appointments.Where(a =>
                    a.PatientName.ToLower() == obj.PatientName.ToLower() &&
                    a.PatientContact.ToLower() == obj.PatientContact.ToLower() &&
                    a.AppointmentDateTime == obj.AppointmentDateTime &&
                    a.DoctorId == obj.DoctorId
                ).Include(appointment => appointment.Doctor).FirstOrDefaultAsync();
                if (alreadyExist != null)
                {
                    return new CommonResponseDto
                    {
                        Message = $"{obj.PatientName} already Added an appointment to {alreadyExist.Doctor.Name} on {obj.AppointmentDateTime}",
                        StatusCode = 500,
                        Succeed = false
                    };
                }
                var data = new Appointment()
                {
                    PatientName = obj.PatientName,
                    PatientContact = obj.PatientContact,
                    AppointmentDateTime = obj.AppointmentDateTime,
                    DoctorId = obj.DoctorId,
                };

                await _context.Appointments.AddAsync(data);
                await _context.SaveChangesAsync();
                _cache.Remove("GetAllAppointment");
                return new CommonResponseDto()
                {
                    Message = $"New Appointment has been added for {obj.PatientName}",
                    StatusCode = 200,
                    Succeed = true,

                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> UpdateAppointment(UpdateAppointmentRequestsDto obj)
        {
            try
            {
                var response = new CommonResponseDto();
                if (CommonHelper.FutureDateValidation(obj.AppointmentDateTime) == false)
                {
                    return new CommonResponseDto
                    {
                        Message = "The appointment date must be in the future.",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Succeed = false
                    };
                }
                if (CommonHelper.ValidateDoctorExists(obj.DoctorId, _context) == false)
                {
                    return new CommonResponseDto
                    {
                        Message = "Please check doctor Id",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Succeed = false
                    };
                }
                Appointment? appointment = await _context.Appointments.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();

                if (appointment == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "Appointment not found",
                        StatusCode = StatusCodes.Status404NotFound,
                        Succeed = false
                    };
                }

                appointment.PatientName = obj.PatientName;
                appointment.PatientContact = obj.PatientContact;
                appointment.AppointmentDateTime = obj.AppointmentDateTime;
                appointment.DoctorId = obj.DoctorId;
                _cache.Remove("GetAllAppointment");
                _cache.Remove($"GetAppointmentById_{obj.Id}");

                return new CommonResponseDto()
                {
                    Message = $"{obj.PatientName} has updated the Appointment successfully",
                    StatusCode = 200,
                    Succeed = true,
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> GetAllAppointment(string? search, int? doctorId, int pageNo, int pageSize)
        {
            string cacheKey = $"GetAllAppointment_{search}_{doctorId}_{pageNo}_{pageSize}";

            if (!_cache.TryGetValue(cacheKey, out CommonResponseDto cachedResponse))
            {
                try
                {
                    var query = _context.Appointments
                        .Include(a => a.Doctor)
                        .AsQueryable();

                    if (!string.IsNullOrWhiteSpace(search))
                    {
                        query = query.Where(a => a.PatientName.Contains(search));
                    }

                    if (doctorId.HasValue)
                    {
                        query = query.Where(a => a.DoctorId == doctorId.Value);
                    }

                    var totalRecords = await query.CountAsync();

                    var appointments = await query
                        .OrderBy(a => a.AppointmentDateTime)
                        .Skip((pageNo - 1) * pageSize)
                        .Take(pageSize)
                        .Select(a => new GetAppointmentDto
                        {
                            Id = a.Id,
                            PatientName = a.PatientName,
                            PatientContact = a.PatientContact,
                            AppointmentDateTime = a.AppointmentDateTime,
                            DoctorId = a.DoctorId,
                            DoctorName = a.Doctor.Name
                        })
                        .ToListAsync();

                    var response = new CommonResponseDto
                    {
                        Message = string.Empty,
                        StatusCode = 200,
                        Succeed = true,
                        Data = new
                        {
                            appointments,
                            totalRecords
                        }
                    };

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                    _cache.Set(cacheKey, response, cacheOptions);

                    return response;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return cachedResponse;
        }

        public async Task<CommonResponseDto> GetAppointmentById(int id)
        {
            string cacheKey = $"GetAppointmentById_{id}";

            if (!_cache.TryGetValue(cacheKey, out CommonResponseDto cachedResponse))
            {
                try
                {
                    var data = await _context.Appointments
                        .Include(a => a.Doctor)
                        .Where(a => a.Id == id)
                        .Select(a => new GetAppointmentDto
                        {
                            Id = a.Id,
                            PatientName = a.PatientName,
                            PatientContact = a.PatientContact,
                            AppointmentDateTime = a.AppointmentDateTime,
                            DoctorId = a.DoctorId,
                            DoctorName = a.Doctor.Name
                        })
                        .FirstOrDefaultAsync();

                    var response = new CommonResponseDto
                    {
                        Data = data,
                        Message = string.Empty,
                        StatusCode = 200,
                        Succeed = true
                    };

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                    _cache.Set(cacheKey, response, cacheOptions);

                    return response;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return cachedResponse;
        }


        public async Task<CommonResponseDto> DeleteAppointment(int id)
        {
            try
            {
                var data = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);
                if (data == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "Appointment not found",
                        StatusCode = StatusCodes.Status404NotFound,
                        Succeed = false
                    };
                }
                _context.Appointments.Remove(data);
                await _context.SaveChangesAsync();

                _cache.Remove("GetAllAppointment");
                _cache.Remove($"GetAppointmentById_{id}");

                return new CommonResponseDto
                {
                    Message = "Appointment has been deleted",
                    StatusCode = 200,
                    Succeed = true
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
