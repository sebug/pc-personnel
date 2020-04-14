using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using PCPersonnel.Models;
using System.Linq;

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

        private List<Person> ReadPeopleFromSpreadsheet(SpreadsheetDocument document)
        {
            var sheets = document.WorkbookPart.Workbook.Descendants<Sheet>();
            if (!sheets.Any())
            {
                throw new Exception("No worksheet found");
            }
            var firstSheet = sheets.First();

            WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(firstSheet.Id);
            var worksheet = worksheetPart.Worksheet;
            var peopleRows = worksheet.Descendants<Row>().Skip(25);

            var result = peopleRows.Select(r => this.ReadPerson(r, document))
                .Where(p => p != null && !p.IsEmpty)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToList();

            return result;
        }

        private Person ReadPerson(Row r, SpreadsheetDocument document)
        {
            var firstCell = r.Descendants<Cell>().FirstOrDefault();
            if (firstCell == null)
            {
                return null;
            }
            var secondCell = r.Descendants<Cell>().Skip(1).FirstOrDefault();
            if (secondCell == null)
            {
                return null;
            }
            var result = new Person();

            result.LastName = this.ExcelFileRepository.GetStringValue(firstCell, document);
            result.FirstName = this.ExcelFileRepository.GetStringValue(secondCell, document);

            return result;
        }
    }
}
