using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCPersonnel.Repositories;

namespace PCPersonnel.Pages
{
    public class IndexModel : PageModel
    {
        private IExcelFileRepository ExcelFileRepository { get; }

        public IndexModel(IExcelFileRepository excelFileRepository)
        {
            this.ExcelFileRepository = excelFileRepository;
        }

        public void OnGet()
        {
        }

        public async Task OnPostAsync(IFormFile excelFile)
        {
            await this.ExcelFileRepository.StoreExcelFile(excelFile);
        }
    }
}
