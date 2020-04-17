using System;
using PCPersonnel.Models;

namespace PCPersonnel.Services
{
    public interface IRollCallService
    {
        RollCall GetByEntryAndDate(string entry, DateTime date);
        RollCallOptions GetRollCallOptions(DateTime date);
    }
}
