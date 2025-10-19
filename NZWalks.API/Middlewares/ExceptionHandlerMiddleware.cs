using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _nextDelegate;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate nextDelegate)
        {
            _logger = logger;
            _nextDelegate = nextDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _nextDelegate(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                //Log this exception
                _logger.LogError(ex, $"{errorId} : {ex.Message}");

                //Return a custom error response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Some error occurred. Please contact support with the Error Id."
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
