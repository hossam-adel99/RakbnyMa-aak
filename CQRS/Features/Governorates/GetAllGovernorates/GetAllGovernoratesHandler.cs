using AutoMapper;
using MediatR;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.CQRS.Features.Governorates;
using RakbnyMa_aak.CQRS.Features.Governorates.GetAllGovernorates;

public class GetAllGovernoratesHandler : IRequestHandler<GetAllGovernoratesQuery, Response<PaginatedResult<GovernorateDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllGovernoratesHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<PaginatedResult<GovernorateDto>>> Handle(GetAllGovernoratesQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.GovernorateRepository.GetAllQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var pagedItems = await query
            .Skip((request.RequestDto.Page - 1) * request.RequestDto.PageSize)
            .Take(request.RequestDto.PageSize)
            .ToListAsync(cancellationToken);

        var result = new PaginatedResult<GovernorateDto>(
            items: _mapper.Map<List<GovernorateDto>>(pagedItems),
            totalCount: totalCount,
            page: request.RequestDto.Page,
            pageSize: request.RequestDto.PageSize
        );

        return Response<PaginatedResult<GovernorateDto>>.Success(result);
    }
}
