using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Skoruba.AuditLogging.Constants;
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
        private readonly IAuditEventLogger _auditEventLogger;
        private readonly ILogger<AuditController> _logger;

        public AuditController(IAuditEventLogger auditEventLogger, ILogger<AuditController> logger)
        {
            _auditEventLogger = auditEventLogger;
            _logger = logger;
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