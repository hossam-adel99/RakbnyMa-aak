using AutoMapper;
using MediatR;
using RakbnyMa_aak.CQRS.Features.Trips.Queries.GetScheduledForDriver;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using System.Linq.Expressions;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Features.Trips.Queries.GetScheduledTrips
{
    public class GetScheduledTripsQueryHandler : IRequestHandler<GetScheduledTripsQuery, Response<PaginatedResult<TripResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetScheduledTripsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<PaginatedResult<TripResponseDto>>> Handle(GetScheduledTripsQuery request, CancellationToken cancellationToken)
        {
            var filterDto = request.Filter;

            Expression<Func<RakbnyMa_aak.Models.Trip, bool>> filter = trip =>
                trip.TripStatus == TripStatus.Scheduled &&
                !trip.IsDeleted &&
                (!filterDto.CreatedAfter.HasValue || trip.CreatedAt >= filterDto.CreatedAfter.Value) &&
                (!filterDto.CreatedBefore.HasValue || trip.CreatedAt <= filterDto.CreatedBefore.Value);

            var result = await _unitOfWork.TripRepository.GetProjectedPaginatedAsync<TripResponseDto>(
                predicate: filter,
                page: filterDto.Page,
                pageSize: filterDto.PageSize,
                mapper: _mapper,
                cancellationToken: cancellationToken
            );

            return Response<PaginatedResult<TripResponseDto>>.Success(result);
        }
    }
}
