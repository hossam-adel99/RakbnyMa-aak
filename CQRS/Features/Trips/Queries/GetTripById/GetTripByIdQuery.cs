using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Trips.Queries.GetTripById
{
    public record GetTripByIdQuery(int TripId) : IRequest<Response<TripResponseDto>>;
}