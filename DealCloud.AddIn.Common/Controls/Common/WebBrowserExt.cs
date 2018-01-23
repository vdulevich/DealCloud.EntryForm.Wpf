using DealCloud.AddIn.Common.Controls.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DealCloud.Common.Serialization;
using Newtonsoft.Json;
using DealCloud.AddIn.Common.Utils;
using DealCloud.Common.Extensions;
using System.Security;
using mshtml;
using System.Linq;
using NLog;

namespace DealCloud.AddIn.Common.Controls
{
    public class WebBrowserExt : WebBrowser
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly string AboutBlank = "about:blank";

        public event Action<bool> Loading;

        public event Action NavigateError;

        public event Action NavigateRestricted;

        public event Action Timeout;

        private Timer _timer;

        private int _timerInterval = -1;

        private HTMLDocumentEvents2_Event _domDocument;

        private List<Tuple<string, object[]>> _delayedScripts = new List<Tuple<string, object[]>>();

        public Uri RestrictedUri { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { if (_isLoading != value && Loading != null) { Loading(value); } _isLoading = value; }
        }

        public bool HasDelayedScripts => _delayedScripts.Any();

        public bool CanInvokeScripts => !IsDisposed && Url != null && !Url.ToString().Equals(AboutBlank) && !IsLoading;

        public HttpStatusCode StatusCode { get; private set; }

        public bool HasErorr { get; private set; }

        public string ErrorDescription => HasErorr ? _errorsTable.GetValueOrDefault($"0x{(int)StatusCode:X}") ?? ErrorMessageBaseUtil.GetErrorMessage(StatusCode) : null;

        public int TimeoutInterval
        {
            get { return _timerInterval; }
            set
            {
                _timerInterval = value;
                if (_timerInterval > 0)
                {
                    if (_timer == null)
                    {
                        _timer = new Timer();
                        _timer.Tick += Timer_Tick;
                    }
                    _timer.Interval = _timerInterval;
                }
                else
                {
                    _timer?.Stop();
                }
            }
        }

        public WebBrowserExt()
        {
            this.IsWebBrowserContextMenuEnabled = false;
            this.AllowWebBrowserDrop = false;
        }

        public object InvokeScript(string name, params object[] parameters)
        {
            if (this.InvokeRequired) return this.Invoke(new Func<object>(() => InvokeScript(name, parameters)));
            if (!CanInvokeScripts)
            {
                _delayedScripts.Add(new Tuple<string, object[]>(name, parameters));
                return null;
            }
            else
            {
                return this.Document?.InvokeScript(name, parameters);
            }
        }

        public object InvokeScriptJson(string name, object parameter, bool autoType = true)
        {
            return InvokeScript(name, JsonConvert.SerializeObject(parameter, autoType ?
                SerializationHelper.JsonSettingsCamelCaseTypeAuto :
                SerializationHelper.JsonSettingsCamelCase));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Timeout?.Invoke();
        }

        private void Wb2_BeforeNavigate2(object pDisp, ref object url, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
        {
            HasErorr = false;
            StatusCode = HttpStatusCode.OK;
        }

        private void Wb2_NavigateError(object pDisp, ref object url, ref object frame, ref object statusCode, ref bool cancel)
        {
            HasErorr = true;
            StatusCode = (HttpStatusCode)(int)statusCode;
            NavigateError?.Invoke();
            IsLoading = false;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            SHDocVw.WebBrowser wb2 = (SHDocVw.WebBrowser)ActiveXInstance;
            wb2.NavigateError += Wb2_NavigateError;
            wb2.BeforeNavigate2 += Wb2_BeforeNavigate2;
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                SHDocVw.WebBrowser wb2 = (SHDocVw.WebBrowser)ActiveXInstance;
                wb2.NavigateError -= Wb2_NavigateError;
                wb2.BeforeNavigate2 -= Wb2_BeforeNavigate2;
                if (_domDocument != null)
                {
                    _domDocument.onmousewheel -= docEvents_onmousewheel;
                    Marshal.ReleaseComObject(_domDocument);
                }
                _timer?.Dispose();
                //release events
                NavigateRestricted = null;
                NavigateError = null;
                Loading = null;
                Timeout = null;
                _domDocument = null;
                base.Dispose(disposing);
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
        }

        protected override void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e)
        {
            base.OnDocumentCompleted(e);
            _timer?.Stop();
            _domDocument = (HTMLDocumentEvents2_Event)this.Document.DomDocument;
            _domDocument.onmousewheel -= docEvents_onmousewheel;
            _domDocument.onmousewheel += docEvents_onmousewheel;
            IsLoading = false;
            if (HasDelayedScripts && CanInvokeScripts)
            {
                _delayedScripts.ForEach(s => { InvokeScript(s.Item1, s.Item2); });
                _delayedScripts.Clear();
            }
        }

