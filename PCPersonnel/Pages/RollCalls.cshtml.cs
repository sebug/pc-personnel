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
    public class RollCallsModel : PageModel
    {
        private readonly IRollCallService _rollCallService;
        public RollCallsModel(IRollCallService rollCallService)
        {
            this._rollCallService = rollCallService;
        }

        public RollCallOptions RollCallOptions { get; set; }

        public void OnGet(string date)
        {
            DateTime dt;
            if (date == null ||
                !DateTime.TryParse(date, out dt))
            {
                this.RollCallOptions = this._rollCallService.GetRollCallOptions(
                    DateTime.Now.Date);
            }
            else
            {
                this.RollCallOptions = this._rollCallService.GetRollCallOptions(
                    dt);
            }
        }
    }
}
