using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public string Marko()
        {
            return "Polo!";
        }

        [HttpGet]
        public dynamic Internal()
        {
            return new
            {
                IsHealthy = true
            };
        }
        
        [HttpGet]
        public IActionResult External()
        {
            return StatusCode((int) HttpStatusCode.InternalServerError, new 
            {
                IsHealthy = false
            });
        }
    }
}