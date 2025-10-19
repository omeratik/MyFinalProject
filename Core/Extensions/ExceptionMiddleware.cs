using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = "Internal Server Error";
            string detailedMessage = e.Message;
            
            // Log exception for server-side debugging
            Console.WriteLine($"HATA: {e.Message}");
            Console.WriteLine($"STACK TRACE: {e.StackTrace}");
            
            // Log inner exception if exists
            if (e.InnerException != null)
            {
                Console.WriteLine($"INNER EXCEPTION: {e.InnerException.Message}");
                Console.WriteLine($"INNER STACK TRACE: {e.InnerException.StackTrace}");
                detailedMessage += $" - {e.InnerException.Message}";
            }
            
            IEnumerable<ValidationFailure> errors; //hatanın türünü kontrol et doğrulama hatası ise bunu döndür demek alttak kodlar.
            if (e.GetType() == typeof(ValidationException))
            {
                message = e.Message;
                errors = ((ValidationException)e).Errors;
                httpContext.Response.StatusCode = 400;

                return httpContext.Response.WriteAsync(new ValidationErrorDetails //errordetails yazmıyoruz artık burası validation a özel çalışacak.
                {
                    StatusCode = 406,
                    Message = message,
                    Errors = errors //error aldığım zaman doldurmak için ErrorDetails de IEnumerable olduğu için validationfailture den ekliyoruz.

                }.ToString());
            }

            // Check if we're in development environment
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = isDevelopment ? detailedMessage : message, // Development ortamında detaylı mesaj göster
                DetailedError = isDevelopment ? e.ToString() : null  // Development ortamında stack trace göster
            }.ToString());
        }
    }
}
