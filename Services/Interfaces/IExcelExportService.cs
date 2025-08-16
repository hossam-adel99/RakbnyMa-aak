namespace RakbnyMa_aak.Services.Interfaces
{
    public interface IExcelExportService
    {
        byte[] ExportToExcel<T>(List<T> data, string sheetName = "Sheet1");
    }
}
