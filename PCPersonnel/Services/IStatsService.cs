using System;
using PCPersonnel.Models;

namespace PCPersonnel.Services
{
    public interface IStatsService
    {
        StatsByDate GetStatsByDate(DateTime date);
    }
}
