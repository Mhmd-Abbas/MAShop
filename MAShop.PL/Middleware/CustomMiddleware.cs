namespace MAShop.PL.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("Middleware 1: Before next()");
            await _next(context);
            Console.WriteLine("Middleware 1: After next()");
        }
    }
}
