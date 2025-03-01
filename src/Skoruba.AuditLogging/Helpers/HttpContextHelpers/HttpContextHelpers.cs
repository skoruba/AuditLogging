// Original file: https://github.com/thepirat000/Audit.NET/blob/9ee49b5295119ef7cc6648977f90c46ce39cc698/src/Audit.WebApi/AuditApiHelper.cs
// Modified: Jan Škoruba

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.IO;

namespace Skoruba.AuditLogging.Helpers.HttpContextHelpers
{
    public class HttpContextHelpers
    {
        public static IDictionary<string, string> GetFormVariables(HttpContext context)
        {
            if (!context.Request.HasFormContentType)
            {
                return null;
            }
            IFormCollection formCollection;
            try
            {
                formCollection = context.Request.Form;
            }
            catch (InvalidDataException)
            {
                // InvalidDataException could be thrown if the form count exceeds the limit, etc
                return null;
            }
            return ToDictionary(formCollection);
        }

        public static IDictionary<string, string> ToDictionary(IEnumerable<KeyValuePair<string, StringValues>> col)
        {
            if (col == null)
            {
                return null;
            }
            var dict = new Dictionary<string, string>();
            foreach (var k in col)
            {
                dict.Add(k.Key, string.Join(", ", [.. k.Value]));
            }
            return dict;
        }
    }
}