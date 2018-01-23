using System;
using System.Runtime.InteropServices;
using System.Text;
using NLog;

namespace DealCloud.Common.Logging
{
    /// <summary>
    ///     Log when this object was created and disposed. Use for logging method call, or sections of code calls
    ///     using (new LogCall(log, "BlockName", param1, param2))
    ///     {
    ///          ...your code
    ///     }
    /// </summary>
    public class LogCall : IDisposable
    {
        private readonly Logger _log;
        private readonly string _methodName;

        /// <summary>
        ///     Constructs a logger wrapper, which can be used to log method enter and exit. Do not pass COM objects to here as part of args
        /// </summary>
        public LogCall(Logger log, string methodName, params object[] args)
        {
            _methodName = methodName;
            _log = log;

            if (_log.IsDebugEnabled)
            {
                var sb = new StringBuilder(9 + args.Length*5);

                sb.Append(_methodName).Append(" - Start");

                switch (args.Length)
                {
                    case 1:
                        _log.Debug(sb.Append(", {0}").ToString(), args[0]);
                        break;
                    case 2:
                        _log.Debug(sb.Append(", {0}, {1}").ToString(), args[0], args[1]);
                        break;
                    case 3:
                        _log.Debug(sb.Append(", {0}, {1}, {2}").ToString(), args[0], args[1], args[2]);
                        break;
                    case 4:
                        _log.Debug(sb.Append(", {0}, {1}, {2}, {3}").ToString(), args[0], args[1], args[2], args[3]);
                        break;
                    case 5:
                        _log.Debug(sb.Append(", {0}, {1}, {2}, {3}, {4}").ToString(), args[0], args[1], args[2], args[3], args[4]);
                        break;
                    case 6:
                        _log.Debug(sb.Append(", {0}, {1}, {2}, {3}, {4}, {5}").ToString(), args[0], args[1], args[2], args[3], args[4], args[5]);
                        break;
                    case 7:
                        _log.Debug(sb.Append(", {0}, {1}, {2}, {3}, {4}, {5}, {6}").ToString(), args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                        break;
                    case 8:
                        _log.Debug(sb.Append(", {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}").ToString(), args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                        break;
                    case 9:
                        _log.Debug(sb.Append(", {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}").ToString(), args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                        break;
                    default:
                        _log.Debug(sb.ToString());
                        break;
                }
            }
        }

        /// <summary>
        ///     Logs exit of the method
        /// </summary>
        public void Dispose()
        {
            //NOTE: Marshal.GetExceptionCode() helps to determine if there was an exception and not write End method in this case, please see http://ayende.com/blog/2577/did-you-know-find-out-if-an-exception-was-thrown-from-a-finally-block
            if (_log.IsDebugEnabled && (Marshal.GetExceptionCode() == 0))
            {
                _log.Debug(_methodName + " - End");
            }
        }
    }
}