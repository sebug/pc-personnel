using System;
using System.Collections.Generic;
using PCPersonnel.Models;

namespace PCPersonnel.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private IExcelFileRepository ExcelFileRepository { get; }

        public PersonRepository(IExcelFileRepository excelFileRepository)
        {
            this.ExcelFileRepository = excelFileRepository;
        }

        public PersonRepository()
        {
        }

        public List<Person> GetAll()
        {
            return new List<Person>();
        }
    }
}
