using MediatR;
using RakbnyMa_aak.DTOs.DriverDTOs.Dashboard;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;
using System.Security.Claims;
using RakbnyMa_aak.CQRS.Queries.Driver.GetDriverTotalEarnings.RakbnyMa_aak.CQRS.Queries.Driver.GetDriverTotalEarnings;
using RakbnyMa_aak.GeneralResponse;
using Microsoft.EntityFrameworkCore;

namespace RakbnyMa_aak.CQRS.Queries.Driver.GetDriverTotalEarnings
{
    public class GetDriverTotalEarningsQueryHandler : IRequestHandler<GetDriverTotalEarningsQuery, Response<List<MonthlyEarningsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetDriverTotalEarningsQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<List<MonthlyEarningsDto>>> Handle(GetDriverTotalEarningsQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Response<List<MonthlyEarningsDto>>.Fail("غير مصرح");

            var now = DateTime.UtcNow;
            var fromDate = now.AddMonths(-5);

            var bookings = await _unitOfWork.BookingRepository.GetAllQueryable()
                .Where(b => b.Trip.DriverId == userId && b.RequestStatus == RequestStatus.Confirmed && b.PaymentStatus == PaymentStatus.Completed && b.CreatedAt >= fromDate)
                .ToListAsync(cancellationToken);

            var result = bookings
                .GroupBy(b => new { b.CreatedAt.Year, b.CreatedAt.Month })
                .Select(g => new MonthlyEarningsDto
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM"),
                    TotalEarnings = g.Sum(b => b.TotalPrice)
                }).ToList();

            return Response<List<MonthlyEarningsDto>>.Success(result);
        }
    }

}
