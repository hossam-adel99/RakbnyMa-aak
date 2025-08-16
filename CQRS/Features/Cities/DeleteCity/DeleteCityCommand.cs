using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Cities.DeleteCity
{
    public record DeleteCityCommand(int Id) : IRequest<Response<string>>;
}
