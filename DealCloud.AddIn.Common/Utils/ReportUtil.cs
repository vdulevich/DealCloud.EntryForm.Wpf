using DealCloud.AddIn.Common.Controls;
using DealCloud.Common;
using DealCloud.Common.Entities;
using DealCloud.Common.Extensions;
using DealCloud.Common.Helpers;
using DealCloud.Common.Interfaces;
using NLog;
using System;

namespace DealCloud.AddIn.Common.Utils
{
    public class ReportUtil : ReportService
    {
        public static IReportService Instance
        {
            get
            {
                return IoContainer.Resolve<IReportService>();
            }
        }

        public override void ShowReport()
        {
            using (var reportDialog = CreateReportDialog())
            {
                reportDialog.ShowHandledDialog();
            }
        }

        public override ErrorReport Report(string message, Exception error = null)
        {
            ErrorReport report = base.Report(message, error);

            try
            {
                int result = DataProviderBase.GetInstance().ReportError(report).Result;
            }
            catch (Exception ex)
            {
                ex.GenerateReferenceId();
                LogManager.GetCurrentClassLogger().Error(ex);

                using (var errorDialog = ErrorMessageBaseUtil.Instance.CreateMessageErrorDialog())
                {
                    var info = ErrorMessageBaseUtil.Instance.GetErrorMessage(ex);
                    errorDialog.Message = info.Message;
                    errorDialog.MessageDetails = info.Details;
                    var result = errorDialog.ShowHandledDialog();
                }
            }

            return report;
        }

        public override string GetSystemInfo()
        {
            string systemInfo = base.GetSystemInfo();

            try
            {
                var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                systemInfo += $"{currentProcess.MainModule.FileVersionInfo.ProductName} {currentProcess.MainModule.FileVersionInfo.InternalName} {currentProcess.MainModule.FileVersionInfo.ProductVersion} ({(Environment.Is64BitProcess ? "64-bit" : "32-bit")}){Environment.NewLine}";
                systemInfo += $"{Environment.NewLine}";
            }
            catch (Exception ex)
            {
                systemInfo += $"{Environment.NewLine}Warning! Cannot completely gets the system info:{Environment.NewLine}{ex}{Environment.NewLine}";
            }

            return systemInfo;
        }

        protected virtual FormExt CreateReportDialog()
        {
            return new ReportDialog();
        }
    }
}
