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

            builder.WebHost.UseWebRoot("wwwroot");
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionstring = builder.Configuration.GetConnectionString("CarConnection");

            //customer
            builder.Services.AddScoped<ICustomerRepository>(provider=>new CustomerRepository(connectionstring));
            builder.Services.AddScoped<ICustomerService, CustomerService>();

            //car
            builder.Services.AddScoped<Icarrepository>(Provider => new Carrepository(connectionstring));
            builder.Services.AddScoped<Icarservice, Carservice>();

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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();
            app.UseCors();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
