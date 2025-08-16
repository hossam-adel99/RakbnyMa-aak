using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RakbnyMa_aak.DTOs.Shared;
using RakbnyMa_aak.Services.Interfaces;
using RakbnyMa_aak.UOW;

namespace RakbnyMa_aak.Services.Implementations
{
    public class AdminReportService
    {
        private readonly IExcelExportService _excelExportService;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AdminReportService(IExcelExportService excelExportService, IUnitOfWork uow, IMapper mapper)
        {
            _excelExportService = excelExportService;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<byte[]> ExportTripReport(DateTime fromDate, DateTime toDate)
        {
            var trips = await _uow.TripRepository.GetAllQueryable()
                .Where(t => t.TripDate >= fromDate && t.TripDate <= toDate)
                .ProjectTo<TripReportDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return _excelExportService.ExportToExcel(trips, "Trip Report");
        }

    }
}

