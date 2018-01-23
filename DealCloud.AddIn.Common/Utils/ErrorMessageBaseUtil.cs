using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Controls;
using DealCloud.AddIn.Common.Properties;
using NLog;
using DealCloud.Common;
using DealCloud.Common.Entities;
using DealCloud.Common.Extensions;

namespace DealCloud.AddIn.Common.Utils
{
    public class ErrorMessage
    {
        public string Message { get; set; }

        public string Details { get; set; }
    }

    public abstract class ErrorMessageBaseUtil
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static ErrorMessageBaseUtil Instance
        {
            get
            {
                return IoContainer.Resolve<ErrorMessageBaseUtil>();
            }
        }

        public static bool IsCancelation(Exception e)
        {
            var error = ExtractFromAggregateException(e);
            return error is TaskCanceledException || error is OperationCanceledException;
        }

        public static Exception ExtractFromAggregateException(Exception e)
        {
            while (e is AggregateException && e.InnerException != null)
            {
                e = e.InnerException;
            }
            return e;
        }

        public virtual ErrorMessage GetErrorMessage(Exception e)
        {
            Exception error = ExtractFromAggregateException(e);
            ErrorMessage result = new ErrorMessage();
            if (error is HttpClientException)
            {
                HttpClientException clientException = error as HttpClientException;
                ErrorInfo errorInfo = clientException.ErrorInfo;
                if (errorInfo?.Code == Constants.ADDIN_VERSION_LOGIN)
                {
                    result.Message = Resources.Addin_Login_Vesrion_Error;
                }
                else if (errorInfo?.Code == Constants.ADDIN_VERSION_LOGOUT)
                {
                    result.Message = Resources.Addin_Logout_Version_Error;
                }
                else
                {
                    result.Message = $"{GetErrorMessage(clientException.StatusCode)}";
                }
                Log.Debug($"HttpClientException reasonPhrase: {clientException.ReasonPhrase}");
            }
            else if (error is WebException)
            {
                result.Message = GetErrorMessage(error as WebException);
            }
            else if (error is UnauthorizedAccessException)
            {
                result.Message = "You don't have access permission to this item.";
            }
            else
            {
                result.Message = error.Message;
            }
            result.Details = error.ToString();
            return result;
        }

        public static string GetErrorMessage(WebException exception)
        {
            switch (exception.Status)
            {
                case WebExceptionStatus.NameResolutionFailure:
                    return $"Name resolution failure. Cann't found server by url {exception.Response?.ResponseUri}, please check network or wi-fi connection";
                default:
                    return $"Network error. Error status code '{exception.Status}'";
            }
        }

        public static string GetErrorMessage(HttpStatusCode code)
        {
            switch (code)
            {
                case HttpStatusCode.ServiceUnavailable:
                    return "The server is currently unable to handle the request due to a temporary overloading or maintenance of the server";
                case HttpStatusCode.BadRequest:
                    return "The request could not be understood by the server due to malformed syntax.";
                case HttpStatusCode.Unauthorized:
                    return "Your session expired. Please re-login to continue.";
                case HttpStatusCode.Forbidden:
                    return "Request forbidden by the server, please try again later.";
                case HttpStatusCode.NotFound:
                    return "The server has not found anything matching the request.";
                default:
                    return $"Server returned an error '{code}'.";
            }
        }

