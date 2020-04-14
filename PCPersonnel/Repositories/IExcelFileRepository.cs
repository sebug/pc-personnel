using System;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;

namespace PCPersonnel.Repositories
{
    public interface IExcelFileRepository
    {
        Task StoreExcelFile(IFormFile excelFile);

        T ReadExcelFile<T>(Func<SpreadsheetDocument, T> reader);
    }
}
