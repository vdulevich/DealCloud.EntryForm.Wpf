using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace DealCloud.AddIn.Common.Utils
{
    public static class EnumExtensionsExt
    {
        public static string GetDisplayName(this Enum e, Type resorceType )
        {
            var rm = new ResourceManager(resorceType);
            var resourceDisplayName = rm.GetString(e.GetType().Name + "_" + e);

            return string.IsNullOrWhiteSpace(resourceDisplayName) ? $"[[{e}]]" : resourceDisplayName;
        }

        public static Dictionary<T, string> GetValues<T>(Type resorceType)
        {
            Dictionary<T, string> result = new Dictionary<T, string>();
            foreach (Enum value in Enum.GetValues(typeof(T)))
            {
                result[(T)Enum.ToObject(typeof(T), value)] = value.GetDisplayName(resorceType);
            }
            return result;
        }
    }
}
