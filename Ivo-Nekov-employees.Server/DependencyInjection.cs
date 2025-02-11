using FluentValidation;
using Ivo_Nekov_employees.Server.Application.Interfaces;
using Ivo_Nekov_employees.Server.Application.Services;
using Ivo_Nekov_employees.Server.Application.Validators;
using Ivo_Nekov_employees.Server.Domain.Interfaces;
using Ivo_Nekov_employees.Server.Infrastructure.Factories;
using Ivo_Nekov_employees.Server.Infrastructure.FileReaders;
using Ivo_Nekov_employees.Server.Presentation.Filters;

namespace Ivo_Nekov_employees.Server
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddScoped<IFileReader, CsvFileReader>();
            services.AddScoped<IFileReader, JsonFileReader>();
            services.AddScoped<IFileReader, XmlFileReader>();

            services.AddScoped<IEmployeeService, EmployeeService>();

            // Register the factory and processor
            services.AddScoped<FileReaderFactory>();
            services.AddScoped<IFileProcessor, FileProcessor>();

            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IValidator<IFormFile>, FileUploadValidator>();
            services.AddScoped<IValidator<string>>(provider => new FileNameValidator("UploadedFiles"));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