        bool docEvents_onmousewheel(mshtml.IHTMLEventObj pEvtObj)
        {
            if (pEvtObj.ctrlKey)
            {
                pEvtObj.cancelBubble = true; //not sure what this does really
                pEvtObj.returnValue = false;  //this cancels the event
                return false; //not sure what this does really
            }
            else
                return true; //again not sure what this does
        }

        protected override void OnNavigating(WebBrowserNavigatingEventArgs e)
        {
            if (RestrictedUri != null && e.Url.DnsSafeHost.IndexOf(RestrictedUri.DnsSafeHost, StringComparison.OrdinalIgnoreCase) > -1)
            {
                e.Cancel = true;
                NavigateRestricted?.Invoke();
            }
            base.OnNavigating(e);
            _timer?.Start();
            if (e.Url.IsHttpScheme())
            {
                IsLoading = true;
            }
        }

        protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
        {
            return new DCWebBrowserSite(this);
        }

        protected class DCWebBrowserSite : WebBrowserSite, UnsafeNativeMethods.IDocHostUIHandler, IOleCommandTarget, ICustomQueryInterface
        {
            UnsafeNativeMethods.IDocHostUIHandler _baseIDocHostUIHandler;

            IntPtr _unkInnerAggregated;
            IntPtr _unkOuter;
            Inner _inner;

            public DCWebBrowserSite(WebBrowser host) : base(host)
            {
                // get the CCW object for this
                _unkOuter = Marshal.GetIUnknownForObject(this);
                Marshal.AddRef(_unkOuter);
                try
                {
                    // aggregate the CCW object with the helper Inner object
                    _inner = new Inner(this);
                    _unkInnerAggregated = Marshal.CreateAggregatedObject(_unkOuter, _inner);

                    // obtain private WebBrowserSite COM interfaces
                    _baseIDocHostUIHandler = (UnsafeNativeMethods.IDocHostUIHandler)Marshal.GetTypedObjectForIUnknown(_unkInnerAggregated, typeof(UnsafeNativeMethods.IDocHostUIHandler));
                }
                finally
                {
                    Marshal.Release(_unkOuter);
                }
            }

            ~DCWebBrowserSite()
            {
                Dispose(false);
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (_inner != null)
                {
                    _inner?.Dispose();
                    _inner = null;
                }

                if (_baseIDocHostUIHandler != null && Marshal.IsComObject(_baseIDocHostUIHandler))
                {
                    Marshal.ReleaseComObject(_baseIDocHostUIHandler);
                    _baseIDocHostUIHandler = null;
                }

                if (_unkInnerAggregated != IntPtr.Zero)
                {
                    Marshal.Release(_unkInnerAggregated);
                    _unkInnerAggregated = IntPtr.Zero;
                }

                if (_unkOuter != IntPtr.Zero)
                {
                    Marshal.Release(_unkOuter);
                    _unkOuter = IntPtr.Zero;
                }
            }

            #region ICustomQueryInterface

