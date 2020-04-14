﻿using System;
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
