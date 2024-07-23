
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ProductAPI.Services
{
    public class CSVService : ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Replace("-", "").ToLowerInvariant() // trebalo bi da oba prolaze kroz ovu funkciju
            }; // bilo je bitno odraditi replace
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<T>();
            return records;
        }
    }
}
