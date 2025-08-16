using MediatR;
using RakbnyMa_aak.DTOs.DriverDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Auth.Queries.GetDriverById
{
    public record GetDriverByIdQuery(string DriverId) : IRequest<Response<DriverResponseDto>>;

}
