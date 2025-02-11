using Ivo_Nekov_employees.Server.Domain.Enitites;
using Ivo_Nekov_employees.Server.Domain.Interfaces;

namespace Ivo_Nekov_employees.Server.Infrastructure.FileReaders
{
    public class JsonFileReader : IFileReader
    {
        public async Task<string> ReadFileAsync(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
