using System;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Utils
{
    public class NativeWindowAssignHandle: IWin32Window
    {
        public NativeWindowAssignHandle()
        {
            Handle = WinApi.GetActiveWindow();
        }

        public NativeWindowAssignHandle(IntPtr handle)
        {
            Handle = handle;
        }

        public IntPtr Handle { get; }
    }
}
