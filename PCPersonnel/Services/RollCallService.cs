using System;
using System.Collections.Generic;
using PCPersonnel.Models;

namespace PCPersonnel.Services
{
    public class RollCallService : IRollCallService
    {
        public RollCall GetByEntryAndDate(string entry, DateTime date)
        {
            var result = new RollCall();
            result.Entry = entry;
            result.Date = date;
            result.Presences = new List<RollCallPerson>();

            return result;
        }
    }
}
