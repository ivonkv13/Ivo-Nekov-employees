using Ivo_Nekov_employees.Server.Domain.Enitites;
using Ivo_Nekov_employees.Server.Domain.Interfaces;
using System.Formats.Asn1;
using System.Globalization;
using System.Text.Json;

namespace Ivo_Nekov_employees.Server.Infrastructure.FileReaders
{
    public class CsvFileReader : IFileReader
    {
        public async Task<string> ReadFileAsync(string filePath)
        {
            var employees = await ReadEmployeesFromCsvAsync(filePath);
            return JsonSerializer.Serialize(employees, new JsonSerializerOptions { WriteIndented = true });
        }

        public async Task<List<Employee>> ReadEmployeesFromCsvAsync(string filePath)
        {
            var employees = new List<Employee>();

            using (var reader = new StreamReader(filePath))
            {
                bool isHeader = true;

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();

                    if (isHeader)
                    {
                        isHeader = false; // Skip the header row
                        continue;
                    }

                    var values = line?.Split(',');

                    if (values?.Length < 3) continue; // Make sure that the minimum required columns exist

                    var employee = new Employee
                    {
                        EmpId = int.Parse(values[0].Trim()),
                        ProjectId = int.Parse(values[1].Trim()),
                        DateFrom = DateTime.ParseExact(values[2].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        DateTo = (values.Length > 3 && !string.IsNullOrWhiteSpace(values[3]))
                            ? TryParseDate(values[3])
                            : DateTime.Today.Date // Default to today's date
                    };

                    employees.Add(employee);
                }
            }

            return employees;
        }

        private DateTime TryParseDate(string dateString)
        {
            if (DateTime.TryParseExact(dateString.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate.Date;
            }
            return DateTime.Today.Date; // Default to today's date if parsing fails
        }
    }
}
