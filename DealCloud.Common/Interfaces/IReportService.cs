using DealCloud.Common.Entities;
using System;
using System.Collections.Generic;

namespace DealCloud.Common.Interfaces
{
    public interface IReportService
    {
        /// <summary>
        ///     Shows the report user interface
        /// </summary>
        void ShowReport();

        /// <summary>
        ///     Reports issues that user having with any of the DealCloud software,
        ///     describes the issue,
        ///     sends the DealCloud logs and system info
        /// </summary>
        ErrorReport Report(string message, Exception error = null);

        /// <summary>
        ///     Gets the DealCloud system info
        /// </summary>
        string GetSystemInfo();

        /// <summary>
        ///     Gets the DealCloud log files
        /// </summary>
        List<string> GetLogFiles();
    }
}
