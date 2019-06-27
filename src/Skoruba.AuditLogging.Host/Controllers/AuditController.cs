using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skoruba.AuditLogging.Host.Consts;
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
            var productGetUserEvent = new ProductGetEvent
            {
                Category = nameof(ProductGetEvent),
                Product = productDto
            };

            var productGetMachineEvent = new ProductGetEvent
            {
                Category = nameof(ProductGetEvent),
                Product = productDto,
                SubjectType = AuditSubjectTypes.Machine,
                SubjectName = Environment.MachineName,
                SubjectIdentifier = Environment.MachineName,
                Action = new { Method = nameof(Get), Class = nameof(AuditController) }
            };

            await _auditLogger.LogAsync(productGetMachineEvent, false, false);
            await _auditLogger.LogAsync(productGetUserEvent);

            return Ok(productDto);
        }
    }
}