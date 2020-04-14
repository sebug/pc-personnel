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

            Action<string, Action<string>> assignColumn = (columnID, setter) =>
            {
                var cell = FindCellByColumn(r, columnID);
                if (cell != null)
                {
                    string v = this.ExcelFileRepository.GetStringValue(cell, document);
                    setter(v);
                }
            };

            assignColumn("C", v => result.PhoneNumber = v);
            assignColumn("D", v => result.Email = v);
            assignColumn("E", v => result.AVSNumber = v);
            assignColumn("F", v => result.ZipCode = v);
            assignColumn("G", v => result.City = v);
            assignColumn("H", v => result.Canton = v);
            assignColumn("J", v => result.Assignment = v);
            assignColumn("K", v => result.Function = v);
            assignColumn("L", v => result.Rank = v);
            assignColumn("M", v => result.IsEM = v == "EM");
            assignColumn("N", v => result.InternalDomain = v);
            assignColumn("O", v => result.Classification = v);
            assignColumn("P", v => result.Locale = v);
            assignColumn("Q", v => result.HealthNetwork = v);
            assignColumn("R", v => result.SickStatus = v);
            assignColumn("T", v =>
            {
                if (v != null)
                {
                    if (v.Equals("OUI", StringComparison.InvariantCultureIgnoreCase))
                    {
                        result.HasDriversLicense = true;
                    }
                    else if (v.Equals("NON", StringComparison.InvariantCultureIgnoreCase))
                    {
                        result.HasDriversLicense = false;
                    }
                }
            });
            assignColumn("X", v => result.Status = v);
            assignColumn("Y", v => result.Mission = v);
            assignColumn("Z", v => result.MissionResponsible = v);
            assignColumn("AA", v => result.SecondaryMissionResponsible = v);
            assignColumn("AB", v => result.PlaceOfConvocation = v);
            assignColumn("AC", v => result.KitchenInfo = v);


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
