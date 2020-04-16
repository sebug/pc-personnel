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
    public class PeopleModel : PageModel
    {
        private readonly IPersonRepository _personRepository;

        public PeopleModel(IPersonRepository personRepository)
        {
            this._personRepository = personRepository;
        }

        public List<Person> People { get; set; }

        public void OnGet()
        {
            this.People = this._personRepository.GetAll()
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToList();
        }
    }
}
