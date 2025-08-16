using AutoMapper;
using MediatR;
using RakbnyMa_aak.CQRS.Features.Governorates;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Features.Governorates.GetGovernorateByName
{
    public class GetGovernorateByNameHandler : IRequestHandler<GetGovernorateByNameQuery, Response<GovernorateDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetGovernorateByNameHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<GovernorateDto>> Handle(GetGovernorateByNameQuery request, CancellationToken cancellationToken)
        {
            var dto = await _unitOfWork.GovernorateRepository
                .GetProjectedSingleAsync<GovernorateDto>(
                    predicate: g => g.Name.ToLower() == request.Name.ToLower() && !g.IsDeleted,
                    mapper: _mapper,
                    cancellationToken: cancellationToken
                );

            if (dto is null)
                return Response<GovernorateDto>.Fail("لم يتم العثور على المحافظة.");

            return Response<GovernorateDto>.Success(dto);
        }
    }
}
