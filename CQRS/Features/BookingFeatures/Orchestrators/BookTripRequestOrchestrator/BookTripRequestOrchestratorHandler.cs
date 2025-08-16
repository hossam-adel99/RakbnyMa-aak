using AutoMapper;
using MediatR;
using RakbnyMa_aak.CQRS.Commands.SendNotification;
using RakbnyMa_aak.CQRS.Commands.Validations.CheckUserAlreadyBooked;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripExists;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.CreateBooking;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Commands.PreventDriverSelfBooking;
using RakbnyMa_aak.CQRS.Trips.Queries;
using RakbnyMa_aak.DTOs.BookingsDTOs.Requests;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.BookTripRequest
{
    public class BookTripRequestOrchestratorHandler : IRequestHandler<BookTripRequestOrchestrator, Response<int>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookTripRequestOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(BookTripRequestOrchestrator request, CancellationToken cancellationToken)
        {
            request.BookingDto.UserId = request.userId;
            var bookingDto = request.BookingDto;

            // Step 1: Validate Trip Exists
            var validateTripResponse = await _mediator.Send(new ValidateTripExistsCommand(bookingDto.TripId));
            if (!validateTripResponse.IsSucceeded)
                return Response<int>.Fail(validateTripResponse.Message);

            var trip = validateTripResponse.Data;

            // Step 2: prevent Driver From Booking From himself 
            var preventBookingResponse = await _mediator.Send(new PreventDriverSelfBookingCommand(trip.DriverId, request.userId));
            if (!preventBookingResponse.IsSucceeded)
                return Response<int>.Fail(preventBookingResponse.Message);

            if (bookingDto.NumberOfSeats > trip.AvailableSeats)
                return Response<int>.Fail($"عدد المقاعد المتاحة غير كافٍ. المتبقي فقط {trip.AvailableSeats} مقعداً.");

            // Step 3: Check if user already booked the same trip
            var checkBookingResponse = await _mediator.Send(
                new CheckUserAlreadyBookedCommand(
                    new CheckUserAlreadyBookedDto
                    {
                        UserId = request.userId,
                        TripId = bookingDto.TripId
                    }));

            if (!checkBookingResponse.IsSucceeded)
                return Response<int>.Fail(checkBookingResponse.Message);

            Response<int> bookingResponse;

            if (checkBookingResponse.Data) // already booked → can't book again, but can update existing booking
            {
                // Get booking by user and trip
                var existingBooking = await _unitOfWork.BookingRepository
                    .GetBookingByUserAndTripAsync(request.userId, bookingDto.TripId);

                if (existingBooking == null)
                    return Response<int>.Fail("الحجز الحالي غير موجود.");

                return Response<int>.Fail("يمكنك إجراء حجز واحد فقط، يرجى تعديل الحجز الحالي.");
            }
            else // first time booking → create
            {
                var createBookingDto = _mapper.Map<CreateBookingRequestDto>(bookingDto);

                createBookingDto.PricePerSeat = trip.PricePerSeat;

                bookingResponse = await _mediator.Send(new CreateBookingCommand(createBookingDto));
            }

            if (!bookingResponse.IsSucceeded)
                return bookingResponse;

            // Step 4: Get DriverId by TripId
            var driverIdResponse = await _mediator.Send(new GetDriverIdByTripIdQuery(bookingDto.TripId));
            if (!driverIdResponse.IsSucceeded || string.IsNullOrEmpty(driverIdResponse.Data))
                return Response<int>.Fail("لم يتم العثور على السائق لهذه الرحلة.");

            var driverId = driverIdResponse.Data;

            // Step 5: Send Notification to Driver
            await _mediator.Send(new SendNotificationCommand(new SendNotificationDto
            {
                ReceiverId = driverId,
                SenderUserId = request.userId,
                Message = "لديك طلب حجز جديد."
            }));

            return bookingResponse;
        }
    }
}
