using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using RakbnyMa_aak.DTOs.BookingsDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.SignalR;
using RakbnyMa_aak.UOW;
using BookingModel = RakbnyMa_aak.Models.Booking;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Response<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CreateBookingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
            IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<Response<int>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = _mapper.Map<BookingModel>(request.BookingDto);

            await _unitOfWork.BookingRepository.AddAsync(booking);

            await _unitOfWork.CompleteAsync();

            //Notification but i will do orchestrator

            var responseDto = _mapper.Map<CreateBookingResponseDto>(booking);

            return Response<int>.Success(responseDto.BookingId, "تم إرسال طلب الحجز إلى السائق.");
        }
    }
}
