using System;
using Microsoft.AspNetCore.Mvc;
using PCPersonnel.Models;
using PCPersonnel.Services;

namespace PCPersonnel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RollCallOptionsController : ControllerBase
    {
        private readonly IRollCallService _rollCallService;
        public RollCallOptionsController(IRollCallService rollCallService)
        {
            this._rollCallService = rollCallService;
        }

        [HttpGet]
        [Route("{id}")]
        public RollCallOptions Get(string id)
        {
            DateTime dt;
            if (id == null || !DateTime.TryParse(id, out dt))
            {
                return null;
            }
            var options = this._rollCallService.GetRollCallOptions(dt.Date);
            return options;
        }
    }
}
