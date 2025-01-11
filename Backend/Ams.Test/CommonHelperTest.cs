using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ams.Api.Controllers;
using Ams.Api.IRepository;
using Ams.Api.Dto.Requests;
using Ams.Api.Dto;
using Ams.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Ams.Api.Context;
using Ams.Api.Repository;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Ams.Api.Helper;
using Microsoft.AspNetCore.Http;

namespace Ams.Test
{
    public class CommonHelperTests
    {
        [Theory]
        [InlineData("", "Password123@", StatusCodes.Status400BadRequest, "Name is required.")]
        [InlineData("John", "", StatusCodes.Status400BadRequest, "Password is required.")]
        [InlineData("John", "short", StatusCodes.Status400BadRequest, "Password must be at least 6 characters long and contain uppercase, lowercase, a digit, and a special character.")]
        [InlineData("John", "Password", StatusCodes.Status400BadRequest, "Password must be at least 6 characters long and contain uppercase, lowercase, a digit, and a special character.")]
        [InlineData("John", "Password123@", null, null)]
        public void ValidateNameAndPassword_ShouldReturnCorrectResponse(string name, string password, int? expectedStatusCode, string expectedMessage)
        {
            // Act
            var result = CommonHelper.ValidateNameAndPassword(name, password);

            // Assert
            if (expectedStatusCode.HasValue)
            {
                Assert.NotNull(result);
                Assert.Equal(expectedStatusCode.Value, result.StatusCode);
                Assert.Equal(expectedMessage, result.Message);
                Assert.False(result.Succeed);
            }
            else
            {
                Assert.Null(result);
            }
        }

        [Theory]
        [InlineData("2025-02-15T10:00:00", true)]
        [InlineData("2020-02-15T10:00:00", false)]
        [InlineData("2025-12-31T23:59:59", true)]
        [InlineData("2020-01-01T01:01:01", false)]
        public void FutureDateValidation_ShouldReturnCorrectResult(string dateString, bool expectedResult)
        {
            // Act
            var date = DateTime.Parse(dateString);
            var result = CommonHelper.FutureDateValidation(date);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(999, false)]
        public void ValidateDoctorExists_ShouldReturnCorrectResult(int doctorId, bool expectedResult)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<HealthCareDbContext>()
                .UseInMemoryDatabase(databaseName: "HealthCareDb")
                .Options;

            using (var context = new HealthCareDbContext(options))
            {
                if (doctorId == 1)
                {
                    context.Doctors.Add(new Doctor { Id = 1, Name = "Dr. John Doe" });
                    context.SaveChanges();
                }

                // Act
                var result = CommonHelper.ValidateDoctorExists(doctorId, context);

                // Assert
                Assert.Equal(expectedResult, result);
            }
        }
    }
}
