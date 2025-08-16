using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.DTOs.DriverDTOs.ResponseDTOs;

namespace RakbnyMa_aak.CQRS.Features.Auth.Queries.GetPendingDrivers
{
    public record GetPendingDriversQuery(int Page = 1, int PageSize = 10)
        : IRequest<Response<PaginatedResult<PendingDriverResponseDto>>>;
}
