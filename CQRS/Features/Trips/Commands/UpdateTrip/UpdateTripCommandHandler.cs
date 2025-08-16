using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.UpdateTrip
{
    public class UpdateTripCommandHandler : IRequestHandler<UpdateTripCommand, Response<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateTripCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(UpdateTripCommand request, CancellationToken cancellationToken)
        {
            var existingTrip = await _unitOfWork.TripRepository.GetByIdAsync(request.TripId);

            if (existingTrip == null || existingTrip.IsDeleted)
                return Response<int>.Fail("لم يتم العثور على الرحلة.");

            var hasConfirmedBookings = await _unitOfWork.BookingRepository
                .GetAllQueryable()
                .AnyAsync(b => b.TripId == request.TripId && b.RequestStatus == RequestStatus.Confirmed, cancellationToken);

            if (hasConfirmedBookings)
            {
                return Response<int>.Fail("لا يمكن تعديل الرحلة لوجود حجوزات مؤكدة.");
            }

            _mapper.Map(request.TripDto, existingTrip);
            existingTrip.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.TripRepository.Update(existingTrip);
            await _unitOfWork.CompleteAsync();

            return Response<int>.Success(existingTrip.Id, "تم تعديل الرحلة بنجاح.");
        }
    }
}
