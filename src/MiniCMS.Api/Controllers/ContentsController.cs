using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniCMS.Domain.Entities;
using MiniCMS.Infrastructure.Repositories;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.SignalR;
using MiniCMS.Api.Hubs;
using MiniCMS.Application.Services;

namespace MiniCMS.Api.Controllers
{
    [ApiController]
    [Route("api/apps/{appName}/{schemaName}")]
    public class ContentsController : ControllerBase
    {
        private readonly ContentRepository _contentRepository;
        private readonly AppRepository _appRepository;
        private readonly SchemaRepository _schemaRepository;
        private readonly IHubContext<ContentHub> _hubContext;
        private readonly IContentValidator _contentValidator;

        public ContentsController(
            ContentRepository contentRepository, 
            AppRepository appRepository,
            SchemaRepository schemaRepository,
            IHubContext<ContentHub> hubContext,
            IContentValidator contentValidator)
        {
            _contentRepository = contentRepository;
            _appRepository = appRepository;
            _schemaRepository = schemaRepository;
            _hubContext = hubContext;
            _contentValidator = contentValidator;
        }

        /// <summary>
        /// Get all content items for a schema
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Content>>> GetContents(
            string appName, 
            string schemaName,
            [FromQuery] bool publishedOnly = false)
        {
            // For now, we'll return a simplified response
            // Full implementation would resolve app and schema IDs
            var contents = await _contentRepository.GetAllAsync();
            return Ok(contents);
        }

        /// <summary>
        /// Get a specific content item
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Content>> GetContent(string appName, string schemaName, Guid id)
        {
            var content = await _contentRepository.GetByIdAsync(id);
            if (content == null)
            {
                return NotFound(new { message = "Content not found" });
            }
            return Ok(content);
        }

        /// <summary>
        /// Create a new content item
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Content>> CreateContent(
            string appName, 
            string schemaName, 
            [FromBody] CreateContentRequest request)
        {
            if (request?.Data == null)
            {
                return BadRequest(new { message = "Content data is required" });
            }

            var app = await _appRepository.GetByNameAsync(appName);
            if (app == null)
            {
                return NotFound(new { message = $"App '{appName}' not found" });
            }

            var schema = await _schemaRepository.GetByNameAsync(app.Id, schemaName);
            if (schema == null)
            {
                return NotFound(new { message = $"Schema '{schemaName}' not found" });
            }

            if (!_contentValidator.Validate(schema, request.Data, out var errors))
            {
                return BadRequest(new { message = "Validation failed", errors });
            }

            // In full implementation, we would validate against schema
            var content = new Content(app.Id, schema.Id, request.Data.ToString());
            await _contentRepository.AddAsync(content);

            return CreatedAtAction(nameof(GetContent), 
                new { appName, schemaName, id = content.Id }, content);
        }

        /// <summary>
        /// Update a content item
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Content>> UpdateContent(
            string appName, 
            string schemaName, 
            Guid id,
            [FromBody] UpdateContentRequest request)
        {
            var content = await _contentRepository.GetByIdAsync(id);
            if (content == null)
            {
                return NotFound(new { message = "Content not found" });
            }

            content.SetData(request.Data.ToString());
            await _contentRepository.UpdateAsync(content);

            return Ok(content);
        }

        /// <summary>
        /// Publish a content item
        /// </summary>
        [HttpPut("{id}/publish")]
        public async Task<ActionResult<Content>> PublishContent(
            string appName, 
            string schemaName, 
            Guid id)
        {
            var content = await _contentRepository.GetByIdAsync(id);
            if (content == null)
            {
                return NotFound(new { message = "Content not found" });
            }

            content.Publish("system"); // TODO: Get from auth
            await _contentRepository.UpdateAsync(content);

            return Ok(content);
        }

        /// <summary>
        /// Delete a content item
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContent(string appName, string schemaName, Guid id)
        {
            var content = await _contentRepository.GetByIdAsync(id);
            if (content == null)
            {
                return NotFound(new { message = "Content not found" });
            }

            await _contentRepository.DeleteAsync(id);
            return NoContent();
        }
    }

    public class CreateContentRequest
    {
        public JObject Data { get; set; }
    }

    public class UpdateContentRequest
    {
        public JObject Data { get; set; }
    }
}
