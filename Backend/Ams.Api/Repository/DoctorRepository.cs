using Ams.Api.Context;
using Ams.Api.Dto;
using Ams.Api.Dto.Requests;
using Ams.Api.Helper;
using Ams.Api.IRepository;
using Ams.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ams.Api.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthCareDbContext _context;

        public DoctorRepository(HealthCareDbContext context)
        {
            _context = context;
        }
        public async Task<CommonResponseDto> CreateUpdateDoctor(DoctorRequestDto obj)
        {
            try
            {
                var response = new CommonResponseDto();

                if (obj.Id == 0)
                {
                    var alreadyExist = await _context.Doctors.Where(a => a.Name.ToLower() == obj.Name.ToLower()).FirstOrDefaultAsync();
                    if (alreadyExist != null)
                    {
                        return new CommonResponseDto
                        {
                            Message = $"{obj.Name} already exists",
                            StatusCode = 500,
                            Succeed = false
                        };
                    }
                    var data = new Doctor()
                    {
                        Name = obj.Name,
                    };

                    await _context.Doctors.AddAsync(data);


                    response.Message = data.Name + " - Doctor has been added!";
                    response.StatusCode = 200;
                    response.Succeed = true;
                }
                else
                {
                    var data = await _context.Doctors.Where(a => a.Id == obj.Id).FirstOrDefaultAsync();
                    if (data == null)
                    {
                        response.Message = "Problem with updating " + obj.Name;
                        response.StatusCode = 500;
                        response.Succeed = false;
                        return response;
                    }
                    data.Name = obj.Name;

                    _context.Doctors.Update(data);

                    response.Message = data.Name + " - Doctor has been updated!";
                    response.StatusCode = 200;
                    response.Succeed = true;


                }
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<DoctorRequestDto>> GetAllDoctor(string? search)
        {
            try
            {

                var query = _context.Doctors.AsQueryable();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(a => a.Name.Contains(search));
                }

                var data = await query.Select(a => new DoctorRequestDto
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToListAsync();

                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<DoctorRequestDto> GetDoctorById(int id)
        {
            try
            {
                Doctor? doctor = await _context.Doctors.FirstOrDefaultAsync(a => a.Id == id);
                if (doctor == null) return null!;
                var data = new DoctorRequestDto
                {
                    Id = doctor.Id,
                    Name = doctor.Name
                };
                return data;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> DeleteDoctor(int id)
        {
            try
            {
                var data = await _context.Doctors.FirstOrDefaultAsync(a => a.Id == id);
                if (data == null)
                {
                    return new CommonResponseDto()
                    {
                        Message = "No records found",
                        StatusCode = 500,
                        Succeed = false,
                        Data = id
                    };
                }
                _context.Doctors.Remove(data);
                await _context.SaveChangesAsync();
                return new CommonResponseDto()
                {
                    Message = "Doctor has been deleted",
                    StatusCode = 200,
                    Succeed = true,
                    Data = id
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
