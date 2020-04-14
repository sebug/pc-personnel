using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PCPersonnel.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task OnPostAsync(IFormFile excelFile)
        {
            if (excelFile == null)
            {
                return;
            }
            using (var ms = new MemoryStream())
            {
                await excelFile.CopyToAsync(ms);
            }
        }
    }
}
