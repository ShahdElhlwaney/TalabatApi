using ApiDemo.ResponseModule;
using System.Net;
using System.Text.Json;

namespace ApiDemo.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostEnvironment environment;
        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(RequestDelegate next,IHostEnvironment environment,ILogger<ExceptionMiddleware>logger)
        {
            this.next = next;
            this.environment = environment;
            this.logger = logger;
        }
        public async Task Invoke(HttpContext httpContext) {
            try
            {
              await next(httpContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                httpContext.Response.ContentType = "Application/json";
                httpContext.Response.StatusCode=(int) HttpStatusCode.InternalServerError;
                var response = environment.IsDevelopment() ?
                    new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace) :
                    new ApiException((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json=JsonSerializer.Serialize(response, options);
               await httpContext.Response.WriteAsync(json);
            }
        }
    }
}
