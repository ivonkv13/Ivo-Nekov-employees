using Ivo_Nekov_employees.Server.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace Ivo_Nekov_employees.Server.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileProcessor _fileProcessor;
        private readonly IValidator<IFormFile> _fileValidator;
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

        public FileController(IFileProcessor fileProcessor, IValidator<IFormFile> fileValidator)
        {
            _fileProcessor = fileProcessor;
            _fileValidator = fileValidator;
            if (!Directory.Exists(_uploadFolder)) Directory.CreateDirectory(_uploadFolder);
        }

        [HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null)
                return BadRequest(new { Error = "No file uploaded." });

            var validationResult = await _fileValidator.ValidateAsync(file);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Error = "File validation failed",
                    Details = validationResult.Errors.Select(e => e.ErrorMessage)
                });
            }

            var filePath = Path.Combine(_uploadFolder, file.FileName);

            Directory.CreateDirectory(_uploadFolder);

            await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await file.CopyToAsync(stream);

            var content = await _fileProcessor.ProcessFileAsync(filePath);

            return Ok(new { Message = "File uploaded successfully", FileName = file.FileName, Content = content });
        }

        [HttpGet]
        [Route("GetAllFilesInFolder")]
        public IActionResult GetAllFilesInFolder()
        {
            var allFiles = Directory.GetFiles(_uploadFolder)
                .Select(Path.GetFileName)
                .ToArray();

            return Ok(new { Files = allFiles });
        }
    }
}
                                       