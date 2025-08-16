using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using RakbnyMa_aak.Utilities;

namespace RakbnyMa_aak.CQRS.Queries.GetTripCoordinates
{
    public class GetTripCoordinatesQueryHandler : IRequestHandler<GetTripCoordinatesQuery, Response<TripCoordinatesResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTripCoordinatesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<TripCoordinatesResponseDto>> Handle(GetTripCoordinatesQuery request, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.TripRepository
                .GetAllQueryable()
                .Include(t => t.FromCity)
                .Include(t => t.ToCity)
                .FirstOrDefaultAsync(t => t.Id == request.TripId && !t.IsDeleted);

            if (trip is null)
                return Response<TripCoordinatesResponseDto>.Fail("الرحلة غير موجودة.");

            var fromCityName = trip.FromCity?.Name;
            var toCityName = trip.ToCity?.Name;

            if (string.IsNullOrEmpty(fromCityName) || string.IsNullOrEmpty(toCityName))
                return Response<TripCoordinatesResponseDto>.Fail("اسم إحدى المدينتين أو كليهما مفقود.");

            if (!CityCoordinates.Coordinates.TryGetValue(fromCityName, out var fromCoordinates))
                return Response<TripCoordinatesResponseDto>.Fail($"الإحداثيات الخاصة بـ '{fromCityName}' غير موجودة.");

            if (!CityCoordinates.Coordinates.TryGetValue(toCityName, out var toCoordinates))
                return Response<TripCoordinatesResponseDto>.Fail($"الإحداثيات الخاصة بـ '{toCityName}' غير موجودة.");

            var result = new TripCoordinatesResponseDto
            {
                FromCity = fromCityName,
                FromLatitude = fromCoordinates.Latitude,
                FromLongitude = fromCoordinates.Longitude,
                ToCity = toCityName,
                ToLatitude = toCoordinates.Latitude,
                ToLongitude = toCoordinates.Longitude
            };

            return Response<TripCoordinatesResponseDto>.Success(result);
        }
    }
}
