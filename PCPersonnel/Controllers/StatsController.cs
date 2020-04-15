using System;
using Microsoft.AspNetCore.Mvc;
using PCPersonnel.Models;
using PCPersonnel.Services;

namespace PCPersonnel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;
        public StatsController(IStatsService statsService)
        {
            this._statsService = statsService;
        }

        [HttpGet]
        [Route("{id}")]
        public StatsByDate Get(string id)
        {
            DateTime dt;
            if (!DateTime.TryParse(id, out dt))
            {
                return null;
            }
            return this._statsService.GetStatsByDate(dt.Date);
        }
    }
}
