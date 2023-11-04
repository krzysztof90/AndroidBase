using Android.Content.Res;
using AndroidBase.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AndroidBase.Tools
{
    public static class EnumOperations
    {
        public static string GetEnumDescription<EnumType>(this EnumType value, Resources resources) where EnumType : struct, IConvertible, IComparable, IFormattable
        {
            int resource = value.GetEnumAttribute<EnumType, DescriptionResourceAttribute>().Resource;

            if (resource == 0)
                return String.Empty;
            return resources.GetString(resource);
        }

        public static T GetEnumAttribute<EnumType, T>(this EnumType value, T defaultValue = default) where EnumType : struct, IConvertible, IComparable, IFormattable
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            return GetFieldAttribute(fieldInfo, defaultValue);
        }

        public static T GetFieldAttribute<T>(this MemberInfo memberInfo, T defaultValue = default)
        {
            IEnumerable<T> attributes = (T[])memberInfo.GetCustomAttributes(typeof(T), false).Cast<T>();

            if (attributes != null && attributes.Any())
            {
                return attributes.Cast<T>().Single();
            }
            return defaultValue;
        }
    }
}