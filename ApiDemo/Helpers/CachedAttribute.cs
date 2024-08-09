using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiDemo.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLiveInSeconds;

        public CachedAttribute(int timeToLiveInSeconds)
        {
            this.timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService= context.HttpContext.RequestServices
                .GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateKeyFromRequest(context.HttpContext.Request);
            var responseResult=await cacheService.GetCachedResponse(cacheKey);
            if (!string.IsNullOrEmpty(responseResult)) {
                var contentResult = new ContentResult()
                {
                    Content = responseResult,
                    ContentType="application/json"
                };
                return;
            }
            var executedContext = await next();
            if (executedContext.Result is OkObjectResult okObjectResult)
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult,TimeSpan.FromSeconds(timeToLiveInSeconds));
           
        }
        private string GenerateKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key,value) in request.Query.OrderBy(k=>k.Key))
                keyBuilder.Append($"|{key}-{value}");
            return keyBuilder.ToString();
        }
    }
}
