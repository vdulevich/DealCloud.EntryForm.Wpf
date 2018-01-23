using System;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace DealCloud.AddIn.Common.Utils
{
    public static class RegistryUtil
    {
        public static bool GetOpenKey(RegistryKey key, string comName)
        {
            Regex regex = new Regex("^open\\d*$", RegexOptions.IgnoreCase);
            foreach (string valueName in key.GetValueNames())
            {
                if (regex.IsMatch(valueName) && key.GetValue(valueName).ToString().IndexOf(comName, StringComparison.OrdinalIgnoreCase) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetOpenIndex(RegistryKey key)
        {
            Regex regex = new Regex("^open\\d*$", RegexOptions.IgnoreCase);
            string index = "";
            int indexNext;
            foreach (string valueName in key.GetValueNames())
            {
                if (regex.IsMatch(valueName))
                {
                    index = valueName.Substring(4);
                }
            }
            int.TryParse(index, out indexNext);
            return (indexNext + 1).ToString();
        }

        public static void RemoveOpenKey(RegistryKey key)
        {
            Regex regex = new Regex("^open\\d*$", RegexOptions.IgnoreCase);
            foreach (string valueName in key.GetValueNames())
            {
                if (regex.IsMatch(valueName))
                {
                    key.DeleteValue(valueName);
                }
            }
        }
    }
}
