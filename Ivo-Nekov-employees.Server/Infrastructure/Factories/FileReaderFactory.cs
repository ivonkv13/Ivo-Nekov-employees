using Ivo_Nekov_employees.Server.Domain.Interfaces;
using Ivo_Nekov_employees.Server.Infrastructure.FileReaders;

namespace Ivo_Nekov_employees.Server.Infrastructure.Factories
{
    public class FileReaderFactory
    {
        private readonly Dictionary<string, IFileReader> _readers;

        public FileReaderFactory(IEnumerable<IFileReader> fileReaders)
        {
            _readers = new Dictionary<string, IFileReader>(StringComparer.OrdinalIgnoreCase);

            foreach (var reader in fileReaders)
            {
                _readers[GetExtension(reader)] = reader;
            }
        }

        /// <summary>
        /// We can further improve and introduce new extensions using the factory
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private string GetExtension(IFileReader reader) => reader switch
        {
            CsvFileReader => ".csv",
            JsonFileReader => ".json",
            XmlFileReader => ".xml",
            _ => throw new NotSupportedException($"Unknown file reader: {reader.GetType().Name}")
        };

        public IFileReader GetFileReader(string fileExtension)
        {
            if (_readers.TryGetValue(fileExtension, out var reader))
            {
                return reader;
            }

            throw new NotSupportedException($"File type {fileExtension} is not supported.");
        }
    }
}
