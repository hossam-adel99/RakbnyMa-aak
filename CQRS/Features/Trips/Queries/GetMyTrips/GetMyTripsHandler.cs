using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using System.Security.Claims;

namespace RakbnyMa_aak.CQRS.Features.Trip.Queries.GetMyTrips
{
    public class GetMyTripsHandler : IRequestHandler<GetMyTripsQuery, Response<PaginatedResult<TripResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyTripsHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<PaginatedResult<TripResponseDto>>> Handle(GetMyTripsQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Response<PaginatedResult<TripResponseDto>>.Fail("غير مصرح");
            }

            var result = await _unitOfWork.TripRepository.GetProjectedPaginatedAsync<TripResponseDto>(
                predicate: t => t.DriverId == userId && !t.IsDeleted,
                page: request.Page,
                pageSize: request.PageSize,
                mapper: _mapper,
                cancellationToken: cancellationToken
            );

            return Response<PaginatedResult<TripResponseDto>>.Success(result);
        }
    }
}
