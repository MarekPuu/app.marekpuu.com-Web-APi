using Newtonsoft.Json;
using System.Net;

namespace portfolio_api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;
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

        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var errorDetails = new ErrorDetails
            {
                ErrorType = "Internal server error",
                Message = ex.Message,
            };

            switch (ex)
            {
                case KeyNotFoundException e:
                    errorDetails.ErrorType = "Not Found";
                    statusCode = HttpStatusCode.NotFound;
                    break;
            }

            string response = JsonConvert.SerializeObject(errorDetails);
            httpContext.Response.StatusCode = (int)statusCode;
            return httpContext.Response.WriteAsync(response);

        }

        public class ErrorDetails
        {
            public string ErrorType { get; set; }
            public string Message { get; set; }
        }

    }
}
