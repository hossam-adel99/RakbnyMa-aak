using AutoMapper;
using RakbnyMa_aak.CQRS.Commands.Validations.ValidateTripExists;
using RakbnyMa_aak.CQRS.Features.BookingFeatures.Orchestrators.BookValidationOrchestrator;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.CQRS.Features.Governorates;
using RakbnyMa_aak.CQRS.Queries.GetMessagesByTripId;
using RakbnyMa_aak.DTOs.Auth.RequestDTOs;
using RakbnyMa_aak.DTOs.BookingsDTOs.Requests;
using RakbnyMa_aak.DTOs.BookingsDTOs.ResponseDTOs;
using RakbnyMa_aak.DTOs.DriverDTOs.ResponseDTOs;
using RakbnyMa_aak.DTOs.Shared;
using RakbnyMa_aak.DTOs.TripDTOs.RequestsDTOs;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.DTOs.UserDTOs;
using RakbnyMa_aak.Models;
using static RakbnyMa_aak.Utilities.Enums;
namespace RakbnyMa_aak.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<RegisterUserRequestDto, ApplicationUser>().ReverseMap();
            CreateMap<TripRequestDto, Trip>().ReverseMap();
            CreateMap<CreateBookingRequestDto, Booking>().ReverseMap();
            CreateMap<GovernorateDto, Governorate>().ReverseMap();
            CreateMap<CityDto, City>().ReverseMap();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.FullName));

            CreateMap<City, CityResponseDTO>()
                .ForMember(dest => dest.GovernorateName, opt => opt.MapFrom(src => src.Governorate.Name));
            CreateMap<City, CityDto>()
                .ForMember(dest => dest.GovernorateName, opt => opt.MapFrom(src => src.Governorate.Name));
            CreateMap<Governorate, GovernorateResponseDTO>();
            CreateMap<GovernorateDto, Governorate>();

            CreateMap<ApplicationUser, UserResponseDto>()
                .ForMember(dest => dest.TotalTrips,
                    opt => opt.MapFrom(src => src.Bookings
                        .Count(b => b.RequestStatus == RequestStatus.Confirmed)))

                .ForMember(dest => dest.AverageRating,
                    opt => opt.MapFrom(src =>
                        src.RatingsReceived != null && src.RatingsReceived.Any()
                            ? src.RatingsReceived.Average(r => r.RatingValue)
                            : 0));


            CreateMap<ApplicationUser, DriverResponseDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => src.Picture))

            .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.Driver.CarModel))
            .ForMember(dest => dest.CarType, opt => opt.MapFrom(src => src.Driver.CarType))
            .ForMember(dest => dest.CarColor, opt => opt.MapFrom(src => src.Driver.CarColor))
            .ForMember(dest => dest.CarCapacity, opt => opt.MapFrom(src => src.Driver.CarCapacity))
            .ForMember(dest => dest.CarPlateNumber, opt => opt.MapFrom(src => src.Driver.CarPlateNumber))
            .ForMember(dest => dest.CarLicenseImage, opt => opt.MapFrom(src => src.Driver.CarLicenseImagePath))
            .ForMember(dest => dest.DriverLicenseImage, opt => opt.MapFrom(src => src.Driver.DriverNationalIdImagePath))

            .ForMember(dest => dest.TotalTrips, opt => opt.MapFrom(src =>
                src.Driver.Trips != null ? src.Driver.Trips.Count : 0))

            .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                src.RatingsReceived != null && src.RatingsReceived.Any()
                    ? src.RatingsReceived.Average(r => r.RatingValue)
                    : 0));


            CreateMap<Booking, CreateBookingResponseDto>()
            .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PassengerId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));

            CreateMap<BookTripRequestDto, CreateBookingRequestDto>()
            .ForMember(dest => dest.PricePerSeat, opt => opt.Ignore());

            //CreateMap<Trip, TripResponseDto>()
            //  .ForMember(dest => dest.FromCityName, opt => opt.MapFrom(src => src.FromCity.Name))
            //  .ForMember(dest => dest.ToCityName, opt => opt.MapFrom(src => src.ToCity.Name))
            //  .ForMember(dest => dest.FromGovernorateName, opt => opt.MapFrom(src => src.FromGovernorate.Name))
            //  .ForMember(dest => dest.ToGovernorateName, opt => opt.MapFrom(src => src.ToGovernorate.Name))
            //  .ForMember(dest => dest.TripStatus, opt => opt.MapFrom(src => src.TripStatus.ToString()));

            CreateMap<Booking, BookingStatusResponseDto>()
               .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.TripID, opt => opt.MapFrom(src => src.TripId))
               .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
               .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));


            //CreateMap<Trip, TripResponseDto>()
            //  .ForMember(dest => dest.DriverFullName, opt => opt.MapFrom(src => src.Driver.User.FullName))
            //  .ForMember(dest => dest.FromCityName, opt => opt.MapFrom(src => src.FromCity.Name))
            //  .ForMember(dest => dest.ToCityName, opt => opt.MapFrom(src => src.ToCity.Name))
            //  .ForMember(dest => dest.FromGovernorateName, opt => opt.MapFrom(src => src.FromGovernorate.Name))
            //  .ForMember(dest => dest.ToGovernorateName, opt => opt.MapFrom(src => src.ToGovernorate.Name))
            //  .ForMember(dest => dest.TripStatus, opt => opt.MapFrom(src => src.TripStatus.ToString()));

            CreateMap<Trip, TripResponseDto>()
                .ForMember(dest => dest.DriverFullName,opt => opt.MapFrom(src => src.Driver.User.FullName))
                .ForMember(dest => dest.DriverPicture,opt => opt.MapFrom(src => src.Driver.User.Picture))
                .ForMember(dest => dest.DriverRate,opt => opt.MapFrom(src =>src.Driver.User.RatingsReceived.Any()
                     ? src.Driver.User.RatingsReceived.Average(r => r.RatingValue).ToString("0.0"): "0.0"))
                .ForMember(dest => dest.PickupLocation,opt => opt.MapFrom(src => src.PickupLocation))
                .ForMember(dest => dest.DestinationLocation,opt => opt.MapFrom(src => src.DestinationLocation))
                .ForMember(dest => dest.TripDate,opt => opt.MapFrom(src => src.TripDate))
                .ForMember(dest => dest.AvailableSeats,opt => opt.MapFrom(src => src.AvailableSeats))
                .ForMember(dest => dest.PricePerSeat,opt => opt.MapFrom(src => src.PricePerSeat))
                .ForMember(dest => dest.FromCityName,opt => opt.MapFrom(src => src.FromCity.Name))
                .ForMember(dest => dest.ToCityName,opt => opt.MapFrom(src => src.ToCity.Name))
                .ForMember(dest => dest.FromGovernorateName, opt => opt.MapFrom(src => src.FromCity.Governorate.Name))
                .ForMember(dest => dest.ToGovernorateName, opt => opt.MapFrom(src => src.ToCity.Governorate.Name))
                .ForMember(dest => dest.TripStatus,opt => opt.MapFrom(src => src.TripStatus.ToString()))
                .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.Driver.CarModel))
                .ForMember(dest => dest.IsRecurring,opt => opt.MapFrom(src => src.IsRecurring))
                .ForMember(dest => dest.WomenOnly,opt => opt.MapFrom(src => src.WomenOnly));




            CreateMap<Trip, TripReportDto>()
                .ForMember(dest => dest.TripId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DriverFullName, opt => opt.MapFrom(src => src.Driver.User.FullName))
                .ForMember(dest => dest.FromCityName, opt => opt.MapFrom(src => src.FromCity.Name))
                .ForMember(dest => dest.FromGovernorateName, opt => opt.MapFrom(src => src.FromGovernorate.Name))
                .ForMember(dest => dest.ToCityName, opt => opt.MapFrom(src => src.ToCity.Name))
                .ForMember(dest => dest.ToGovernorateName, opt => opt.MapFrom(src => src.ToGovernorate.Name))
                .ForMember(dest => dest.TripDate, opt => opt.MapFrom(src => src.TripDate))
                .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(src => src.AvailableSeats))
                .ForMember(dest => dest.PricePerSeat, opt => opt.MapFrom(src => src.PricePerSeat))
                .ForMember(dest => dest.TripStatus, opt => opt.MapFrom(src => src.TripStatus.ToString()))
                .ForMember(dest => dest.WomenOnly, opt => opt.MapFrom(src => src.WomenOnly))
                .ForMember(dest => dest.IsRecurring, opt => opt.MapFrom(src => src.IsRecurring));


            //CreateMap<(Booking booking, Trip trip), BookingValidationResultDto>()
            //   .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.booking.Id))
            //   .ForMember(dest => dest.TripId, opt => opt.MapFrom(src => src.trip.Id))
            //   .ForMember(dest => dest.PassengerId, opt => opt.MapFrom(src => src.booking.UserId))
            //   .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.trip.DriverId))
            //   .ForMember(dest => dest.NumberOfSeats, opt => opt.MapFrom(src => src.booking.NumberOfSeats))
            //   .ForMember(dest => dest.requestStatus, opt => opt.MapFrom(src => src.booking.RequestStatus))
            //   .ForMember(dest => dest.DriverFullName, opt => opt.MapFrom(src => src.trip.Driver.User.FullName))
            //   .ForMember(dest => dest.DriverPicture, opt => opt.MapFrom(src => src.trip.Driver.User.Picture));

            CreateMap<(Booking booking, TripValidationResultDto validation), BookingValidationResultDto>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.booking.Id))
                .ForMember(dest => dest.TripId, opt => opt.MapFrom(src => src.booking.TripId))
                .ForMember(dest => dest.PassengerId, opt => opt.MapFrom(src => src.booking.UserId))
                .ForMember(dest => dest.DriverId, opt => opt.MapFrom(src => src.booking.Trip.DriverId))
                .ForMember(dest => dest.NumberOfSeats, opt => opt.MapFrom(src => src.booking.NumberOfSeats))
                .ForMember(dest => dest.requestStatus, opt => opt.MapFrom(src => src.booking.RequestStatus))
                .ForMember(dest => dest.DriverFullName, opt => opt.MapFrom(src => src.booking.Trip.Driver.User.FullName))
                .ForMember(dest => dest.DriverPicture, opt => opt.MapFrom(src => src.booking.Trip.Driver.User.Picture));

        }
    }
}