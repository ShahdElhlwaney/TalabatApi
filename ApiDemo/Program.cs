using ApiDemo.Extensions;
using ApiDemo.Middlewares;
using Core.Entities.Identity;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ApiDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.ApplicationServices();
            builder.Services.IdentityServices(builder.Configuration);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDbContext<StoreDbContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                }
                );

            builder.Services.AddDbContext<AppIdentityDbContext>(
            options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            }
            );
            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });


            
            builder.Services.AddSwaggerDocServices();
            builder.Services.AddCors(c => c.AddPolicy("CorsPolicy", c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin=>true)));
            var app = builder.Build();

            using (var scope=app.Services.CreateScope())
            {
                      
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<StoreDbContext>();
                    await context.Database.MigrateAsync();
                    await StoreDbContextSeed.Seed(context,loggerFactory);
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    await identityContext.Database.MigrateAsync();
                    await AppIdentityDbContextSeed.SeedUserAsync(userManager);

                }
                catch (Exception ex) {
                  var logger=  loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex.Message);
                }
             
            }
                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
            app.MapSwagger().RequireAuthorization();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
             
//
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.MapControllers();

            app.Run();
        }
    }
}