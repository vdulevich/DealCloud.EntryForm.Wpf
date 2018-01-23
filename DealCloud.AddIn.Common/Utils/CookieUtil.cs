using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace DealCloud.AddIn.Common.Utils
{
    public class CookieUtil
    {
        public const string CookieNameAppAuth = ".AspNet.ApplicationCookie";

        public const string CookieNameWebSession = "DealCloud.Web.SessionId";

        public const string CookieNameSsoSession = "DealCloud.Sso.SessionId";

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            Int32 dwFlags,
            IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookieEx(
            string lpszUrlName, 
            string lpszCookieName, 
            string lpszCookieData, 
            int dwFlags,
            int dwReserved);

        public const Int32 InternetCookieHttponly = 0x2000;

        public static CookieContainer InternetGetCookieEx(Uri uri)
        {
            CookieContainer cookies = null;
            // Determine the size of the cookie
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
            {
                if (datasize < 0)
                {
                    return null;
                }
                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(uri.ToString(), null, cookieData,ref datasize,InternetCookieHttponly, IntPtr.Zero))
                    return null;
            }
            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }

        public static void InternetSetCookieEx(CookieContainer container, Uri uri)
        {
            foreach (Cookie cookie in container.GetCookies(uri))
            {
                InternetSetCookieEx(uri.ToString(), cookie.Name, cookie.Value, InternetCookieHttponly, 0);
            }
        }
    }
}
