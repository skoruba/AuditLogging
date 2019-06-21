using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skoruba.AuditLogging.Host.Dtos;
using Skoruba.AuditLogging.Host.Events;
using Skoruba.AuditLogging.Services;

namespace Skoruba.AuditLogging.Host.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly IAuditLogger _auditLogger;

        public AuditController(IAuditLogger auditLogger)
        {
            _auditLogger = auditLogger;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            // Create fake product
            var productDto = new ProductDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Category = Guid.NewGuid().ToString()
            };

            // Log this action
            var productGetEvent = new ProductGetEvent
            {
                Category = nameof(ProductGetEvent),
                SubjectIdentifier = Guid.NewGuid().ToString(),
                SubjectName = Guid.NewGuid().ToString(),
                Product = productDto
            };

            await _auditLogger.LogAsync(productGetEvent, false);

            await _auditLogger.LogAsync(productGetEvent, true);

            return Ok(productDto);
        }
    }
}