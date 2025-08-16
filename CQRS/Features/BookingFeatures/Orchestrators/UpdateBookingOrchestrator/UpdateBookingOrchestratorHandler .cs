using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.UpdateConfirmedBooking;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.UpdatePendingBooking;
using RakbnyMa_aak.DTOs.BookingsDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.UpdateBookingOrchestrator
{
    public class UpdateBookingOrchestratorHandler : IRequestHandler<UpdateBookingOrchestrator, Response<UpdateBookingSeatsResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public UpdateBookingOrchestratorHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Response<UpdateBookingSeatsResponseDto>> Handle(UpdateBookingOrchestrator request, CancellationToken cancellationToken)
        {
            var dto = request.BookingDto;

            var bookingInfo = await _unitOfWork.BookingRepository
                .GetAllQueryable()
                .Where(b => b.Id == dto.BookingId && b.UserId == request.userId)
                .Select(b => new { OldSeats = b.NumberOfSeats, b.RequestStatus })
                .FirstOrDefaultAsync(cancellationToken);

            if (bookingInfo == null)
                return Response<UpdateBookingSeatsResponseDto>.Fail("لم يتم العثور على الحجز أو ليس لديك صلاحية الوصول");

            var difference = dto.NewNumberOfSeats - bookingInfo.OldSeats;
            if (difference == 0)
                return Response<UpdateBookingSeatsResponseDto>.Fail("لم يتم تعديل عدد المقاعد");

            if (bookingInfo.RequestStatus == RequestStatus.Confirmed)
            {
                return await _mediator.Send(new UpdateConfirmedBookingCommand(dto, bookingInfo.OldSeats, request.userId));
            }
            else
            {
                return await _mediator.Send(new UpdatePendingBookingCommand(dto, bookingInfo.OldSeats, request.userId));
            }
        }
    }
}
