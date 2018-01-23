using System.Runtime.InteropServices;

namespace DealCloud.AddIn.Common.Utils
{
    public static class ComExceptionExt
    {
        public static string GetErrorCodeX8(this COMException e)
        {
            return $"0x{e.ErrorCode:X8}";
        }
    }
}
