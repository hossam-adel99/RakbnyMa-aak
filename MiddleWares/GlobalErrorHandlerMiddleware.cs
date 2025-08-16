using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.MiddleWares
{
    public class GlobalErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;

        public GlobalErrorHandlerMiddleware(ILogger<GlobalErrorHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var path = context.Request.Path.Value?.ToLower();
                if (path != null && path.StartsWith("/hangfire"))
                {
                    throw;
                }

                _logger.LogError(ex, "حدث استثناء غير معالج.");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = Response<string>.Fail($"حدث خطأ غير متوقع: {ex.Message}", statusCode: 500);

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
