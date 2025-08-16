using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.CQRS.Features.Trip.Commands.PersistTrip;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;
using TripModel = RakbnyMa_aak.Models.Trip;

namespace RakbnyMa_aak.CQRS.Features.Trips.Commands.PersistTrip
{
    public class PersistTripCommandHandler : IRequestHandler<PersistTripCommand, Response<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PersistTripCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(PersistTripCommand request, CancellationToken cancellationToken)
        {
            var trip = _mapper.Map<TripModel>(request.TripDto);
            trip.TripStatus = TripStatus.Scheduled;
            await _unitOfWork.TripRepository.AddAsync(trip);
            await _unitOfWork.CompleteAsync();

            var fromCity = await _unitOfWork.CityRepository.GetCityNameByIdAsync(request.TripDto.FromCityId);

            var toCity = await _unitOfWork.CityRepository.GetCityNameByIdAsync(request.TripDto.ToCityId);

            return Response<int>.Success(trip.Id, $"تم إنشاء رحلة من {fromCity} إلى {toCity} بنجاح.");
        }
    }
}
