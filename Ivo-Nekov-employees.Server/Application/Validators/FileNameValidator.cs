using FluentValidation;

namespace Ivo_Nekov_employees.Server.Application.Validators
{
    public class FileNameValidator : AbstractValidator<string>
    {
        private readonly string _uploadFolder;
        private static readonly string[] AllowedExtensions = { ".csv" };

        public FileNameValidator(string uploadFolder)
        {
            if (string.IsNullOrEmpty(uploadFolder))
                throw new ArgumentNullException(nameof(uploadFolder));

            _uploadFolder = uploadFolder;

            RuleFor(fileName => fileName)
                .NotEmpty().WithMessage("Filename cannot be empty.")
                .Must(HaveValidExtension).WithMessage($"Invalid file format. Allowed: {string.Join(", ", AllowedExtensions)}")
                .Must(FileExists).WithMessage("File does not exist in the upload folder.");
        }

        private bool HaveValidExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return Array.Exists(AllowedExtensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        private bool FileExists(string fileName)
        {
            var filePath = Path.Combine(_uploadFolder, fileName);
            return File.Exists(filePath);
        }
    }
}
