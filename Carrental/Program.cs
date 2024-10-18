using Carrental.IRepositries;
using Carrental.IsServices;
using Carrental.Repositoies;
using Carrental.Services;
using Microsoft.EntityFrameworkCore;

namespace Carrental
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionstring = builder.Configuration.GetConnectionString("CarConnection");
            builder.Services.AddScoped<ICustomerRepository>(provider=>new CustomerRepository(connectionstring));
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IManagerRepository>(provider => new ManagerRepository(connectionstring));
            builder.Services.AddScoped<IManagerService, ManagerService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
