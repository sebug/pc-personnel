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
        private List<Person> PersonCache { get; set; }

        public PersonRepository(IExcelFileRepository excelFileRepository)
        {
            this.ExcelFileRepository = excelFileRepository;
            this.ExcelFileRepository.RegisterExcelFileUploadedCallback(bytes =>
            {
                this.PersonCache = null;
            });
        }

        public List<Person> GetAll()
        {
            return this.ExcelFileRepository.ReadExcelFile(this.ReadPeopleFromSpreadsheet);
        }

        private List<Person> ReadPeopleFromSpreadsheet(SpreadsheetDocument document)
        {
            if (this.PersonCache != null)
            {
                return this.PersonCache;
            }
            var sheets = document.WorkbookPart.Workbook.Descendants<Sheet>();
            if (!sheets.Any())
            {
                throw new Exception("No worksheet found");
            }
            var firstSheet = sheets.First();

            WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(firstSheet.Id);
            var worksheet = worksheetPart.Worksheet;

            var dateHeadingsRow = worksheet.Descendants<Row>().Skip(23).First();

            var dateHeadingCells = dateHeadingsRow.Descendants<Cell>().ToList();

            Dictionary<string, DateTime> columnToDate = new Dictionary<string, DateTime>();

            foreach (var cell in dateHeadingCells)
            {
                DateTime? dh = this.ExcelFileRepository.GetDateValue(cell, document);
                if (dh.HasValue)
                {
                    var m = this._cellReferenceRegex.Match(cell.CellReference);
                    if (m.Success)
                    {
                        string column = m.Groups["column"].Value;
                        columnToDate[column] = dh.Value;
                    }
                }
            }

            var peopleRows = worksheet.Descendants<Row>().Skip(25);

            var result = peopleRows.Select(r => this.ReadPerson(r, document, columnToDate))
                .Where(p => p != null && !p.IsEmpty)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToList();

            this.PersonCache = result;

            return result;
        }

        private Person ReadPerson(Row r, SpreadsheetDocument document, IReadOnlyDictionary<string, DateTime> dateColumns)
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

            var dateKvps = dateColumns.OrderBy(kvp => kvp.Value).ToList();
            result.Presences = new List<PresenceEntry>();
            foreach (var dateKvp in dateKvps)
            {
                var calledCell = FindCellByColumn(r, dateKvp.Key);
                var presenceEntry = new PresenceEntry()
                {
                    Date = dateKvp.Value
                };
                if (calledCell != null)
                {
                    string calledString = this.ExcelFileRepository.GetStringValue(calledCell, document);
                    presenceEntry.Called = calledString != null && calledString.Equals("x", StringComparison.InvariantCultureIgnoreCase);

                    var presentCell = FindCellByColumn(r, NextColumn(dateKvp.Key));
                    if (presentCell != null)
                    {
                        presenceEntry.Presence = this.ExcelFileRepository.GetStringValue(presentCell, document);
                    }
                }
                result.Presences.Add(presenceEntry);
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

        private string NextColumn(string column)
        {
            return this.ExcelColumnFromNumber(this.NumberFromExcelColumn(column) + 1);
        }

        private int NumberFromExcelColumn(string column)
        {
            int retVal = 0;
            string col = column.ToUpper();
            for (int iChar = col.Length - 1; iChar >= 0; iChar--)
            {
                char colPiece = col[iChar];
                int colNum = colPiece - 64;
                retVal = retVal + colNum * (int)Math.Pow(26, col.Length - (iChar + 1));
            }
            return retVal;
        }

        private string ExcelColumnFromNumber(int column)
        {
            string columnString = "";
            decimal columnNumber = column;
            while (columnNumber > 0)
            {
                decimal currentLetterNumber = (columnNumber - 1) % 26;
                char currentLetter = (char)(currentLetterNumber + 65);
                columnString = currentLetter + columnString;
                columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
            }
            return columnString;
        }
    }
}
