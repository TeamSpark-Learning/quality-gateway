using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Marko()
        {
            return StatusCode((int) HttpStatusCode.OK, "Polo!");
        }

        [HttpGet]
        public IActionResult Internal()
        {
            return StatusCode((int) HttpStatusCode.OK, new 
            {
                IsHealthy = true
            });
        }
        
        [HttpGet]
        public IActionResult External()
        {
            return StatusCode((int) HttpStatusCode.OK, new 
            {
                IsHealthy = true
            });
        }
    }
}