            public CustomQueryInterfaceResult GetInterface(ref Guid iid, out IntPtr ppv)
            {
                // CustomQueryInterfaceMode.Ignore is to avoid infinite loop during QI.
                if (iid == typeof(UnsafeNativeMethods.IDocHostUIHandler).GUID)
                {
                    ppv = Marshal.GetComInterfaceForObject(this, typeof(UnsafeNativeMethods.IDocHostUIHandler), CustomQueryInterfaceMode.Ignore);
                }
                else
                {
                    ppv = IntPtr.Zero;
                    return CustomQueryInterfaceResult.NotHandled;
                }
                return CustomQueryInterfaceResult.Handled;
            }

            #endregion

            #region IDocHostUIHandler

            int UnsafeNativeMethods.IDocHostUIHandler.ShowContextMenu(int dwID, ref UnsafeNativeMethods.POINT pt, IntPtr pcmdtReserved, IntPtr pdispReserved)
            {
                return _baseIDocHostUIHandler.ShowContextMenu(dwID, ref pt, pcmdtReserved, pdispReserved);
            }

            int UnsafeNativeMethods.IDocHostUIHandler.GetHostInfo(ref UnsafeNativeMethods.DOCHOSTUIINFO info)
            {
                Debug.Print("IDocHostUIHandler.GetHostInfo");
                var ret = _baseIDocHostUIHandler.GetHostInfo(ref info);
                info.dwFlags = info.dwFlags & 0x6FFFFFFF;
                info.dwFlags = info.dwFlags | 0x08000000;
                info.dwFlags = info.dwFlags | 0x40000000;
                return ret;
            }

            int UnsafeNativeMethods.IDocHostUIHandler.ShowUI(int dwID, IntPtr activeObject, IntPtr commandTarget, IntPtr frame, IntPtr doc)
            {
                return _baseIDocHostUIHandler.ShowUI(dwID, activeObject, commandTarget, frame, doc);
            }

            int UnsafeNativeMethods.IDocHostUIHandler.HideUI()
            {
                return _baseIDocHostUIHandler.HideUI();
            }

            int UnsafeNativeMethods.IDocHostUIHandler.UpdateUI()
            {
                return _baseIDocHostUIHandler.UpdateUI();
            }

            int UnsafeNativeMethods.IDocHostUIHandler.EnableModeless(bool fEnable)
            {
                return _baseIDocHostUIHandler.EnableModeless(fEnable);
            }

            int UnsafeNativeMethods.IDocHostUIHandler.OnDocWindowActivate(bool fActivate)
            {
                return _baseIDocHostUIHandler.OnDocWindowActivate(fActivate);
            }

            int UnsafeNativeMethods.IDocHostUIHandler.OnFrameWindowActivate(bool fActivate)
            {
                return _baseIDocHostUIHandler.OnFrameWindowActivate(fActivate);
            }

            int UnsafeNativeMethods.IDocHostUIHandler.ResizeBorder(ref UnsafeNativeMethods.COMRECT rect, IntPtr doc, bool fFrameWindow)
            {
                return _baseIDocHostUIHandler.ResizeBorder(ref rect, doc, fFrameWindow);
            }

            int UnsafeNativeMethods.IDocHostUIHandler.TranslateAccelerator(ref UnsafeNativeMethods.MSG msg, ref Guid group, int nCmdID)
            {
                if (msg.message != WinApi.WM_KEYDOWN)
                {
                    return _baseIDocHostUIHandler.TranslateAccelerator(ref msg, ref @group, nCmdID);
                }
                KeyEventArgs e = new KeyEventArgs((Keys)((int)msg.wParam & 0xff) | Control.ModifierKeys);
                if ((e.Modifiers == Keys.Control && (e.KeyCode == Keys.O ||
                                                     e.KeyCode == Keys.L ||
                                                     e.KeyCode == Keys.F ||
                                                     e.KeyCode == Keys.P ||
                                                     e.KeyCode == Keys.N ||
                                                     e.KeyCode == Keys.Oemplus ||
                                                     e.KeyCode == Keys.OemMinus)) ||
                    e.KeyCode == Keys.F5)
                {
                    return 0;
                }
                else
                {
                    return _baseIDocHostUIHandler.TranslateAccelerator(ref msg, ref @group, nCmdID);
                }
            }

