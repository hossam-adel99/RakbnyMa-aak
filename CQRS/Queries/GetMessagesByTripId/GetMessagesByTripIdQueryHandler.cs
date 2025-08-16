using AutoMapper;
using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using System.Security.Claims;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Queries.GetMessagesByTripId
{
    public class GetMessagesByTripIdQueryHandler : IRequestHandler<GetMessagesByTripIdQuery, Response<List<MessageDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMessagesByTripIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response<List<MessageDto>>> Handle(GetMessagesByTripIdQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = user?.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Response<List<MessageDto>>.Fail("غير مصرح لك بالوصول.");

            var trip = await _unitOfWork.TripRepository.GetByIdAsync(request.TripId);
            if (trip == null)
                return Response<List<MessageDto>>.Fail("لم يتم العثور على الرحلة.");

            var approvedPassengerIds = await _unitOfWork.BookingRepository
                .FindAllAsync(b => b.TripId == request.TripId && b.RequestStatus == RequestStatus.Confirmed);
            var passengerIds = approvedPassengerIds.Select(b => b.UserId).ToList();

            var isDriver = trip.DriverId == userId;
            var isPassenger = passengerIds.Contains(userId);
            var isAdmin = userRole == "Admin"; 

            if (!isDriver && !isPassenger && !isAdmin)
                return Response<List<MessageDto>>.Fail("غير مصرح لك بعرض رسائل هذه الرحلة.");

            var messages = await _unitOfWork.MessageRepository.GetMessagesByTripIdAsync(request.TripId);
            var messageDtos = _mapper.Map<List<MessageDto>>(messages);

            return Response<List<MessageDto>>.Success(messageDtos);
        }

    }



}
