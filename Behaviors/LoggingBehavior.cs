using MediatR;

namespace RakbnyMa_aak.Behaviors
{
    using MediatR;

    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            Console.WriteLine($"[LOG] Handling {typeof(TRequest).Name}");

            var response = await next();

            Console.WriteLine($"[LOG] Handled {typeof(TResponse).Name}");

            return response;
        }
    }

}