        public virtual void ProcessError(Exception error, bool showError = true)
        {
            Exception innerError = ExtractFromAggregateException(error);
            if (IsCancelation(innerError) || innerError == null)
            {
                return;
            }

            innerError.GenerateReferenceId();

            Log.Debug(GetErrorMessage(innerError).Message);
            Log.Error(innerError);
            if (innerError is HttpClientException)
            {
                HttpClientException httpException = innerError as HttpClientException;
                ErrorInfo errorInfo = httpException.ErrorInfo;
                if (httpException.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (!IsLoogedIn) return;
                    LogoutAction();
                    if (!showError) return;
                    using (MessageDialog dlgResult = CreateMessageDialog())
                    {
                        dlgResult.Title = Resources.Session_Expired;
                        dlgResult.Messsage = GetErrorMessage(httpException).Message;
                        dlgResult.OkText = "Login";
                        dlgResult.AllowReporting = false;
                        if (dlgResult.ShowDialog() == DialogResult.OK)
                        {
                            LoginAction();
                        }
                    }
                    return;
                }
                if (httpException.StatusCode == HttpStatusCode.Forbidden)
                {
                    if (errorInfo?.Code == Constants.ADDIN_VERSION_LOGIN ||
                        errorInfo?.Code == Constants.ADDIN_VERSION_LOGOUT)
                    {
                        if (!IsLoogedIn) return;
                        LogoutAction();
                    }
                    if (!showError) return;
                    using (MessageDialog dialog = CreateMessageDialog())
                    {
                        dialog.Title = httpException.StatusCode.ToString();
                        dialog.Messsage = GetErrorMessage(httpException).Message;
                        dialog.ShowCancel = false;
                        dialog.OkText = Resources.Ok;
                        dialog.AllowReporting = true;
                        dialog.ShowHandledDialog();
                    }
                    return;
                }
                if (httpException.StatusCode == HttpStatusCode.BadRequest)
                {
                    if (!showError) return;
                    using (MessageDialog dialog = CreateMessageDialog())
                    {
                        dialog.Title = httpException.StatusCode.ToString();
                        dialog.Messsage = GetErrorMessage(httpException).Message;
                        dialog.ShowCancel = false;
                        dialog.OkText = Resources.Ok;
                        dialog.AllowReporting = true;
                        dialog.ShowHandledDialog();
                    }
                    return;
                }
                if (httpException.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    if (!showError) return;
                    using (MessageDialog dialog = CreateMessageDialog())
                    {
                        dialog.Title = httpException.StatusCode.ToString();
                        dialog.Messsage = GetErrorMessage(httpException).Message;
                        dialog.ShowCancel = false;
                        dialog.OkText = Resources.Ok;
                        dialog.AllowReporting = true;
                        dialog.ShowHandledDialog();
                    }
                    return;
                }
            }

            if (innerError is HttpRequestException)
            {
                if (innerError.InnerException is WebException)
                {
                    if (!showError) return;
                    using (MessageDialog dialog = CreateMessageDialog())
                    {
                        dialog.Title = Resources.Network_Error;
                        dialog.Messsage = GetErrorMessage((WebException)innerError.InnerException);
                        dialog.ShowCancel = false;
                        dialog.OkText = Resources.Ok;
                        dialog.AllowReporting = true;
                        dialog.ShowHandledDialog();
                    }
                    return;
                }
            }

            if (innerError is IeVersionException)
            {
                if (!showError) return;
                using (MessageDialog dialog = CreateMessageDialog())
                {
                    dialog.Title = "Error";
                    dialog.Messsage = Resources.To_use_addin_please_install_IE_with_version_10_or_upper;
                    dialog.ShowCancel = false;
                    dialog.OkText = Resources.Ok;
                    dialog.AllowReporting = true;
                    dialog.ShowHandledDialog();
                }
                return;
            }

            if (!showError) return;
            using (MessageErrorDialog dlg = CreateMessageErrorDialog())
            {
                var info = GetErrorMessage(error);
                dlg.Message = info.Message;
                dlg.MessageDetails = info.Details;
                dlg.AllowReporting = true;
                dlg.ShowDialog();
            }
        }

        public abstract bool IsLoogedIn { get; }

        public abstract void LoginAction();

        public abstract void LogoutAction();

        public abstract MessageErrorDialog CreateMessageErrorDialog();

        public abstract MessageDialog CreateMessageDialog();
    }

    public class IeVersionException : Exception
    {

    }
}
