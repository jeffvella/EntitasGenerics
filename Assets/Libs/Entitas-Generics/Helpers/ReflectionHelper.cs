using System;
using System.Linq;
using System.Reflection;

namespace Entitas.Generics
{
    public static class ReflectionHelper
    {
        public static bool HasAttribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault() != null;
        }

        public static bool TryGetAttribute<T>(MemberInfo memberInfo, out T customAttribute) where T : Attribute
        {
            var attributes = memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            if (attributes == null)
            {
                customAttribute = null;
                return false;
            }
            customAttribute = (T)attributes;
            return true;
        }

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