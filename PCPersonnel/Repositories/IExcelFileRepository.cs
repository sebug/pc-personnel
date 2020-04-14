using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PCPersonnel.Repositories
{
    public interface IExcelFileRepository
    {
        Task StoreExcelFile(IFormFile excelFile);
    }
}
