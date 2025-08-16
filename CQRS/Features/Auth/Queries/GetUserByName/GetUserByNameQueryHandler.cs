// Handler
using AutoMapper;
using MediatR;
using RakbnyMa_aak.DTOs.UserDTOs;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Repositories.Interfaces;

namespace RakbnyMa_aak.CQRS.Features.Auth.Queries.GetUserByName
{
    public class GetUserByNameQueryHandler : IRequestHandler<GetUserByNameQuery, Response<UserResponseDto>>
    {
        private readonly IGenericRepository<ApplicationUser> _userRepo;
        private readonly IMapper _mapper;

        public GetUserByNameQueryHandler(IGenericRepository<ApplicationUser> userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<Response<UserResponseDto>> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return Response<UserResponseDto>.Fail("اسم المستخدم مطلوب.");

            var userDto = await _userRepo.GetProjectedSingleAsync<UserResponseDto>(
              u => u.FullName != null && u.FullName.ToUpper() == request.Name.ToUpper(),
             _mapper,
             cancellationToken);


            if (userDto == null)
                return Response<UserResponseDto>.Fail("لم يتم العثور على مستخدم بهذا الاسم.");

            return Response<UserResponseDto>.Success(userDto);
        }
    }
}
