﻿using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Skoruba.AuditLogging.Events;

namespace Skoruba.AuditLogging.Helpers.JsonHelpers
{
    public class AuditLoggerContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Ignore base properties of audit event, because these properties are stored separately
        /// </summary>
        /// <param name="member"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (typeof(AuditEvent).IsAssignableFrom(member.DeclaringType) 
                && (member.Name == nameof(AuditEvent.Id)
                    || member.Name == nameof(AuditEvent.Category)
                    || member.Name == nameof(AuditEvent.SubjectIdentifier)
                    || member.Name == nameof(AuditEvent.SubjectName)))
            {
                property.ShouldSerialize = i => false;
                property.Ignored = true;
            }
            return property;
        }
    }
}