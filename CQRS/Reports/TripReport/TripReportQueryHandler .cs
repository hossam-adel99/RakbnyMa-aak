using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.Shared;
using RakbnyMa_aak.GeneralResponse;
using RakbnyMa_aak.Services.Interfaces;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.CQRS.Reports.TripReport
{
    public class TripReportQueryHandler : IRequestHandler<TripReportQuery, Response<byte[]>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IExcelExportService _excelService;

        public TripReportQueryHandler(IUnitOfWork uow, IMapper mapper, IExcelExportService excelService)
        {
            _uow = uow;
            _mapper = mapper;
            _excelService = excelService;
        }

        public async Task<Response<byte[]>> Handle(TripReportQuery request, CancellationToken cancellationToken)
        {
            var trips = await _uow.TripRepository.GetAllQueryable()
                .Where(t => t.TripDate >= request.FromDate && t.TripDate <= request.ToDate)
                .ProjectTo<TripReportDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var fileBytes = _excelService.ExportToExcel(trips, "Trip Report");

            return Response<byte[]>.Success(fileBytes, "تم إنشاء ملف الإكسل بنجاح");
        }
    }
}
