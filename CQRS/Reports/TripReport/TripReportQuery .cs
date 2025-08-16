using MediatR;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Reports.TripReport
{
    public class TripReportQuery : IRequest<Response<byte[]>>
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public TripReportQuery(DateTime fromDate, DateTime toDate)
        {
            FromDate = fromDate;
            ToDate = toDate;
        }
    }
}
