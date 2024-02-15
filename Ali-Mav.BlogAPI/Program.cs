
using Ali_Mav.BlogAPI.Data;
using Ali_Mav.BlogAPI.Data.Interfaces;
using Ali_Mav.BlogAPI.Data.Repositories;
using Ali_Mav.BlogAPI.Service.Implementation;
using Ali_Mav.BlogAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ali_Mav.BlogAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IPostService , PostService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPostRepository,PostRepository>();
            builder.Services.AddScoped<ISeedService,SeedService>();


            var connection = builder.Configuration.GetConnectionString("DbContext");
            builder.Services.AddDbContext<AppDbContext>(options => options
            .UseNpgsql(connection));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

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
