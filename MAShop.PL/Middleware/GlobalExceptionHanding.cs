using MAShop.DAL.DTO.Response;

namespace MAShop.PL.Middleware
{
    public class GlobalExceptionHanding
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionHanding(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var errorDetails = new ErrorDetails
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    StackTrace = ex.InnerException?.Message
                };

                context.Response.StatusCode = errorDetails.StatusCode;
                await context.Response.WriteAsJsonAsync(errorDetails);

            }
        }

    }
}
