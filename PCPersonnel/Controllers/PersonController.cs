using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PCPersonnel.Models;
using PCPersonnel.Repositories;

namespace PCPersonnel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonRepository _personRepository;

        public PersonController(ILogger<PersonController> logger,
            IPersonRepository personRepository)
        {
            _logger = logger;
            this._personRepository = personRepository;
        }

        [HttpGet]
        public IEnumerable<Person> Get()
        {
            var people = this._personRepository.GetAll();

            return people;
        }

        [HttpGet]
        [Route("{id}")]
        public Person Get(string id)
        {
            return this._personRepository.GetByAVSNumber(id);
        }
    }
}
