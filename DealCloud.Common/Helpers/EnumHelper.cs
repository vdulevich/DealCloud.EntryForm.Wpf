using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DealCloud.Common.Extensions;

namespace DealCloud.Common.Helpers
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MemberDescriptionAttribute : Attribute
    {
        public MemberDescriptionAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }

    public static class EnumHelper
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<object, string>> _enumDescriptions = new ConcurrentDictionary<Type, Dictionary<object, string>>();

        public static string GetMemberDescription<TEnum>(TEnum enumMember) where TEnum : struct
        {
            var enumType = GetEnumType<TEnum>();
            var memberDescriptions = _enumDescriptions.GetOrAdd(enumType, InitEnumDescriptions);

            var result = memberDescriptions.ContainsKey(enumMember) ? memberDescriptions[enumMember] : null;

            return result;
        }

        public static Dictionary<TEnum, string> GetMemberDescriptions<TEnum>() where TEnum : struct
        {
            var enumType = GetEnumType<TEnum>();
            var memberDescriptions = _enumDescriptions.GetOrAdd(enumType, InitEnumDescriptions);

            var result = memberDescriptions.ToDictionary(x => (TEnum) x.Key, x => x.Value);

            return result;
        }

        public static ILookup<string, TEnum> GetMemberDescriptionsByString<TEnum>() where TEnum : struct
        {
            var enumType = GetEnumType<TEnum>();
            var memberDescriptions = _enumDescriptions.GetOrAdd(enumType, InitEnumDescriptions);

            var result = memberDescriptions.ToLookup(x => x.Value, x => (TEnum)x.Key, StringComparer.OrdinalIgnoreCase);

            return result;
        }

        public static TEnum ParseFromMemberDescription<TEnum>(string value, bool isFlags) where TEnum : struct
        {
            if (value.IsNullOrEmpty()) return default(TEnum);

            var descriptions = GetMemberDescriptionsByString<TEnum>();
            TEnum result = descriptions[value].FirstOrDefault();

            if (isFlags)
            {
                var arr = value.Split(Constants.COMMA_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

                foreach (var str in arr)
                {
                    result = (TEnum) (object) (((int) (object) result) | ((int) (object) descriptions[str.Trim()].FirstOrDefault()));
                }
            }

            return result;
        }

        private static Type GetEnumType<TEnum>() where TEnum : struct
        {
            var enumType = typeof (TEnum);

            if (!enumType.IsEnum) throw new InvalidOperationException("Enum type is expected.");

            return enumType;
        }

        private static Dictionary<object, string> InitEnumDescriptions(Type enumType)
        {
            var result = new Dictionary<object, string>();
            var members = Enum.GetValues(enumType);

            foreach (var member in members)
            {
                if (member == null) continue;

                var hasDescription = false;
                var memberInfos = enumType.GetMember(member.ToString());

                if (memberInfos.Length > 0)
                {
                    var attrs = memberInfos[0].GetCustomAttributes(typeof (MemberDescriptionAttribute), false);

                    if (attrs.Length > 0)
                    {
                        var descriptionAttr = attrs[0] as MemberDescriptionAttribute;

                        if (!string.IsNullOrEmpty(descriptionAttr?.Description))
                        {
                            result.Add(member, descriptionAttr.Description);

                            hasDescription = true;
                        }
                    }
                }

                if (!hasDescription) result.Add(member, member.ToString());
            }

            return result;
        }
    }
}