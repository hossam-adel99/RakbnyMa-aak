using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.CQRS.Features.Auth.Queries.GetDriverById;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.DTOs.DriverDTOs.ResponseDTOs;
using RakbnyMa_aak.GeneralResponse;

namespace RakbnyMa_aak.CQRS.Features.Drivers.Queries
{
    public class GetDriverByIdQueryHandler : IRequestHandler<GetDriverByIdQuery, Response<DriverResponseDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetDriverByIdQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<DriverResponseDto>> Handle(GetDriverByIdQuery request, CancellationToken cancellationToken)
        {
            var driverDto = await _context.Users
                .Where(u => u.Id == request.DriverId && u.UserType == Utilities.Enums.UserType.Driver)
                .Include(u => u.Driver).ThenInclude(d => d.Trips)
                .Include(u => u.RatingsReceived)
                .ProjectTo<DriverResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (driverDto is null)
                return Response<DriverResponseDto>.Fail("لم يتم العثور على السائق");

            return Response<DriverResponseDto>.Success(driverDto);
        }
    }
}
