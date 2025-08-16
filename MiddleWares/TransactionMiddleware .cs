using RakbnyMa_aak.Data;

namespace RakbnyMa_aak.MiddleWares
{
    public class TransactionMiddleware : IMiddleware
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<TransactionMiddleware> _logger;

        public TransactionMiddleware(AppDbContext context, ILogger<TransactionMiddleware> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            // Skip transactions for GET requests (read-only)
            if (httpContext.Request.Method == HttpMethods.Get)
            {
                await next(httpContext);
                return;
            }

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await next(httpContext);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "فشلت العملية. سيتم التراجع...");

                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}
