
using FluentValidation;
using Ivo_Nekov_employees.Server.Application.Interfaces;
using Ivo_Nekov_employees.Server.Application.Services;
using Ivo_Nekov_employees.Server.Application.Validators;
using Ivo_Nekov_employees.Server.Domain.Interfaces;
using Ivo_Nekov_employees.Server.Infrastructure.Factories;
using Ivo_Nekov_employees.Server.Infrastructure.FileReaders;
using Ivo_Nekov_employees.Server.Infrastructure.Middleware;
using Ivo_Nekov_employees.Server.Presentation.Filters;

namespace Ivo_Nekov_employees.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddApplicationServices(builder.Configuration);
        //// Add services to the container.

        //// Register all file readers
        //builder.Services.AddScoped<IFileReader, CsvFileReader>();
        //builder.Services.AddScoped<IFileReader, JsonFileReader>();
        //builder.Services.AddScoped<IFileReader, XmlFileReader>();

        //builder.Services.AddScoped<IEmployeeService, EmployeeService>();

        //// Register the factory and processor
        //builder.Services.AddScoped<FileReaderFactory>();
        //builder.Services.AddScoped<IFileProcessor,FileProcessor>();

        //builder.Services.AddControllers(options =>
        //{
        //    options.Filters.Add<ValidationFilter>();
        //});
        //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen();

        //builder.Services.AddScoped<IValidator<IFormFile>, FileUploadValidator>();
        //builder.Services.AddScoped<IValidator<string>>(provider => new FileNameValidator("UploadedFiles"));

        //builder.Services.AddCors(options =>
        //{
        //    options.AddPolicy("AllowAll", policy =>
        //    {
        //        policy.AllowAnyOrigin()
        //              .AllowAnyHeader()
        //              .AllowAnyMethod();
        //    });
        //});

        var app = builder.Build();

        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");    
        app.UseAuthorization();


        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
