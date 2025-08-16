using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.DriverDTOs.Dashboard;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using System.Security.Claims;

namespace RakbnyMa_aak.CQRS.Queries.Driver.GetDriverTripCount
{
    public class GetDriverTripCountQueryHandler : IRequestHandler<GetDriverTripCountQuery, Response<List<MonthlyTripCountDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetDriverTripCountQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<List<MonthlyTripCountDto>>> Handle(GetDriverTripCountQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Response<List<MonthlyTripCountDto>>.Fail("غير مصرح");

            var fromDate = DateTime.UtcNow.AddMonths(-5);

            var trips = await _unitOfWork.TripRepository.GetAllQueryable()
                .Where(t => t.DriverId == userId && t.CreatedAt >= fromDate)
                .ToListAsync(cancellationToken);

            var result = trips
                .GroupBy(t => new { t.CreatedAt.Year, t.CreatedAt.Month })
                .Select(g => new MonthlyTripCountDto
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM"),
                    TripCount = g.Count()
                }).ToList();

            return Response<List<MonthlyTripCountDto>>.Success(result);
        }
    }

}
