using Microsoft.OpenApi.Models;

namespace ApiDemo.Extensions
{
    public static class SwaggerDocExtensions
    {
        public static IServiceCollection AddSwaggerDocServices(this IServiceCollection services)
        {
            
            services.AddSwaggerGen(c =>
                {
                    
                    c.SwaggerDoc("v1", new OpenApiInfo
                    { Title = "Api Demo", Version = "v1" });
                    var securitySchema = new OpenApiSecurityScheme()
                    {
                        Type = SecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Description = "JWT Auth Bearer Scheme",
                        Scheme = "Bearer",
                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }

                    };
                    c.AddSecurityDefinition("Bearer",securitySchema);
                    var securityRequirement = new OpenApiSecurityRequirement()
                    {
                        {securitySchema,new string[]{} }
                    };
                }
                );
            
            services.AddAuthorization();
            return services;

        }

    }
}
