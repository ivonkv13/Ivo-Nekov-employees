using Ivo_Nekov_employees.Server.Domain.Enitites;

namespace Ivo_Nekov_employees.Server.Domain.Interfaces
{
    public interface IFileReader
    {
        Task<string> ReadFileAsync(string filePath);
    }
}
    