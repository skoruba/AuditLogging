// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

// Original file: https://github.com/IdentityServer/IdentityServer4/blob/master/src/IdentityServer4/src/Logging/LogSerializer.cs
// Modified by Jan Škoruba

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Formatting = Newtonsoft.Json.Formatting;

namespace Skoruba.AuditLogging.Helpers.JsonHelpers
{
    /// <summary>
    /// Helper to JSON serialize object data for audit logging.
    /// </summary>
    public static class AuditLogSerializer
    {
        public static readonly JsonSerializerSettings DefaultAuditJsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            Formatting = Formatting.Indented
        };

        public static readonly JsonSerializerSettings BaseAuditEventJsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            Formatting = Formatting.Indented
        };

        static AuditLogSerializer()
        {
            DefaultAuditJsonSettings.Converters.Add(new StringEnumConverter());
            BaseAuditEventJsonSettings.Converters.Add(new StringEnumConverter());
            BaseAuditEventJsonSettings.ContractResolver = new AuditLoggerContractResolver();
        }

        /// <summary>
        /// Serializes the audit event object.
        /// </summary>
        /// <param name="logObject">The object.</param>
        /// <returns></returns>
        public static string Serialize(object logObject)
        {
            return JsonConvert.SerializeObject(logObject, DefaultAuditJsonSettings);
        }

        /// <summary>
        /// Serializes the audit event object.
        /// </summary>
        /// <param name="logObject">The object.</param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static string Serialize(object logObject, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(logObject, settings);
        }
    }
}