            int UnsafeNativeMethods.IDocHostUIHandler.GetOptionKeyPath(string[] pbstrKey, int dw)
            {
                return _baseIDocHostUIHandler.GetOptionKeyPath(pbstrKey, dw);
            }

            int UnsafeNativeMethods.IDocHostUIHandler.GetDropTarget(IntPtr pDropTarget, out IntPtr ppDropTarget)
            {
                Debug.Print(nameof(UnsafeNativeMethods.IDocHostUIHandler.GetDropTarget));
                return _baseIDocHostUIHandler.GetDropTarget(pDropTarget, out ppDropTarget);
            }

            int UnsafeNativeMethods.IDocHostUIHandler.GetExternal(out object ppDispatch)
            {
                return _baseIDocHostUIHandler.GetExternal(out ppDispatch);
            }

            int UnsafeNativeMethods.IDocHostUIHandler.TranslateUrl(int dwTranslate, string strURLIn, out string pstrURLOut)
            {
                return _baseIDocHostUIHandler.TranslateUrl(dwTranslate, strURLIn, out pstrURLOut);
            }

            int UnsafeNativeMethods.IDocHostUIHandler.FilterDataObject(IntPtr pDO, out IntPtr ppDORet)
            {
                return _baseIDocHostUIHandler.FilterDataObject(pDO, out ppDORet);
            }

            [return: MarshalAs(UnmanagedType.I4)]
            public int QueryStatus(ref Guid pguidCmdGroup, int cCmds, [In, Out] OLECMD prgCmds, [In, Out] IntPtr pCmdText)
            {
                if ((int)OLECMDID.OLECMDID_SHOWSCRIPTERROR == prgCmds.cmdID)
                {   // Do nothing (suppress script errors)
                    return S_OK;
                }

                // Indicate that command is unknown. The command will then be handled by another IOleCommandTarget.
                return OLECMDERR_E_UNKNOWNGROUP;
            }

            [return: MarshalAs(UnmanagedType.I4)]
            public int Exec(ref Guid pguidCmdGroup, int nCmdID, int nCmdexecopt, [In, MarshalAs(UnmanagedType.LPArray)] object[] pvaIn, int pvaOut)
            {
                if ((int)OLECMDID.OLECMDID_SHOWSCRIPTERROR == nCmdID)
                {   // Do nothing (suppress script errors)
                    return S_OK;
                }

                // Indicate that command is unknown. The command will then be handled by another IOleCommandTarget.
                return OLECMDERR_E_UNKNOWNGROUP;
            }

            #endregion

            class Inner : ICustomQueryInterface, IDisposable
            {
                object _outer;

                Dictionary<Guid, Type> _interfaces;

                public Inner(object outer)
                {
                    _outer = outer;
                    _interfaces = new Dictionary<Guid, Type>();
                    foreach (var iface in _outer.GetType().BaseType.GetInterfaces())
                    {
                        _interfaces[iface.GUID] = iface;
                    }
                }

                public CustomQueryInterfaceResult GetInterface(ref Guid iid, out IntPtr ppv)
                {
                    if (_outer != null)
                    {
                        var guid = iid;
                        Type iface;
                        if (_interfaces.TryGetValue(guid, out iface))
                        {
                            var unk = Marshal.GetComInterfaceForObject(_outer, iface, CustomQueryInterfaceMode.Ignore);
                            if (unk != IntPtr.Zero)
                            {
                                ppv = unk;
                                return CustomQueryInterfaceResult.Handled;
                            }
                        }
                    }
                    ppv = IntPtr.Zero;
                    return CustomQueryInterfaceResult.Failed;
                }

