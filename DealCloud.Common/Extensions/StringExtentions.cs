using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DealCloud.Common.Extensions
{
    public static class StringExtentions
    {
        private static Regex htmlToPlain = new Regex("<[^>]*>");

        public static string TrimSafe(this string source)
        {
            if (!source.IsNullOrEmpty())
            {
                return source.Trim();
            }

            return source;
        }

        public static string SubstringSafe(this string source, int takeCount)
        {
            if (!source.IsNullOrEmpty())
            {
                if (source.Length <= takeCount)
                {
                    return source;
                }

                return source.Substring(0, takeCount - 1);
            }

            return source;
        }

        public static bool SafeEquals(this string source, string other, StringComparison comparison = StringComparison.Ordinal)
        {
            return string.Equals(source, other, comparison);
        }

        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        public static bool Contains(this string source, string term, StringComparison comparer)
        {
            return source.IndexOf(term, comparer) >= 0; 
        }

        public static string FormatDomainName(this string source)
        {
            return source?.Trim().TrimEnd('/').ToLower();
        }

        public static string NormalizeName(this string text)
        {
            return text.Replace("\"", "");
        }

        public static bool Matches(this string text, string pattern)
        {
            return Regex.IsMatch(text, pattern);
        }

        public static string EscapeWordMergeFiledName(this string source)
        {
            return source.IsNullOrWhiteSpace()
                ? source
                : source.Replace("'", string.Empty)
                        .Replace("\"", string.Empty)
                        .Replace("\\", string.Empty)
                        .Replace(" ", "_");
        }

        public static string GetMd5Hash(this string source)
        {
            var result = string.Empty;

            if (!source.IsNullOrEmpty())
            {
                using (var md5 = MD5.Create())
                {
                   var hash = md5.ComputeHash(Encoding.Unicode.GetBytes(source));

                    result = BitConverter.ToString(hash).Replace("-", "");
                }
            }

            return result;
        }

        public static string ResolveWildcards(this string source, Dictionary<string, string> wildcards)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (wildcards == null) throw new ArgumentNullException(nameof(wildcards));

            const char start = '[';
            const char end = ']';
            var builder = new StringBuilder();
            var currentWildcard = new List<char>();
            var isWildcard = false;

            foreach (var letter in source)
            {
                if (letter == start)
                {
                    if (currentWildcard.Any())
                    {
                        builder.Append(new string(currentWildcard.ToArray()));

                        currentWildcard.Clear();
                    }

                    currentWildcard.Add(letter);

                    isWildcard = true;

                    continue;
                }

                if (letter == end)
                {
                    currentWildcard.Add(letter);

                    isWildcard = false;

                    var wildcardValue = new string(currentWildcard.ToArray());

                    builder.Append(wildcards.GetValueOrDefault(wildcardValue) ?? wildcardValue);

                    currentWildcard.Clear();

                    continue;
                }

                if (isWildcard)
                {
                    currentWildcard.Add(letter);
                }
                else
                {
                    builder.Append(letter);
                }
            }

            if (currentWildcard.Any())
            {
                builder.Append(new string(currentWildcard.ToArray()));
            }

            return builder.ToString();
        }

        public static string HtmlToPlain(this string html)
        {
            return htmlToPlain.Replace(html, string.Empty);
        }

        public static string HtmlDecode(this string html)
        {
            return WebUtility.HtmlDecode(html);
        }
    }
}