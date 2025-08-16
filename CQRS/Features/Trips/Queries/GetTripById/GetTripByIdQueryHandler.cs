using AutoMapper;
using MediatR;
using RakbnyMa_aak.DTOs.TripDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Trips.Queries.GetTripById
{
    public class GetTripByIdQueryHandler : IRequestHandler<GetTripByIdQuery, Response<TripResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTripByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<TripResponseDto>> Handle(GetTripByIdQuery request, CancellationToken cancellationToken)
        {
            var tripDto = await _unitOfWork.TripRepository.GetProjectedSingleAsync<TripResponseDto>(
                predicate: t => t.Id == request.TripId && !t.IsDeleted,
                mapper: _mapper,
                cancellationToken: cancellationToken);

            if (tripDto == null)
                return Response<TripResponseDto>.Fail("لم يتم العثور على الرحلة", statusCode: 404);

            return Response<TripResponseDto>.Success(tripDto);
        }
    }
}
