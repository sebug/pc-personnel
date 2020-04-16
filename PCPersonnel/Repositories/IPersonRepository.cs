using System;
using System.Collections.Generic;
using PCPersonnel.Models;

namespace PCPersonnel.Repositories
{
    public interface IPersonRepository
    {
        List<Person> GetAll();

        Person GetByAVSNumber(string avsNumber);
    }
}
