using System;
using System.Collections.Generic;

namespace PCPersonnel.Models
{
    public class Person
    {
        public Person()
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AVSNumber { get; set; }

        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Canton { get; set; }

        public string Assignment { get; set; }
        public string Function { get; set; }
        public string Rank { get; set; }

        // Well this one is completely custom
        public bool IsEM { get; set; }

        public string InternalDomain { get; set; }
        public string Classification { get; set; }

        public string Locale { get; set; }
        // Whatever that means
        public string HealthNetwork { get; set; }

        public string SickStatus { get; set; }

        public bool? HasDriversLicense { get; set; }

        public string Status { get; set; }

        public string Mission { get; set; }
        public string PlaceOfConvocation { get; set; }
        public string MissionResponsible { get; set; }
        public string SecondaryMissionResponsible { get; set; }

        public string KitchenInfo { get; set; }

        public List<PresenceEntry> Presences { get; set; }

        public bool IsEmpty
        {
            get
            {
                return String.IsNullOrWhiteSpace(this.FirstName) &&
                    String.IsNullOrWhiteSpace(this.LastName);
            }
        }
    }
}
