using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Skoruba.AuditLogging.Constants;
using Skoruba.AuditLogging.Host.Dtos;
using Skoruba.AuditLogging.Host.Events;
using Skoruba.AuditLogging.Services;
using System;
using System.Threading.Tasks;

namespace Skoruba.AuditLogging.Host.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController(IAuditEventLogger auditEventLogger, ILogger<AuditController> logger) : ControllerBase
    {
        private readonly IAuditEventLogger _auditEventLogger = auditEventLogger;
        private readonly ILogger<AuditController> _logger = logger;

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            // Create fake product
            var productDto = new ProductDto
            {
                Id = Guid.CreateVersion7().ToString(),
                Name = Guid.CreateVersion7().ToString(),
                Category = Guid.CreateVersion7().ToString()
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

            await _auditEventLogger.LogEventAsync(productGetMachineEvent, options =>
                {
                    options.UseDefaultSubject = false;
                    options.UseDefaultAction = false;
                });

            await _auditEventLogger.LogEventAsync(productGetUserEvent);

            var genericProductEvent = new GenericProductEvent<int, string, ProductDto>
            {
                Category = nameof(ProductGetEvent),
                Product = productDto
            };

            await _auditEventLogger.LogEventAsync(genericProductEvent);

            return Ok(productDto);
        }
    }
}