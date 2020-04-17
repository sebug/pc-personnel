using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCPersonnel.Models;
using PCPersonnel.Services;

namespace PCPersonnel.Pages
{
    public class RollCallModel : PageModel
    {
        private readonly IRollCallService _rollCallService;
        public RollCallModel(IRollCallService rollCallService)
        {
            this._rollCallService = rollCallService;
        }

        public RollCall RollCall { get; set; }

        public void OnGet(string date, string placeOfEntry)
        {
            DateTime dt;
            if (date == null ||
                !DateTime.TryParse(date, out dt))
            {
                dt = DateTime.Now.Date;
                this.RollCall = this._rollCallService.GetByEntryAndDate(placeOfEntry, dt);
            }
            else
            {
                this.RollCall = this._rollCallService.GetByEntryAndDate(placeOfEntry, dt.Date);
            }
        }
    }
}
