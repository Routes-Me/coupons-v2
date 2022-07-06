using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace CouponService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public readonly IWebHostEnvironment HostingEnv;


        public HomeController(IWebHostEnvironment hostingEnv)
        {
            HostingEnv = hostingEnv;
        }
        [HttpGet]
        public string Get()
        {
            return "Coupon service started successfully. Environment - " + HostingEnv.EnvironmentName + "";
        }
    }
}
