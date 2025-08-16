using MediatR;
using RakbnyMa_aak.DTOs.DriverDTOs.Dashboard;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Queries.Driver.GetDriverTripCount
{
    public class GetDriverTripCountQuery : IRequest<Response<List<MonthlyTripCountDto>>>
    {
         
    }
}
