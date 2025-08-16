using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Commands.Validations.ValidateCityInGovernorate
{
    public record ValidateCityInGovernorateCommand(int CityId, int GovernorateId) : IRequest<Response<bool>>;
}
