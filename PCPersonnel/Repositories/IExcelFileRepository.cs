using System;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;

namespace PCPersonnel.Repositories
{
    public interface IExcelFileRepository
    {
        Task StoreExcelFile(IFormFile excelFile);

        T ReadExcelFile<T>(Func<SpreadsheetDocument, T> reader);

        string GetStringValue(Cell c, SpreadsheetDocument document);

        DateTime? GetDateValue(Cell c, SpreadsheetDocument document);

        void RegisterExcelFileUploadedCallback(Action<byte[]> newExcelFile);
    }
}
