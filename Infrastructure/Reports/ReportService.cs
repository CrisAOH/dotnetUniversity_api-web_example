using System.Globalization;
using Application.Interfaces;
using CsvHelper;
using Domain;

namespace Infrastructure.Reports
{
    public class ReportService<T> : IReportService<T> where T : BaseEntity
    {
        public Task<MemoryStream> GetCSVReport(List<T> records)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (var textWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(textWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(records);
                textWriter.Flush();
                memoryStream.Seek(0, SeekOrigin.Begin);

                return Task.FromResult(memoryStream);
            }
        }
    }
}