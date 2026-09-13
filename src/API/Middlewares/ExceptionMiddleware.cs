using System.Net;
using Cpa.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
//using NotFoundException = Cpa.Application.Exceptions.NotFoundException;
//using ValidationException = Cpa.Application.Exceptions.ValidationException;

namespace Cpa.Presentation.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _jsonContentType = "application/json";

        private readonly ILogger<ExceptionMiddleware> _logger;
              
        public ExceptionMiddleware(
      RequestDelegate next,
      ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }         
            catch (AuthenticationException ex)
            {
                httpContext.Response.StatusCode =
                    StatusCodes.Status401Unauthorized;

                await httpContext.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(
       HttpContext context,
       Exception exception)
        {
            var problem = exception switch
            {
                ValidationException ex =>
                    CreateProblem(
                        HttpStatusCode.BadRequest,
                        "Validation Error",
                        ex.Errors),

                AuthenticationException ex =>
                    CreateProblem(
                        HttpStatusCode.Unauthorized,
                        ex.Message),

                ForbiddenException ex =>
                    CreateProblem(
                        HttpStatusCode.Forbidden,
                        ex.Message),

                NotFoundException ex =>
                    CreateProblem(
                        HttpStatusCode.NotFound,
                        ex.Message),

                ConflictException ex =>
                    CreateProblem(
                        HttpStatusCode.Conflict,
                        ex.Message),

                _ =>
                    CreateProblem(
                        HttpStatusCode.InternalServerError,
                        "Internal Server Error")
            };

            context.Response.StatusCode =
                problem.Status!.Value;

            await context.Response.WriteAsJsonAsync(problem);
        }

        private static ProblemDetails CreateProblem(
            HttpStatusCode status,
            string detail,
            object? errors = null)
        {
            var problem = new ProblemDetails
            {
                Status = (int)status,
                Title = status.ToString(),
                Detail = detail
            };

            if (errors is not null)
            {
                problem.Extensions["errors"] = errors;
            }

            return problem;
        }

    }
}
