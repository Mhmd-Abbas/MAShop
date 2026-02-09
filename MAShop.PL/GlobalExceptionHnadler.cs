using MAShop.DAL.DTO.Response;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace MAShop.PL
{
    public class GlobalExceptionHnadler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception ex, CancellationToken cancellationToken)
        {
            var errorDetails = new ErrorDetails
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = ex.Message,
                StackTrace = ex.InnerException?.Message
            };

            context.Response.StatusCode = errorDetails.StatusCode;
            await context.Response.WriteAsJsonAsync(errorDetails);

            return true;
        }
    }
}
