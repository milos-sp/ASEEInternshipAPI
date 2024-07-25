
using CsvHelper;
using CsvHelper.Configuration;
using ProductAPI.Controllers;
using System.Globalization;

namespace ProductAPI.Services
{
    public class CSVService : ICSVService
    {
        private readonly ILogger<CSVService> _logger;

        public CSVService(ILogger<CSVService> logger)
        {
            _logger = logger;
        }
        public IEnumerable<T> ReadCSV<T>(Stream file)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Replace("-", "").ToLowerInvariant(), // trebalo bi da oba prolaze kroz ovu funkciju
                MissingFieldFound = args =>
                {
                    _logger.LogInformation($"Field with names ['{string.Join("', '", args.HeaderNames)}'] at index '{args.Index}' was not found. ");
                }
                /*ReadingExceptionOccurred = args =>
                {
                    return false; // mozda probati sa ovim
                }*/
            }; // bilo je bitno odraditi replace
            var reader = new StreamReader(file);
            var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<T>();
            return records;
        }
    }
}
