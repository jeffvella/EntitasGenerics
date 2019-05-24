using System;
using System.Linq;
using System.Reflection;

namespace Entitas.Generics
{
    public static class ReflectionHelper
    {
        public static string PrettyPrintGenericTypeName(this Type type)
        {
            if (type.IsGenericType)
            {
                var simpleName = type.Name.Substring(0, type.Name.IndexOf('`'));
                var genericTypeParams = type.GenericTypeArguments.Select(PrettyPrintGenericTypeName).ToList();
                return string.Format("{0}<{1}>", simpleName, string.Join(", ", genericTypeParams));
            }
            return type.Name;
        }
    }
}