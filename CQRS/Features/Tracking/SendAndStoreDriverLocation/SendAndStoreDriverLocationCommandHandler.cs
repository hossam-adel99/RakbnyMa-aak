using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Hubs;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Commands.Tracking.SendAndStoreDriverLocation
{
    public class SendAndStoreDriverLocationHandler : IRequestHandler<SendAndStoreDriverLocationCommand, Response<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<TripTrackingHub> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendAndStoreDriverLocationHandler(
            IUnitOfWork unitOfWork,
            IHubContext<TripTrackingHub> hubContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<bool>> Handle(SendAndStoreDriverLocationCommand request, CancellationToken cancellationToken)
        {
            var userId = request.driverId;

            var trip = await _unitOfWork.TripRepository.GetByIdAsync(request.TripId);
            if (trip == null)
                return Response<bool>.Fail("لم يتم العثور على الرحلة", statusCode: 404);

            if (trip.DriverId != userId)
                return Response<bool>.Fail("تم الرفض: لا تملك صلاحية الوصول إلى هذه الرحلة", statusCode: 403);

            // Store in DB
            var tracking = new TripTracking
            {
                TripId = request.TripId,
                CurrentLat = request.Lat,
                CurrentLong = request.Lng,
                Timestamp = DateTime.UtcNow
            };

            await _unitOfWork.TripTrackingRepository.AddAsync(tracking);
            await _unitOfWork.CompleteAsync();

            // Send via SignalR
            await _hubContext.Clients.Group(request.TripId.ToString())
                .SendAsync("ReceiveLocation", request.Lat, request.Lng);

            return Response<bool>.Success(true, "تم حفظ الموقع وإرساله.");
        }
    }
}
