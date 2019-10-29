using System;

namespace Skoruba.AuditLogging.Helpers.Common
{
    public static class TypeHelpers
    {
        public static string GetNameWithoutGenericParams(this Type type)
        {
            return type.IsGenericType ? type.Name.Remove(type.Name.IndexOf('`')) : type.Name;
        }
    }
}
