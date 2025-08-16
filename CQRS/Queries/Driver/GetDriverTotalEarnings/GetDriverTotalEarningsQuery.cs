namespace RakbnyMa_aak.CQRS.Queries.Driver.GetDriverTotalEarnings
{
    using global::RakbnyMa_aak.DTOs.DriverDTOs.Dashboard;
    using global::RakbnyMa_aak.GeneralResponse;
    using MediatR;
    using System.Collections.Generic;

    namespace RakbnyMa_aak.CQRS.Queries.Driver.GetDriverTotalEarnings
    {
        public class GetDriverTotalEarningsQuery : IRequest<Response<List<MonthlyEarningsDto>>>
        {

        }
    }

}
