using AutoMapper;
using MediatR;
using RakbnyMa_aak.DTOs.UserDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;

namespace RakbnyMa_aak.CQRS.Features.Auth.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Response<UserResponseDto>>
    {
        private readonly IGenericRepository<ApplicationUser> _userRepo;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IGenericRepository<ApplicationUser> userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<Response<UserResponseDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var userDto = await _userRepo.GetProjectedSingleAsync<UserResponseDto>(
                u => u.Id == request.Id,
                _mapper,
                cancellationToken);

            if (userDto == null)
                return Response<UserResponseDto>.Fail("لم يتم العثور على المستخدم");

            return Response<UserResponseDto>.Success(userDto);
        }
    }
}