                ~Inner()
                {
                    // need to work out the reference counting for GC to work correctly
                    Debug.Print("Inner object finalized.");
                }

                public void Dispose()
                {
                    _outer = null;
                    _interfaces = null;
                }
            }
        }

        public const int OLECMDERR_E_UNKNOWNGROUP = -2147221244;

        /// <summary>
        /// From Microsoft.VisualStudio.OLE.Interop (Visual Studio 2010 SDK).
        /// </summary>
        public enum OLECMDID
        {
            /// <summary />
            OLECMDID_OPEN = 1,

            /// <summary />
            OLECMDID_NEW,

            /// <summary />
            OLECMDID_SAVE,

            /// <summary />
            OLECMDID_SAVEAS,

            /// <summary />
            OLECMDID_SAVECOPYAS,

            /// <summary />
            OLECMDID_PRINT,

            /// <summary />
            OLECMDID_PRINTPREVIEW,

            /// <summary />
            OLECMDID_PAGESETUP,

            /// <summary />
            OLECMDID_SPELL,

            /// <summary />
            OLECMDID_PROPERTIES,

            /// <summary />
            OLECMDID_CUT,

            /// <summary />
            OLECMDID_COPY,

            /// <summary />
            OLECMDID_PASTE,

            /// <summary />
            OLECMDID_PASTESPECIAL,

            /// <summary />
            OLECMDID_UNDO,

            /// <summary />
            OLECMDID_REDO,

            /// <summary />
            OLECMDID_SELECTALL,

            /// <summary />
            OLECMDID_CLEARSELECTION,

            /// <summary />
            OLECMDID_ZOOM,

            /// <summary />
            OLECMDID_GETZOOMRANGE,

            /// <summary />
            OLECMDID_UPDATECOMMANDS,

            /// <summary />
            OLECMDID_REFRESH,

            /// <summary />
            OLECMDID_STOP,

            /// <summary />
            OLECMDID_HIDETOOLBARS,

            /// <summary />
            OLECMDID_SETPROGRESSMAX,

            /// <summary />
            OLECMDID_SETPROGRESSPOS,

            /// <summary />
            OLECMDID_SETPROGRESSTEXT,

            /// <summary />
            OLECMDID_SETTITLE,

            /// <summary />
            OLECMDID_SETDOWNLOADSTATE,

            /// <summary />
            OLECMDID_STOPDOWNLOAD,

            /// <summary />
            OLECMDID_ONTOOLBARACTIVATED,

            /// <summary />
            OLECMDID_FIND,

            /// <summary />
            OLECMDID_DELETE,

            /// <summary />
            OLECMDID_HTTPEQUIV,

            /// <summary />
            OLECMDID_HTTPEQUIV_DONE,

            /// <summary />
            OLECMDID_ENABLE_INTERACTION,

            /// <summary />
            OLECMDID_ONUNLOAD,

            /// <summary />
            OLECMDID_PROPERTYBAG2,

            /// <summary />
            OLECMDID_PREREFRESH,

            /// <summary />
            OLECMDID_SHOWSCRIPTERROR,

            /// <summary />
            OLECMDID_SHOWMESSAGE,

            /// <summary />
            OLECMDID_SHOWFIND,

            /// <summary />
            OLECMDID_SHOWPAGESETUP,

            /// <summary />
            OLECMDID_SHOWPRINT,

            /// <summary />
            OLECMDID_CLOSE,

            /// <summary />
            OLECMDID_ALLOWUILESSSAVEAS,

            /// <summary />
            OLECMDID_DONTDOWNLOADCSS,

            /// <summary />
            OLECMDID_UPDATEPAGESTATUS,

            /// <summary />
            OLECMDID_PRINT2,

            /// <summary />
            OLECMDID_PRINTPREVIEW2,

            /// <summary />
            OLECMDID_SETPRINTTEMPLATE,

            /// <summary />
            OLECMDID_GETPRINTTEMPLATE
        }

