using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniCMS.Domain.Entities;
using MiniCMS.Infrastructure.Repositories;

namespace MiniCMS.Api.Controllers
{
    [ApiController]
    [Route("api/apps/{appName}/webhooks")]
    public class WebhooksController : ControllerBase
    {
        private readonly WebhookRepository _webhookRepository;
        private readonly AppRepository _appRepository;

        public WebhooksController(WebhookRepository webhookRepository, AppRepository appRepository)
        {
            _webhookRepository = webhookRepository;
            _appRepository = appRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Webhook>>> GetWebhooks(string appName)
        {
            var app = await _appRepository.GetByNameAsync(appName);
            if (app == null) return NotFound();

            var webhooks = await _webhookRepository.GetByAppAsync(app.Id);
            return Ok(webhooks);
        }

        [HttpPost]
        public async Task<ActionResult<Webhook>> CreateWebhook(string appName, [FromBody] CreateWebhookRequest request)
        {
            var app = await _appRepository.GetByNameAsync(appName);
            if (app == null) return NotFound();

            var webhook = new Webhook(app.Id, request.Name, request.Url, request.Events, request.Secret);
            await _webhookRepository.AddAsync(webhook);

            return CreatedAtAction(nameof(GetWebhooks), new { appName }, webhook);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWebhook(string appName, Guid id)
        {
            var app = await _appRepository.GetByNameAsync(appName);
            if (app == null) return NotFound();

            await _webhookRepository.DeleteAsync(id);
            return NoContent();
        }
    }

    public class CreateWebhookRequest
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public WebhookEvent Events { get; set; }
        public string Secret { get; set; }
    }
}
