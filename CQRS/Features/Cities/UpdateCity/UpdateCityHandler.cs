using AutoMapper;
using MediatR;
using RakbnyMa_aak.CQRS.Features.Cities;
using RakbnyMa_aak.CQRS.Features.Cities.UpdateCity;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.UOW;

public class UpdateCityHandler : IRequestHandler<UpdateCityCommand, Response<CityResponseDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCityHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<CityResponseDTO>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(request.Dto.Id);

        if (city is null || city.IsDeleted)
            return Response<CityResponseDTO>.Fail("لم يتم العثور على المدينة.");

        var isDuplicate = await _unitOfWork.CityRepository.AnyAsync(
            c => c.Id != request.Dto.Id &&
                 c.Name.ToLower() == request.Dto.Name.ToLower() &&
                 c.GovernorateId == request.Dto.GovernorateId);

        if (isDuplicate)
            return Response<CityResponseDTO>.Fail("مدينة أخرى بنفس الاسم موجودة بالفعل في نفس المحافظة.");

        city.Name = request.Dto.Name;
        city.GovernorateId = request.Dto.GovernorateId;
        city.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.CityRepository.Update(city);
        await _unitOfWork.CompleteAsync();
        var responseDto = _mapper.Map<CityResponseDTO>(city);

        return Response<CityResponseDTO>.Success(responseDto, "تم تحديث المدينة بنجاح.");
    }
}
