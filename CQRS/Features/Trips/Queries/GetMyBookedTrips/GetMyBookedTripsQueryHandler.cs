using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using System.Security.Claims;

namespace RakbnyMa_aak.CQRS.Features.Trips.Queries.GetMyBookedTrips
{
    public class GetMyBookedTripsHandler : IRequestHandler<GetMyBookedTripsQuery, Response<PaginatedResult<TripResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyBookedTripsHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<PaginatedResult<TripResponseDto>>> Handle(GetMyBookedTripsQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Response<PaginatedResult<TripResponseDto>>.Fail("غير مصرح لك");

            var tripsQuery = _unitOfWork.BookingRepository.GetAllQueryable()
                .Where(b => b.UserId == userId && !b.Trip.IsDeleted)
                .Select(b => b.Trip)
                .Distinct()
                .AsNoTracking();

            var totalCount = await tripsQuery.CountAsync(cancellationToken);

            //var pagedTrips = await tripsQuery
            //    .Skip((request.Page - 1) * request.PageSize)
            //    .Take(request.PageSize)
            //    .Include(t => t.Driver)
            //    .Include(t => t.FromCity)
            //    .Include(t => t.ToCity)
            //    .ToListAsync(cancellationToken);

            var pagedTrips = await tripsQuery
                .Include(t => t.Driver)
                   .ThenInclude(d => d.User)
                      .ThenInclude(u => u.RatingsReceived)
                .Include(t => t.FromCity)
                   .ThenInclude(c => c.Governorate)
                .Include(t => t.ToCity)
                   .ThenInclude(c => c.Governorate)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);


            var tripDtos = _mapper.Map<List<TripResponseDto>>(pagedTrips);

            var result = new PaginatedResult<TripResponseDto>(
                tripDtos,
                totalCount,
                request.Page,
                request.PageSize
            );

            return Response<PaginatedResult<TripResponseDto>>.Success(result);
        }
    }
}
