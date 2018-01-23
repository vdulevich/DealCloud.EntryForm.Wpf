using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Linq;
using System.Globalization;

namespace DealCloud.Common.Helpers
{
    public static class EmailParseHelper
    {
        public static IEnumerable<string> ParseAllEmailAddressess(string data)
        {
            HashSet<String> emailAddressess = new HashSet<string>();
            Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
            MatchCollection emailMatches = emailRegex.Matches(data);
            foreach (Match emailMatch in emailMatches)
            {
                if (!string.IsNullOrEmpty(emailMatch.Value) && !IsImageExtension(emailMatch.Value) && (IsValidEmail(emailMatch.Value)))
                {
                    emailAddressess.Add(emailMatch.Value);
                }
            }
            return emailAddressess;
        }


        public static readonly string[] _validExtensions = { "jpg", "bmp", "gif", "png", "jpeg", "tiff", "raw", "psd" };

        public static bool IsImageExtension(string email)
        {
            bool isContainsImageExt = false;

            MailAddress addr = new MailAddress(email);
            string username = addr.User;
            if (!string.IsNullOrEmpty(username) && username.Contains('.'))
            {
                String[] parts = username.Split(new[] { '.' });
                if (!string.IsNullOrEmpty(parts[0]) && !string.IsNullOrEmpty(parts[1]))
                {
                    if (_validExtensions.Contains(parts[1].ToLower()) && (parts[0].ToLower().Contains("image")))
                    {
                        isContainsImageExt = true;
                    }
                }
            }

            return isContainsImageExt;
        }

        static bool invalid = false;
        public static bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            strIn = Regex.Replace(strIn, @"(@)(.+)$", EmailParseHelper.DomainMapper);
            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn,
                   @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                   RegexOptions.IgnoreCase);
        }

        public static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}
