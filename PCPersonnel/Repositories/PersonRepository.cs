using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using PCPersonnel.Models;
using System.Linq;
using System.Text.RegularExpressions;

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
            var firstNameCell = FindCellByColumn(r, "A");
            if (firstNameCell == null)
            {
                return null;
            }
            var lastNameCell = FindCellByColumn(r, "B");
            if (lastNameCell == null)
            {
                return null;
            }
            var result = new Person();

            result.LastName = this.ExcelFileRepository.GetStringValue(firstNameCell, document);
            result.FirstName = this.ExcelFileRepository.GetStringValue(lastNameCell, document);

            var phoneNumberCell = FindCellByColumn(r, "C");
            if (phoneNumberCell != null)
            {
                result.PhoneNumber = this.ExcelFileRepository.GetStringValue(phoneNumberCell, document);
            }

            var emailCell = FindCellByColumn(r, "D");
            if (emailCell != null)
            {
                result.Email = this.ExcelFileRepository.GetStringValue(emailCell, document);
            }

            var avsCell = FindCellByColumn(r, "E");
            if (avsCell != null)
            {
                result.AVSNumber = this.ExcelFileRepository.GetStringValue(avsCell, document);
            }

            var zipCodeCell = FindCellByColumn(r, "F");
            if (zipCodeCell != null)
            {
                result.ZipCode = this.ExcelFileRepository.GetStringValue(zipCodeCell, document);
            }

            var cityCell = FindCellByColumn(r, "G");
            if (cityCell != null)
            {
                result.City = this.ExcelFileRepository.GetStringValue(cityCell, document);
            }

            var cantonCell = FindCellByColumn(r, "H");
            if (cantonCell != null)
            {
                result.Canton = this.ExcelFileRepository.GetStringValue(cantonCell, document);
            }

            return result;
        }

        private Regex _cellReferenceRegex = new Regex("(?<column>[A-Z]+)(?<row>\\d+)", RegexOptions.Compiled);

        private Cell FindCellByColumn(Row r, string column)
        {
            var cells = r.Descendants<Cell>();
            return cells.FirstOrDefault(c =>
            {
                var m = this._cellReferenceRegex.Match(c.CellReference.Value);
                if (!m.Success)
                {
                    return false;
                }
                string col = m.Groups["column"].Value;
                return col == column;
            });
        }
    }
}
