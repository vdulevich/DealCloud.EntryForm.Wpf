using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Utils
{
    public class WinApi
    {
        public const int WM_PARENTNOTIFY = 0x0210;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x101;

        public const int EM_GETLINECOUNT = 0xba;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_NOZORDER = 0x4;

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern bool SetWindowPos(
            uint hWnd,               // handle to window
            int hWndInsertAfter,    // placement-order handle
            int X,                  // horizontal position
            int Y,                  // vertical position
            int cx,                 // width
            int cy,                 // height
            uint uFlags             // window-positioning options
        );

        public const int WM_PAINT = 0x000F;

        public const int CB_ADDSTRING = 0x143;
        public const int CB_INSERTSTRING = 330;
        public const int CB_DELETESTRING = 0x144;
        public const int CB_RESETCONTENT = 0x14B;

        public const int WM_NOTIFY = 0x004e;
        public const int WM_REFLECT = 0x2000;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int CB_SHOWDROPDOWN = 0x014F;

        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_MBUTTONDOWN = 0x0207;
        public const int WM_NCRBUTTONDOWN = 0x00A4;
        public const int WM_NCMBUTTONDOWN = 0x00A7;

        public const int CB_FINDSTRING = 0x014C;
        public const int CB_FINDSTRINGEXACT = 0x0158;

        public const int WM_USER = 0x0400;
        public const uint LVN_FIRST = unchecked(0u - 100u);
        public const uint LVN_GETEMPTYMARKUP = LVN_FIRST - 87;
        public const int L_MAX_URL_LENGTH = 2084;

        [StructLayout(LayoutKind.Sequential)]
        public struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public UInt32 code;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct NMLVEMPTYMARKUP
        {
            public NMHDR hdr;
            public UInt32 dwFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = L_MAX_URL_LENGTH)]
            public String szMarkup;
        }

        [DllImport("user32.dll")]
        static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [ResourceExposure(ResourceScope.None)]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref Point pt, int cPoints);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref System.Drawing.Rectangle rect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetActiveWindow();

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [ComImport, Guid("00000122-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleDropTarget
        {
            [PreserveSig]
            int OleDragEnter([In, MarshalAs(UnmanagedType.IUnknown)] object pDataObj, [In, MarshalAs(UnmanagedType.U4)] int grfKeyState, [In, MarshalAs(UnmanagedType.U8)] long pt, [In, Out] ref int pdwEffect);
            [PreserveSig]
            int OleDragOver([In, MarshalAs(UnmanagedType.U4)] int grfKeyState, [In, MarshalAs(UnmanagedType.U8)] long pt, [In, Out] ref int pdwEffect);
            [PreserveSig]
            int OleDragLeave();
            [PreserveSig]
            int OleDrop([In, MarshalAs(UnmanagedType.IUnknown)] object pDataObj, [In, MarshalAs(UnmanagedType.U4)] int grfKeyState, [In, MarshalAs(UnmanagedType.U8)] long pt, [In, Out] ref int pdwEffect);
        }

        [DllImport("ole32.dll")]
        public static extern int RegisterDragDrop(IntPtr hwnd, IOleDropTarget pDropTarget);

        [DllImport("ole32.dll")]
        public static extern int RevokeDragDrop(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetProp(IntPtr hWnd, string lpString);

        public static IOleDropTarget GetRegisteredDropTargetFromWnd(IntPtr hWnd)
        {
            IOleDropTarget pRetVal = null;
            IntPtr pUnknown = GetProp(hWnd, "OleDropTargetInterface");

            if (pUnknown != IntPtr.Zero)
            {
                Guid interfaceIdGuid = new Guid("00000122-0000-0000-C000-000000000046");
                IntPtr outIntPtr;
                Marshal.QueryInterface(pUnknown, ref interfaceIdGuid, out outIntPtr);
                if (outIntPtr != IntPtr.Zero)
                {
                    return (IOleDropTarget)Marshal.GetObjectForIUnknown(outIntPtr);
                }
            }
            return pRetVal;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public struct COMBOBOXINFO
        {
            public Int32 cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public int buttonState;
            public IntPtr hwndCombo;
            public IntPtr hwndEdit;
            public IntPtr hwndList;
        }

        [DllImport("user32.dll", EntryPoint = "SendMessageW", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessageCb(IntPtr hWnd, int msg, IntPtr wp, out COMBOBOXINFO lp);

        public static bool IsKeyDown(Keys keys)
        {
            return (GetKeyState((int)keys) & 0x8000) == 0x8000;
        }
    }
}
