using FluentValidation;

namespace Ivo_Nekov_employees.Server.Application.Validators
{
    public class FileUploadValidator : AbstractValidator<IFormFile>
    {
        private static readonly string[] AllowedExtensions = { ".csv" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        public FileUploadValidator()
        {
            RuleFor(file => file)
                .NotNull().WithMessage("No file uploaded.")
                .Must(file => file.Length > 0).WithMessage("Uploaded file is empty.");

            RuleFor(file => Path.GetExtension(file.FileName))
                .NotEmpty().WithMessage("Invalid file format.")
                .Must(ext => Array.Exists(AllowedExtensions, e => e.Equals(ext, StringComparison.OrdinalIgnoreCase)))
                .WithMessage($"Only {string.Join(", ", AllowedExtensions)} files are allowed.");

            RuleFor(file => file.Length)
                .LessThanOrEqualTo(MaxFileSize).WithMessage("File size must be less than 5MB.");
        }
    }
}
