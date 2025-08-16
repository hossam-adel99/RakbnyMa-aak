using AutoMapper;
using MediatR;
using RakbnyMa_aak.CQRS.Features.Governorates;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Governorates.UpdateGovernorate
{
    public class UpdateGovernorateHandler : IRequestHandler<UpdateGovernorateCommand, Response<GovernorateDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateGovernorateHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<GovernorateDto>> Handle(UpdateGovernorateCommand request, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.GovernorateRepository.GetByIdAsync(request.Dto.Id);

            if (entity is null || entity.IsDeleted)
                return Response<GovernorateDto>.Fail("لم يتم العثور على المحافظة.");

            var isDuplicate = await _unitOfWork.GovernorateRepository.AnyAsync(
                g => g.Id != request.Dto.Id && g.Name.ToLower() == request.Dto.Name.ToLower());

            if (isDuplicate)
                return Response<GovernorateDto>.Fail("توجد محافظة أخرى بنفس الاسم بالفعل.");

            _mapper.Map(request.Dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.GovernorateRepository.Update(entity);
            await _unitOfWork.CompleteAsync();

            var dto = _mapper.Map<GovernorateDto>(entity);

            return Response<GovernorateDto>.Success(dto, "تم تحديث بيانات المحافظة بنجاح.");
        }
    }
}
