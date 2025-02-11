using FluentValidation;
using Ivo_Nekov_employees.Server.Application.Interfaces;
using Ivo_Nekov_employees.Server.Application.Services;
using Ivo_Nekov_employees.Server.Domain.Enitites;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Ivo_Nekov_employees.Server.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
        private readonly IEmployeeService _employeeService;
        private readonly IFileProcessor _fileProcessor;

        public EmployeeController(IEmployeeService employeeService, IFileProcessor fileProcessor)
        {
            _employeeService = employeeService;
            _fileProcessor = fileProcessor;
        }

        [HttpGet]
        [Route("GetEmployeePairs")]
        public async Task<IActionResult> GetEmployeePairs([FromQuery] string fileName, [FromServices] IValidator<string> fileNameValidator)
        {
            var validationResult = await fileNameValidator.ValidateAsync(fileName);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Error = "File validation failed",
                    Details = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }


            var fullPath = Path.Combine(_uploadFolder, fileName);
            var content = await _fileProcessor.ProcessFileAsync(fullPath);

            if (string.IsNullOrEmpty(content))
                throw new InvalidOperationException("File processing failed. No content found.");

            var employees = JsonSerializer.Deserialize<List<Employee>>(content)
                ?? throw new JsonException("Failed to deserialize employee data.");

            var employeePairs = _employeeService.FindAllWorkingPairs(employees);
            return Ok(employeePairs);
        }
    }
}
