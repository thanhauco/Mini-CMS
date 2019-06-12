using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniCMS.Domain.Entities;
using MiniCMS.Infrastructure.Repositories;

namespace MiniCMS.Api.Controllers
{
    [ApiController]
    [Route("api/apps/{appName}/schemas")]
    public class SchemasController : ControllerBase
    {
        private readonly SchemaRepository _schemaRepository;
        private readonly AppRepository _appRepository;

        public SchemasController(SchemaRepository schemaRepository, AppRepository appRepository)
        {
            _schemaRepository = schemaRepository;
            _appRepository = appRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schema>>> GetSchemas(string appName)
        {
            var app = await _appRepository.GetByNameAsync(appName);
            if (app == null) return NotFound();

            var schemas = await _schemaRepository.GetByAppAsync(app.Id);
            return Ok(schemas);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Schema>> GetSchema(string appName, string name)
        {
            var app = await _appRepository.GetByNameAsync(appName);
            if (app == null) return NotFound();

            var schema = await _schemaRepository.GetByNameAsync(app.Id, name);
            if (schema == null) return NotFound();

            return Ok(schema);
        }

        [HttpPost]
        public async Task<ActionResult<Schema>> CreateSchema(string appName, [FromBody] CreateSchemaRequest request)
        {
            var app = await _appRepository.GetByNameAsync(appName);
            if (app == null) return NotFound();

            var schema = new Schema(app.Id, request.Name, request.DisplayName);
            await _schemaRepository.AddAsync(schema);

            return CreatedAtAction(nameof(GetSchema), new { appName, name = schema.Name }, schema);
        }

        [HttpDelete("{name}")]
        public async Task<ActionResult> DeleteSchema(string appName, string name)
        {
            var app = await _appRepository.GetByNameAsync(appName);
            if (app == null) return NotFound();

            var schema = await _schemaRepository.GetByNameAsync(app.Id, name);
            if (schema == null) return NotFound();

            await _schemaRepository.DeleteAsync(schema.Id);
            return NoContent();
        }
    }

    public class CreateSchemaRequest
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
