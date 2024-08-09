using ApiDemo.ResponseModule;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Controllers
{
    [ApiController]
    public class BuggyController : BaseController
    {
        private readonly StoreDbContext context;

        public BuggyController(StoreDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public ActionResult GetNotFoundRequset()
        {
            var p = context.Products.Find(1000);
            if(p == null)
            {
                return NotFound(new ApiResponse(200));
            }
            return Ok();
        }
    }
}
