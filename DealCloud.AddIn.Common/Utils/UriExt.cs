using DealCloud.Common.Extensions;
using System;

namespace DealCloud.AddIn.Common.Utils
{
    public static class UriExt
    {
        public static bool IsHttpScheme(this Uri uri)
        {
            return uri.Scheme.Contains("http", StringComparison.OrdinalIgnoreCase);
        }
    }
}
