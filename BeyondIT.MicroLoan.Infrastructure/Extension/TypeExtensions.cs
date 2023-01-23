using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BeyondIT.MicroLoan.Infrastructure.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsNumericType(this Type type)
        {
            return IsIntegralType(type) || IsDecimalType(type);
        }

        public static bool IsIntegralType(this Type type)
        {
            return type == typeof(int) || type == typeof(int?) ||
                   type == typeof(long) || type == typeof(long?);
        }

        public static bool IsDecimalType(this Type type)
        {
            return type == typeof(double) || type == typeof(double?) ||
                   type == typeof(decimal) || type == typeof(decimal?);
        }

        public static bool IsDateTimeType(this Type type)
        {
            return type == typeof(DateTime) || type == typeof(DateTime?);
        }

        public static bool IsBooleanType(this Type type)
        {
            return type == typeof(bool);
        }

        public static bool IsCollectionType(this Type type)
        {
            if (type.IsGenericType() && type.GetGenericTypeDefinition() == typeof(ICollection<>))
                return true;
            return type.GetInterfaces().Where(t => t.IsGenericType()).Select(t => t.GetGenericTypeDefinition()).Any(t => t == typeof(ICollection<>));
        }

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static bool IsEnumerableType(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }
    }
}