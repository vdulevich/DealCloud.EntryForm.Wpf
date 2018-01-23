using System;
using System.Linq;
using System.Net;

namespace DealCloud.AddIn.Common.Utils
{
    public static class CookieContainerExt
    {
        public static string ToString(this CookieContainer container, Uri url)
        {
            return string.Join("\n", container.GetCookies(url).Cast<Cookie>().Select(c => $"{c.Name}={c.Value.Substring(0, c.Value.Length > 10 ? 10 : c.Value.Length)}..."));
        }
    }
}
