using System;
using PCPersonnel.Models;
using PCPersonnel.Repositories;

namespace PCPersonnel.Services
{
    public class StatsService : IStatsService
    {
        private readonly IPersonRepository _personRepository;

        public StatsService(IPersonRepository personRepository)
        {
            this._personRepository = personRepository;
        }

        public StatsByDate GetStatsByDate(DateTime date)
        {
            var result = new StatsByDate();
            result.FormattedDate = date.ToString("dd.MM.yyyy");
            return result;
        }
    }
}
