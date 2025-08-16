using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.CQRS.Features.Governorates;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Governorates.GetGovernorateById
{
    public class GetGovernorateByIdHandler : IRequestHandler<GetGovernorateByIdQuery, Response<GovernorateDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetGovernorateByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<GovernorateDto>> Handle(GetGovernorateByIdQuery request, CancellationToken cancellationToken)
        {
            var dto = await _unitOfWork.GovernorateRepository
                .GetProjectedSingleAsync<GovernorateDto>(
                    g => g.Id == request.Id && !g.IsDeleted,
                    _mapper,
                    cancellationToken
                );

            if (dto is null)
                return Response<GovernorateDto>.Fail("لم يتم العثور على المحافظة.");

            return Response<GovernorateDto>.Success(dto);
        }
    }
}
