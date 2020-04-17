using System;
using Microsoft.AspNetCore.Mvc;
using PCPersonnel.Models;
using PCPersonnel.Services;

namespace PCPersonnel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RollCallController : ControllerBase
    {
        private readonly IRollCallService _rollCallService;

        public RollCallController(IRollCallService rollCallService)
        {
            this._rollCallService = rollCallService;
        }

        [HttpGet]
        [Route("{id}")]
        public RollCall Get(string id, string date = null)
        {
            DateTime dt;
            if (date == null || !DateTime.TryParse(date, out dt))
            {
                dt = DateTime.Now.Date;
            }
            var result = this._rollCallService.GetByEntryAndDate(id, dt);
            return result;
        }
    }
}
