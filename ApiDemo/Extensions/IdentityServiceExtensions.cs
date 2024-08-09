using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiDemo.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection IdentityServices(this IServiceCollection services,IConfiguration configuration) 
        {
           services.AddIdentityCore<AppUser>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddSignInManager<SignInManager<AppUser>>()
                .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer
                (
               options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey=true,
                      IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:key"])),
                      ValidIssuer= configuration["Token:Issuer"],
                      ValidateIssuer=true,
                      ValidateAudience=false
                   };
               }
               );
            return services;    
        }
    }
}
