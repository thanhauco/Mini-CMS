using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniCMS.Domain.Entities;
using MiniCMS.Infrastructure.Repositories;

namespace MiniCMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppsController : ControllerBase
    {
        private readonly AppRepository _appRepository;

        public AppsController(AppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        /// <summary>
        /// Get all apps
        /// </summary>
        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<App>>> GetApps()
        {
            var apps = await _appRepository.GetAllAsync();
            return Ok(apps);
        }

        /// <summary>
        /// Get app by name
        /// </summary>
        [HttpGet("{name}")]
        public async Task<ActionResult<App>> GetApp(string name)
        {
            var app = await _appRepository.GetByNameAsync(name);
            if (app == null)
            {
                return NotFound(new { message = $"App '{name}' not found" });
            }
            return Ok(app);
        }

        /// <summary>
        /// Create a new app
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<App>> CreateApp([FromBody] CreateAppRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new { message = "App name is required" });
            }

            var existing = await _appRepository.GetByNameAsync(request.Name);
            if (existing != null)
            {
                return Conflict(new { message = $"App '{request.Name}' already exists" });
            }

            var app = new App(request.Name, request.DisplayName);
            await _appRepository.AddAsync(app);

            return CreatedAtAction(nameof(GetApp), new { name = app.Name }, app);
        }

        /// <summary>
        /// Update an app
        /// </summary>
        [HttpPut("{name}")]
        public async Task<ActionResult<App>> UpdateApp(string name, [FromBody] UpdateAppRequest request)
        {
            var app = await _appRepository.GetByNameAsync(name);
            if (app == null)
            {
                return NotFound(new { message = $"App '{name}' not found" });
            }

            if (!string.IsNullOrWhiteSpace(request.DisplayName))
            {
                app.UpdateDisplayName(request.DisplayName);
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                app.Description = request.Description;
            }

            await _appRepository.UpdateAsync(app);
            return Ok(app);
        }

        /// <summary>
        /// Delete an app
        /// </summary>
        [HttpDelete("{name}")]
        public async Task<ActionResult> DeleteApp(string name)
        {
            var app = await _appRepository.GetByNameAsync(name);
            if (app == null)
            {
                return NotFound(new { message = $"App '{name}' not found" });
            }

            await _appRepository.DeleteAsync(app.Id);
            return NoContent();
        }
    }

    public class CreateAppRequest
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class UpdateAppRequest
    {
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
