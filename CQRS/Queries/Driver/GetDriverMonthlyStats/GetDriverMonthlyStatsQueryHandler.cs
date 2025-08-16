using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.DriverDTOs.Dashboard;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using System.Security.Claims;

namespace RakbnyMa_aak.CQRS.Queries.Driver.GetDriverMonthlyStats
{
    public class GetDriverMonthlyStatsHandler : IRequestHandler<GetDriverMonthlyStatsQuery, Response<DriverMonthlyStatsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetDriverMonthlyStatsHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<DriverMonthlyStatsDto>> Handle(GetDriverMonthlyStatsQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Response<DriverMonthlyStatsDto>.Fail("غير مصرح");

            var today = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            var bookings = await _unitOfWork.BookingRepository.GetAllQueryable()
                .Where(b => b.Trip.DriverId == userId &&
                            b.HasEnded &&
                            b.CreatedAt >= firstDayOfMonth &&
                            !b.IsDeleted &&
                            !b.Trip.IsDeleted)
                .ToListAsync(cancellationToken);

            int tripCount = bookings.Count;
            decimal totalEarnings = bookings.Sum(b => b.TotalPrice);
            decimal avgEarnings = tripCount == 0 ? 0 : Math.Round(totalEarnings / tripCount, 2);

            var result = new DriverMonthlyStatsDto
            {
                TripCount = tripCount,
                TotalEarnings = totalEarnings,
                AverageEarningPerTrip = avgEarnings
            };

            return Response<DriverMonthlyStatsDto>.Success(result);
        }
    }
}
