using System;
using PCPersonnel.Models;
using PCPersonnel.Repositories;
using System.Linq;
using System.Text.RegularExpressions;

namespace PCPersonnel.Services
{
    public class StatsService : IStatsService
    {
        private readonly IPersonRepository _personRepository;

        public StatsService(IPersonRepository personRepository)
        {
            this._personRepository = personRepository;
        }

        private readonly Regex _numbersOnlyRegex = new Regex("^\\d+$");

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
                _numbersOnlyRegex.IsMatch(pp.Presence.Presence) ||
                pp.Presence.Presence.Equals("P", StringComparison.InvariantCultureIgnoreCase)));

            var rests = personsAndPresence
                .Where(pp => pp.Presence != null && pp.Presence.Called &&
                pp.Presence.Presence != null &&
                pp.Presence.Presence.Equals("R", StringComparison.InvariantCultureIgnoreCase));

            var quarantines = personsAndPresence
                .Where(pp => pp.Presence != null && pp.Presence.Called &&
                pp.Presence.Presence != null &&
                pp.Presence.Presence.IndexOf("Q", StringComparison.InvariantCultureIgnoreCase) >= 0);

            result.PresentCount = presents
                .Count();
            result.RestCount = rests.Count();
            result.QuarantineCount = quarantines.Count();

            result.MissionCount = presents.GroupBy(pp => pp.Person.Mission)
                .ToDictionary(g => g.Key, g =>
                {
                    if (g.Key.IndexOf("Quarantaine", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        return result.QuarantineCount;
                    }
                    return g.Count();
                });

            return result;
        }
    }
}