        /// <summary>
        /// From Microsoft.VisualStudio.Shell (Visual Studio 2010 SDK).
        /// </summary>
        public const int S_OK = 0;

        /// <summary>
        /// OLE command structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class OLECMD
        {
            /// <summary>
            /// Command ID.
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public int cmdID;

            /// <summary>
            /// Flags associated with cmdID.
            /// </summary>
            [MarshalAs(UnmanagedType.U4)]
            public int cmdf;
        }

        /// <summary>
        /// Enables the dispatching of commands between objects and containers.
        /// </summary>
        [ComImport]
        [ComVisible(true), Guid("B722BCCB-4E68-101B-A2BC-00AA00404770"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IOleCommandTarget
        {
            /// <summary>Queries the object for the status of one or more commands generated by user interface events.</summary>
            /// <param name="pguidCmdGroup">The GUID of the command group.</param>
            /// <param name="cCmds">The number of commands in <paramref name="prgCmds" />.</param>
            /// <param name="prgCmds">An array of <see cref="T:Microsoft.VisualStudio.OLE.Interop.OLECMD" /> structures that indicate the commands for which the caller needs status information.</param>
            /// <param name="pCmdText">An <see cref="T:Microsoft.VisualStudio.OLE.Interop.OLECMDTEXT" /> structure in which to return name and/or status information of a single command. This parameter can be null to indicate that the caller does not need this information.</param>
            /// <returns>This method returns S_OK on success. Other possible return values include the following.
            /// E_FAIL The operation failed.
            /// E_UNEXPECTED An unexpected error has occurred.
            /// E_POINTER The <paramref name="prgCmds" /> argument is null.
            /// OLECMDERR_E_UNKNOWNGROUPThe <paramref name="pguidCmdGroup" /> parameter is not null but does not specify a recognized command group.</returns>
            [PreserveSig]
            [return: MarshalAs(UnmanagedType.I4)]
            int QueryStatus(ref Guid pguidCmdGroup, int cCmds, [In] [Out] OLECMD prgCmds,
                [In] [Out] IntPtr pCmdText);

            /// <summary>Executes the specified command.</summary>
            /// <param name="pguidCmdGroup">The GUID of the command group.</param>
            /// <param name="nCmdID">The command ID.</param>
            /// <param name="nCmdexecopt">Specifies how the object should execute the command. Possible values are taken from the <see cref="T:Microsoft.VisualStudio.OLE.Interop.OLECMDEXECOPT" /> and <see cref="T:Microsoft.VisualStudio.OLE.Interop.OLECMDID_WINDOWSTATE_FLAG" /> enumerations.</param>
            /// <param name="pvaIn">The input arguments of the command.</param>
            /// <param name="pvaOut">The output arguments of the command.</param>
            /// <returns>This method returns S_OK on success. Other possible return values include 
            /// OLECMDERR_E_UNKNOWNGROUP The <paramref name="pguidCmdGroup" /> parameter is not null but does not specify a recognized command group.
            /// OLECMDERR_E_NOTSUPPORTED The <paramref name="nCmdID" /> parameter is not a valid command in the group identified by <paramref name="pguidCmdGroup" />.
            /// OLECMDERR_E_DISABLED The command identified by <paramref name="nCmdID" /> is currently disabled and cannot be executed.
            /// OLECMDERR_E_NOHELP The caller has asked for help on the command identified by <paramref name="nCmdID" />, but no help is available.
            /// OLECMDERR_E_CANCELED The user canceled the execution of the command.</returns>
            [PreserveSig]
            [return: MarshalAs(UnmanagedType.I4)]
            int Exec(ref Guid pguidCmdGroup, int nCmdID, int nCmdexecopt,
                [MarshalAs(UnmanagedType.LPArray)] [In] object[] pvaIn, int pvaOut);
        }

        private readonly Dictionary<string, string> _errorsTable = new Dictionary<string, string>()
        {
            {"0x800C0009", "Authentication is needed to access the object."},
            {"0x800C0004", "The attempt to connect to the Internet has failed."},
            {"0x800C000B", "The Internet connection has timed out."},
            {"0x800C0007", "An Internet connection was established, but the data cannot be retrieved."},
            {"0x800C0019", "The SSL certificate is invalid."},
            {"0x800C000C", "The request was invalid."},
            {"0x800C0002", "The URL could not be parsed."},
            {"0x800C0005", "The server or proxy was not found."}
        };

        [SuppressUnmanagedCodeSecurity()]
        public static class UnsafeNativeMethods
        {
            // Used to control the webbrowser appearance and provide DTE to script via window.external
            [ComVisible(true), ComImport(), Guid("BD3F23C0-D43E-11CF-893B-00AA00BDCE1A"),
            InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
            public interface IDocHostUIHandler
            {

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int ShowContextMenu(
                    [In, MarshalAs(UnmanagedType.U4)]
                int dwID,
                    [In]
                ref POINT pt,
                    [In]
                IntPtr pcmdtReserved,
                    [In]
                IntPtr pdispReserved);

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int GetHostInfo(
                    [In, Out]
                ref DOCHOSTUIINFO info);

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int ShowUI(
                    [In, MarshalAs(UnmanagedType.I4)]
                int dwID,
                    [In]
                IntPtr activeObject,
                    [In]
                IntPtr commandTarget,
                    [In]
                IntPtr frame,
                    [In]
                IntPtr doc);

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int HideUI();

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int UpdateUI();

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int EnableModeless(
                    [In, MarshalAs(UnmanagedType.Bool)]
                bool fEnable);

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int OnDocWindowActivate(
                    [In, MarshalAs(UnmanagedType.Bool)]
                bool fActivate);

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int OnFrameWindowActivate(
                    [In, MarshalAs(UnmanagedType.Bool)]
                bool fActivate);

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int ResizeBorder(
                    [In]
                ref COMRECT rect,
                    [In]
                IntPtr doc,
                    bool fFrameWindow);

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int TranslateAccelerator(
                    [In]
                ref MSG msg,
                    [In]
                ref Guid group,
                    [In, MarshalAs(UnmanagedType.I4)]
                int nCmdID);

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int GetOptionKeyPath(
                    [Out, MarshalAs(UnmanagedType.LPArray)]
                String[] pbstrKey,
                    [In, MarshalAs(UnmanagedType.U4)]
                int dw);

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int GetDropTarget(
                    [In]
                IntPtr pDropTarget,
                    [Out]
                out IntPtr ppDropTarget);

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int GetExternal(
                    [Out, MarshalAs(UnmanagedType.IDispatch)]
                out object ppDispatch);

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int TranslateUrl(
                    [In, MarshalAs(UnmanagedType.U4)]
                int dwTranslate,
                    [In, MarshalAs(UnmanagedType.LPWStr)]
                string strURLIn,
                    [Out, MarshalAs(UnmanagedType.LPWStr)]
                out string pstrURLOut);

                [return: MarshalAs(UnmanagedType.I4)]
                [PreserveSig]
                int FilterDataObject(
                    IntPtr pDO,
                    out IntPtr ppDORet);
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct POINT
            {
                public int x;
                public int y;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct DOCHOSTUIINFO
            {
                [MarshalAs(UnmanagedType.U4)]
                public int cbSize;
                [MarshalAs(UnmanagedType.I4)]
                public int dwFlags;
                [MarshalAs(UnmanagedType.I4)]
                public int dwDoubleClick;
                [MarshalAs(UnmanagedType.I4)]
                public int dwReserved1;
                [MarshalAs(UnmanagedType.I4)]
                public int dwReserved2;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct COMRECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MSG
            {
                public IntPtr hwnd;
                public int message;
                public IntPtr wParam;
                public IntPtr lParam;
                public int time;
                POINT pt;
            }
        }
    }
}
