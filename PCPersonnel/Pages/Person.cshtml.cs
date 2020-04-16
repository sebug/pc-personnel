using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PCPersonnel.Models;
using PCPersonnel.Repositories;

namespace PCPersonnel.Pages
{
    public class PersonModel : PageModel
    {
        private readonly IPersonRepository _personRepository;

        public PersonModel(IPersonRepository personRepository)
        {
            this._personRepository = personRepository;
        }

        public Person Person { get; set; }

        public void OnGet(string avsNumber)
        {
            this.Person = this._personRepository.GetByAVSNumber(avsNumber);
            if (this.Person != null)
            {
                this.ViewData["title"] = this.Person.FirstName + " " +
                    this.Person.LastName;
            }
        }
    }
}
