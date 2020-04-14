using System;
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
