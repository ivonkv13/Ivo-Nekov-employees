using Ivo_Nekov_employees.Server.Application.Interfaces;
using Ivo_Nekov_employees.Server.Infrastructure.Factories;

namespace Ivo_Nekov_employees.Server.Application.Services
{
    public class FileProcessor : IFileProcessor
    {
        private readonly FileReaderFactory _fileReaderFactory;

        public FileProcessor(FileReaderFactory fileReaderFactory)
        {
            _fileReaderFactory = fileReaderFactory;
        }

        public async Task<string> ProcessFileAsync(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            var reader = _fileReaderFactory.GetFileReader(extension);

            return await reader.ReadFileAsync(filePath);
        }
    }
}
