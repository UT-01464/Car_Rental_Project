//using Carrental.Services;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Configuration;
//using Carrental.IRepositries;
//using Carrental.IsServices;
//using Carrental.Repositoies;
//using Carrental.Helpers;
//using System.IO;
//using Microsoft.AspNetCore.Cors.Infrastructure;

//namespace Carrental
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            builder.Services.AddControllers();

//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            // Read the connection string from configuration
//            var connectionString = builder.Configuration.GetConnectionString("CarConnection");

//            // Set the image folder path using the WebRootPath
//            var imageFolder = Path.Combine(builder.Environment.WebRootPath, "carimages");

//            // Register your repositories and services using raw SQL access
//            builder.Services.AddScoped<ICustomerRepository>(provider => new CustomerRepository(connectionString));
//            builder.Services.AddScoped<ICustomerService, CustomerService>();

//            // Register ImageUploadHelper
//            builder.Services.AddScoped<ImageUploadHelper>(provider => new ImageUploadHelper(imageFolder));

//            // Register ICarRepository and ICarService, passing the ImageUploadHelper to the CarRepository
//            builder.Services.AddScoped<Icarrepository>(provider =>
//            {
//                var imageUploadHelper = provider.GetRequiredService<ImageUploadHelper>();
//                return new Carrepository(connectionString, imageUploadHelper);
//            });
//            builder.Services.AddScoped<Icarservice, Carservice>();

//            builder.Services.AddScoped<IManagerRepository>(provider => new ManagerRepository(connectionString));
//            builder.Services.AddScoped<IManagerService, ManagerService>();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();
//            app.UseAuthorization();
//            app.MapControllers();
//            app.Run();
//        }
//    }
//}

using Carrental.Helpers;
using Carrental.IRepositories; // Note the corrected namespace
using Carrental.IServices; // Note the corrected namespace
using Carrental.Repositories; // Note the corrected namespace
using Carrental.Services; // Note the corrected namespace
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using Carrental.IRepositries;
using Carrental.IsServices;
using Carrental.Repositoies;

namespace Carrental
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Read the connection string from configuration
            var connectionString = builder.Configuration.GetConnectionString("CarConnection");

            // Set the image folder path using the WebRootPath
            var imageFolder = Path.Combine(builder.Environment.WebRootPath, "carimages");

            // Register ImageUploadHelper
            builder.Services.AddScoped<ImageUploadHelper>(provider => new ImageUploadHelper(imageFolder));

            // Register ICarRepository and ICarService, passing the ImageUploadHelper to the CarRepository
            builder.Services.AddScoped<ICarRepository>(provider =>
            {
                var imageUploadHelper = provider.GetRequiredService<ImageUploadHelper>();
                return new CarRepository(connectionString, imageUploadHelper);
            });
            builder.Services.AddScoped<ICarService, CarService>();

            // Register other repositories and services
            builder.Services.AddScoped<ICustomerRepository>(provider => new CustomerRepository(connectionString));
            builder.Services.AddScoped<ICustomerService, CustomerService>();

            builder.Services.AddScoped<IManagerRepository>(provider => new ManagerRepository(connectionString));
            builder.Services.AddScoped<IManagerService, ManagerService>();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting(); 
            app.UseStaticFiles();

            app.UseCors();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
