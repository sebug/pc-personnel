using System;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;

namespace PCPersonnel.Repositories
{
    public class InMemoryExcelFileRepository : IExcelFileRepository
    {
        private byte[] FileContent { get; set; }
        private List<Action<byte[]>> ExcelFilesUpdatedCallbacks { get; set; } = new List<Action<byte[]>>();

        public InMemoryExcelFileRepository()
        {
        }

        public async Task StoreExcelFile(IFormFile excelFile)
        {
            if (excelFile == null)
            {
                return;
            }
            using (var ms = new MemoryStream())
            {
                await excelFile.CopyToAsync(ms);
                this.FileContent = ms.ToArray();
                if (this.ExcelFilesUpdatedCallbacks != null)
                {
                    foreach (var cb in this.ExcelFilesUpdatedCallbacks)
                    {
                        cb(this.FileContent);
                    }
                }
            }
        }

        public T ReadExcelFile<T>(Func<SpreadsheetDocument, T> reader)
        {
            if (this.FileContent == null)
            {
                throw new Exception("Master file has not been uploaded yet.");
            }
            using (var ms = new MemoryStream(this.FileContent))
            {
                using (var spreadsheet = SpreadsheetDocument.Open(ms, false))
                {
                    return reader(spreadsheet);
                }
            }
        }

        public string GetStringValue(Cell c, SpreadsheetDocument document)
        {
            // If the content of the first cell is stored as a shared string, get the text of the first cell
            // from the SharedStringTablePart and return it. Otherwise, return the string value of the cell.
            if (c.DataType != null && c.DataType.Value ==
                CellValues.SharedString)
            {
                SharedStringTablePart shareStringPart = document.WorkbookPart.
            GetPartsOfType<SharedStringTablePart>().First();
                SharedStringItem[] items = shareStringPart.
            SharedStringTable.Elements<SharedStringItem>().ToArray();
                return items[int.Parse(c.CellValue.Text)].InnerText;
            }
            else
            {
                if (c.CellValue != null)
                {
                    return c.CellValue.Text;
                }
                else
                {
                    return c.InnerText;
                }
            }
        }

        public DateTime? GetDateValue(Cell c, SpreadsheetDocument document)
        {
            string s = this.GetStringValue(c, document);
            int i;
            if (s == null || !int.TryParse(s, out i))
            {
                return null;
            }
            return DateTime.FromOADate(i);
        }

        public void RegisterExcelFileUploadedCallback(Action<byte[]> newFileUploadedCallback)
        {
            this.ExcelFilesUpdatedCallbacks.Add(newFileUploadedCallback);
        }
    }
}
