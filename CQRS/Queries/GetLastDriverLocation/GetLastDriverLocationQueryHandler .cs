using MediatR;
using RakbnyMa_aak.DTOs.TripTrackingDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Queries.GetLastDriverLocation
{
    public class GetLastDriverLocationQueryHandler: IRequestHandler<GetLastDriverLocationQuery, Response<DriverLocationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetLastDriverLocationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<DriverLocationDto>> Handle(GetLastDriverLocationQuery request, CancellationToken cancellationToken)
        {
            var tracking = await _unitOfWork.TripTrackingRepository.GetLastLocationAsync(request.TripId);

            if (tracking == null)
                return Response<DriverLocationDto>.Fail("لا توجد موقع لهذه الرحلة.", null, statusCode: 404);

            var dto = new DriverLocationDto
            {
                Lat = tracking.CurrentLat,
                Lng = tracking.CurrentLong,
                Timestamp = tracking.Timestamp
            };

            return Response<DriverLocationDto>.Success(dto, "تم استرجاع الموقع الأخير بنجاح.");
        }
    }

}
