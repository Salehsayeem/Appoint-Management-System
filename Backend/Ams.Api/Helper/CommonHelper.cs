using Ams.Api.Dto.Requests;
using Ams.Api.Dto;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using Ams.Api.Context;

namespace Ams.Api.Helper
{
    public static class CommonHelper
    {
        public static Ulid StringToUlidConverter(string userId)
        {
            try
            {
                if (Ulid.TryParse(userId, out Ulid user))
                {
                    return user;
                }
                else
                {
                    throw new Exception("Invalid User ID format.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public class UlidToStringConverter : ValueConverter<Ulid, string>
        {
            public UlidToStringConverter() : base(
                ulid => ulid.ToString(), // Convert Ulid to string
                value => Ulid.Parse(value) // Convert string to Ulid
            )
            {
            }
        }
        public static CommonResponseDto? ValidateNameAndPassword(string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new CommonResponseDto
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Name is required.",
                    Succeed = false
                };
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return new CommonResponseDto
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Password is required.",
                    Succeed = false
                };
            }

            if (password.Length < 6 ||
                !password.Any(char.IsUpper) ||
                !password.Any(char.IsLower) ||
                !password.Any(char.IsDigit) ||
                !password.Any(ch => "!@#$%^&*()-_=+[]{}|;:'\",.<>?/".Contains(ch)))
            {
                return new CommonResponseDto
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Password must be at least 6 characters long and contain uppercase, lowercase, a digit, and a special character.",
                    Succeed = false
                };
            }
            return null;
        }

        public static bool FutureDateValidation(DateTime date)
        {
            return date > DateTime.Now;
        }
        public static bool ValidateDoctorExists(int doctorId, HealthCareDbContext context)
        {
            return context.Doctors.Any(a => a.Id == doctorId);
        }
    }
}
