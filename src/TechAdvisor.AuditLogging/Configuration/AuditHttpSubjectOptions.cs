using TechAdvisor.AuditLogging.Constants;

namespace TechAdvisor.AuditLogging.Configuration
{
    public class AuditHttpSubjectOptions
    {
        public string SubjectIdentifierClaim { get; set; } = ClaimsConsts.Sub;

        public string SubjectNameClaim { get; set; } = ClaimsConsts.Name;
    }
}