using MediatR;
using RakbnyMa_aak.CQRS.Trips.Queries;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Queries.GetDriverIdByTripId
{
    public class GetDriverIdByTripIdQueryHandler : IRequestHandler<GetDriverIdByTripIdQuery, Response<string?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDriverIdByTripIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<string?>> Handle(GetDriverIdByTripIdQuery request, CancellationToken cancellationToken)
        {
            var driverId = await _unitOfWork.TripRepository.GetDriverIdByTripIdAsync(request.TripId);
            if (driverId == null)
                return Response<string?>.Fail("السائق غير موجود لهذه الرحلة.");

            return Response<string?>.Success(driverId);
        }
    }
}
