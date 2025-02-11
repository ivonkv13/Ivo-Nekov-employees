namespace Ivo_Nekov_employees.Server.Application.Interfaces
{
    public interface IFileProcessor
    {
        public Task<string> ProcessFileAsync(string filePath);
    }
}
