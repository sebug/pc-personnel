using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
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
            return this.ExcelFileRepository.ReadExcelFile(this.ReadPeopleFromSpreadsheet);
        }

        private List<Person> ReadPeopleFromSpreadsheet(SpreadsheetDocument spreadsheetDocument)
        {
            return new List<Person>();
        }
    }
}
