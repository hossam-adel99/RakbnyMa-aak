using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Hubs;
using RakbnyMa_aak.UOW;
using Microsoft.AspNetCore.SignalR;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Features.Trip.Commands.StartTripByDriver
{
    public class StartTripByDriverCommandHandler : IRequestHandler<StartTripByDriverCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _chatHub;

        public StartTripByDriverCommandHandler(IUnitOfWork unitOfWork, IHubContext<ChatHub> chatHub)
        {
            _unitOfWork = unitOfWork;
            _chatHub = chatHub;
        }

        public async Task<Response<bool>> Handle(StartTripByDriverCommand request, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.TripRepository.GetByIdAsync(request.TripId);

            if (trip == null || trip.IsDeleted || trip.DriverId != request.DriverId || trip.TripStatus == TripStatus.Cancelled)
                return Response<bool>.Fail("غير مصرح أو لم يتم العثور على الرحلة.");

            if (trip.TripStatus != TripStatus.Scheduled)
                return Response<bool>.Fail("الرحلة قد بدأت بالفعل أو تم إنهاؤها.");

            if (DateTime.UtcNow < trip.TripDate)
                return Response<bool>.Fail("لا يمكن بدء الرحلة قبل موعد انطلاقها المحدد.");

            trip.TripStatus = TripStatus.Ongoing;
            trip.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.TripRepository.Update(trip);
            await _unitOfWork.CompleteAsync();

            await _chatHub.Clients.Group(request.TripId.ToString())
                .SendAsync("TripStarted", request.TripId);

            return Response<bool>.Success(true, "تم بدء الرحلة بنجاح.");
        }
    }
}
