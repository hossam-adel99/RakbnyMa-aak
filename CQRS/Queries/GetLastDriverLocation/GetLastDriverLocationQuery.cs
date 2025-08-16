using MediatR;
using RakbnyMa_aak.DTOs.TripTrackingDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;

namespace RakbnyMa_aak.CQRS.Queries.GetLastDriverLocation
{
    public record GetLastDriverLocationQuery(int TripId) : IRequest<Response<DriverLocationDto>>;

}
