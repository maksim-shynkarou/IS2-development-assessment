namespace DataExporter.Middleware
{
    public class CancellationTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public CancellationTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                try
                {
                    await _next(context);
                }
                catch (OperationCanceledException)
                {
                    context.Response.StatusCode = 499;
                }
            }
        }
    }
}
