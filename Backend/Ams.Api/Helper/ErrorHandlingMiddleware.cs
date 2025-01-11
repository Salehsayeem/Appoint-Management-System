using Ams.Api.Dto;
using System.Net;
using System.Text.Json;

namespace Ams.Api.Helper
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new CommonResponseDto();

            switch (exception)
            {
                case not null:
                    if (exception.Message.Contains("Invalid token"))
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        response.Succeed = false;
                        break;
                    }
                    response.Succeed = false;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                default:
                    response.Succeed = false;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            response.Message = exception!.Message;

            await context.Response.WriteAsync(new CommonResponseDto()
            {
                StatusCode = response.StatusCode,
                Message = response.Message,
                Succeed = false
            }.ToString());
        }
    }
}
