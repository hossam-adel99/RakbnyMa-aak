using AutoMapper;
using MediatR;
using RakbnyMa_aak.CQRS.Features.Governorates;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Governorates.CreateGovernorate
{
    public class CreateGovernorateHandler : IRequestHandler<CreateGovernorateCommand, Response<GovernorateResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateGovernorateHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<GovernorateResponseDTO>> Handle(CreateGovernorateCommand request, CancellationToken cancellationToken)
        {
            var normalizedName = request.Dto.Name.Trim().ToLower();

            var isExist = await _unitOfWork.GovernorateRepository
                .AnyAsync(g => g.Name.ToLower() == normalizedName);

            if (isExist)
            {
                return Response<GovernorateResponseDTO>.Fail("هذه المحافظة موجودة بالفعل");
            }

            var entity = _mapper.Map<Governorate>(request.Dto);
            entity.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.GovernorateRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            var responseDto = _mapper.Map<GovernorateResponseDTO>(entity);
            return Response<GovernorateResponseDTO>.Success(responseDto, "تم إنشاء المحافظة بنجاح");
        }
    }
}
