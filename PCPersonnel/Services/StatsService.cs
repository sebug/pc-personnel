using System;
using PCPersonnel.Models;
using PCPersonnel.Repositories;
using System.Linq;

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

            var personsAndPresence = this._personRepository.GetAll()
                .Select(p => new
                {
                    Person = p,
                    Presence = p.Presences.FirstOrDefault(p => p.Date.Date == date.Date)
                }).ToList();

            var presents = personsAndPresence
                .Where(pp => pp.Presence != null && pp.Presence.Called &&
                (String.IsNullOrEmpty(pp.Presence.Presence) ||
                pp.Presence.Presence.Equals("P", StringComparison.InvariantCultureIgnoreCase)));

            var rests = personsAndPresence
                .Where(pp => pp.Presence != null && pp.Presence.Called &&
                pp.Presence.Presence != null &&
                pp.Presence.Presence.IndexOf("R", StringComparison.InvariantCultureIgnoreCase) >= 0);

            result.PresentCount = presents
                .Count();
            result.RestCount = rests.Count();

            result.MissionCount = presents.GroupBy(pp => pp.Person.Mission)
                .ToDictionary(g => g.Key, g => g.Count());

            return result;
        }
    }
}
