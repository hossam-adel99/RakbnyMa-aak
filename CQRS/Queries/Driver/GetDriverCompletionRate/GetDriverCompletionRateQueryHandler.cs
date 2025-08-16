using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.CQRS.Queries.Driver.GetDriverCompletionRate;
using RakbnyMa_aak.DTOs.DriverDTOs.Dashboard;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using System.Globalization;
using System.Security.Claims;

public class GetDriverCompletionStatsHandler : IRequestHandler<GetDriverCompletionStatsQuery, Response<DriverCompletionStatsDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetDriverCompletionStatsHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Response<DriverCompletionStatsDto>> Handle(GetDriverCompletionStatsQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Response<DriverCompletionStatsDto>.Fail("غير مصرح");

        var today = DateTime.UtcNow;
        var fiveMonthsAgo = today.AddMonths(-4);

        var bookings = await _unitOfWork.BookingRepository.GetAllQueryable()
            .Where(b => b.Trip.DriverId == userId &&
                        b.CreatedAt >= fiveMonthsAgo &&
                        !b.IsDeleted &&
                        !b.Trip.IsDeleted)
            .Include(b => b.Trip)
            .ToListAsync(cancellationToken);

        var grouped = bookings
            .GroupBy(b => new { b.CreatedAt.Year, b.CreatedAt.Month })
            .Select(g => new MonthlyCompletionRateDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key.Month),
                CompletionRate = g.Count(b => b.HasEnded) == 0 ? 0.0 :
                                 Math.Round(g.Count(b => b.HasEnded) * 100.0 / g.Count(), 1)
            })
            .OrderBy(g => new DateTime(g.Year, g.Month, 1))
            .ToList();

        double overallCompletionRate = bookings.Count == 0 ? 0.0 :
            Math.Round(bookings.Count(b => b.HasEnded) * 100.0 / bookings.Count, 1);

        var result = new DriverCompletionStatsDto
        {
            OverallCompletionRate = overallCompletionRate,
            MonthlyRates = grouped
        };

        return Response<DriverCompletionStatsDto>.Success(result);
    }
}
