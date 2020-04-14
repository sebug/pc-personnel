using System;
using System.IO;
using System.Threading.Tasks;
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
    }
}
