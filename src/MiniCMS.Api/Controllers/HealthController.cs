using Microsoft.AspNetCore.Mvc;

namespace MiniCMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<HealthResponse> GetHealth()
        {
            return Ok(new HealthResponse
            {
                Status = "Healthy",
                Version = "1.0.0"
            });
        }

        [HttpGet("ready")]
        public ActionResult GetReadiness()
        {
            // Add database connectivity check here
            return Ok(new { status = "Ready" });
        }

        [HttpGet("live")]
        public ActionResult GetLiveness()
        {
            return Ok(new { status = "Alive" });
        }
    }

    public class HealthResponse
    {
        public string Status { get; set; }
        public string Version { get; set; }
    }
}
