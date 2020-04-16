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
    public class StatsModel : PageModel
    {
        private readonly IStatsService _statsService;

        public StatsModel(IStatsService statsService)
        {
            this._statsService = statsService;
        }

        public StatsByDate StatsByDate { get; set; }

        public void OnGet(string date)
        {
            DateTime dt;
            if (date != null && DateTime.TryParse(date, out dt))
            {
                this.StatsByDate = this._statsService.GetStatsByDate(dt);
            }
        }
    }
}
