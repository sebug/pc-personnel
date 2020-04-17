using System;
using System.Collections.Generic;
using PCPersonnel.Models;
using PCPersonnel.Repositories;
using System.Linq;

namespace PCPersonnel.Services
{
    public class RollCallService : IRollCallService
    {
        private readonly IPersonRepository _personRepository;
        public RollCallService(IPersonRepository personRepository)
        {
            this._personRepository = personRepository;
        }

        public RollCall GetByEntryAndDate(string entry, DateTime date)
        {
            var result = new RollCall();
            result.Entry = entry;
            result.Date = date;
            result.Presences = new List<RollCallPerson>();

            var people = this._personRepository.GetAll();
            if (people == null)
            {
                throw new Exception("Could not fetch people for roll call");
            }

            var rollCallPeople =
                people.Where(p => p.Presences != null).Select(p =>
                new
                {
                    Person = p,
                    PresenceEntry = p.Presences.FirstOrDefault(presence =>
                presence.Called && presence.Date == date &&
                p.PlaceOfConvocation != null &&
                p.PlaceOfConvocation.IndexOf(entry, StringComparison.InvariantCultureIgnoreCase) >= 0)
                }).Where(o => o.PresenceEntry != null)
                .Select(o => new RollCallPerson
                {
                    FirstName = o.Person.FirstName,
                    LastName = o.Person.LastName,
                    Presence = o.PresenceEntry.Presence
                });

            var withoutRest = rollCallPeople.Where(rp =>
            rp.Presence == null || rp.Presence.IndexOf("R", StringComparison.InvariantCultureIgnoreCase) < 0);

            result.Presences = withoutRest.ToList();

            return result;
        }

        public RollCallOptions GetRollCallOptions(DateTime date)
        {
            var result = new RollCallOptions();
            result.Date = date;

            var people = this._personRepository.GetAll();
            if (people == null)
            {
                throw new Exception("Could not fetch people for roll call");
            }

            var rollCallPeople =
                people.Where(p => p.Presences != null).Select(p =>
                new
                {
                    Person = p,
                    PresenceEntry = p.Presences.FirstOrDefault(presence =>
                presence.Called && presence.Date == date &&
                p.PlaceOfConvocation != null)
                })
                .Where(o => o.PresenceEntry != null)
                .ToList();
            var options = rollCallPeople.GroupBy(o => o.Person.PlaceOfConvocation)
                .Select(g => new RollCallOption
                {
                    PlaceOfEntry = g.Key
                }).ToList();

            foreach (var opt in options)
            {
                opt.RollCallLink = "/RollCall?date=" +
                    result.Date.ToString("yyyy-MM-dd") +
                    "&placeOfEntry=" + opt.PlaceOfEntry;
            }

            result.Options = options;

            return result;
        }
    }
}
