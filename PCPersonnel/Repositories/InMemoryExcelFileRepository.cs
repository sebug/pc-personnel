using System;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;

namespace PCPersonnel.Repositories
{
    public class InMemoryExcelFileRepository : IExcelFileRepository
    {
        private byte[] FileContent { get; set; }

        public InMemoryExcelFileRepository()
        {
        }

        public async Task StoreExcelFile(IFormFile excelFile)
        {
            if (excelFile == null)
            {
                return;
            }
            using (var ms = new MemoryStream())
            {
                await excelFile.CopyToAsync(ms);
                this.FileContent = ms.ToArray();
            }
        }

        public T ReadExcelFile<T>(Func<SpreadsheetDocument, T> reader)
        {
            if (this.FileContent == null)
            {
                throw new Exception("Master file has not been uploaded yet.");
            }
            using (var ms = new MemoryStream(this.FileContent))
            {
                using (var spreadsheet = SpreadsheetDocument.Open(ms, false))
                {
                    return reader(spreadsheet);
                }
            }
        }
    }
}
