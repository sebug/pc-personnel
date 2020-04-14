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
        [BindProperty]
        IFormFile ExcelFile { get; set; }

        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            if (this.ExcelFile == null)
            {
                return;
            }
            using (var ms = new MemoryStream())
            {
                await this.ExcelFile.CopyToAsync(ms);
            }
        }
    }
}
