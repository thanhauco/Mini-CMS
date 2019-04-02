using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniCMS.Domain.Entities;
using MiniCMS.Infrastructure.Repositories;
using MiniCMS.Infrastructure.Services;

namespace MiniCMS.Api.Controllers
{
    [ApiController]
    [Route("api/apps/{appName}/assets")]
    public class AssetsController : ControllerBase
    {
        private readonly AppRepository _appRepository;
        private readonly IFileStorageService _fileStorageService;

        public AssetsController(
            AppRepository appRepository,
            IFileStorageService fileStorageService)
        {
            _appRepository = appRepository;
            _fileStorageService = fileStorageService;
        }

        /// <summary>
        /// Upload an asset
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Asset>> UploadAsset(
            string appName,
            IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded" });
            }

            var app = await _appRepository.GetByNameAsync(appName);
            if (app == null)
            {
                return NotFound(new { message = $"App '{appName}' not found" });
            }

            using (var stream = file.OpenReadStream())
            {
                var storagePath = await _fileStorageService.SaveFileAsync(
                    stream, 
                    file.FileName, 
                    file.ContentType);

                stream.Position = 0;
                var hash = _fileStorageService.GetFileHash(stream);

                var asset = new Asset(
                    app.Id,
                    file.FileName,
                    file.ContentType,
                    file.Length,
                    storagePath);
                
                asset.SetFileHash(hash);

                // In full implementation, save to repository
                return Ok(asset);
            }
        }

        /// <summary>
        /// Download an asset
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsset(string appName, Guid id)
        {
            // In full implementation, fetch from repository
            // For now, return placeholder
            return NotFound(new { message = "Asset not found" });
        }

        /// <summary>
        /// Delete an asset
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsset(string appName, Guid id)
        {
            // In full implementation, remove from storage and repository
            return NoContent();
        }
    }
}
