using DealCloud.Common.Entities;
using DealCloud.Common.Extensions;
using DealCloud.Common.Interfaces;
using Microsoft.VisualBasic.Devices;
using NLog;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace DealCloud.Common.Helpers
{
    public abstract class ReportService : IReportService
    {
        public abstract void ShowReport();

        public virtual ErrorReport Report(string message, Exception error = null)
        {
            ErrorReport report = new ErrorReport() { Message = message, ReferenceId = error != null ? error.GenerateReferenceId() : (int?)null, ReportData = GetReportData() };

            return report;
        }

        public virtual string GetSystemInfo()
        {
            string systemInfo = string.Empty;

            try
            {
                ComputerInfo info = new ComputerInfo();
                systemInfo += $"{info.OSFullName} {info.OSVersion} ({(Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit")}){Environment.NewLine}";
                systemInfo += $"{Environment.NewLine}";

                systemInfo += $"Version: {Environment.OSVersion}{Environment.NewLine}";
                systemInfo += $"Processors: {Environment.ProcessorCount}{Environment.NewLine}";
                systemInfo += $"CLR Version: {Environment.Version}{Environment.NewLine}";
                systemInfo += $"Machine Name: {Environment.MachineName}{Environment.NewLine}";
                systemInfo += $"System Page Size: {Environment.SystemPageSize}{Environment.NewLine}";
                systemInfo += $"System Directory: {Environment.SystemDirectory}{Environment.NewLine}";
                systemInfo += $"Current Directory: {Environment.CurrentDirectory}{Environment.NewLine}";
                systemInfo += $"User Name: {Environment.UserName}{Environment.NewLine}";
                systemInfo += $"Platform: {info.OSPlatform}{Environment.NewLine}";
                systemInfo += $"InstalledUICulture: {info.InstalledUICulture}{Environment.NewLine}";
                systemInfo += $"{Environment.NewLine}";

                systemInfo += $"Total Physical Memory: {info.TotalPhysicalMemory:N0}{Environment.NewLine}";
                systemInfo += $"Available Physical Memory: {info.AvailablePhysicalMemory:N0}{Environment.NewLine}";
                systemInfo += $"Total Virtual Memory: {info.TotalVirtualMemory:N0}{Environment.NewLine}";
                systemInfo += $"Available Virtual Memory: {info.AvailableVirtualMemory:N0}{Environment.NewLine}";
                systemInfo += $"{Environment.NewLine}";
            }
            catch (Exception ex)
            {
                systemInfo += $"{Environment.NewLine}Warning! Cannot completely gets the system info:{Environment.NewLine}{ex}{Environment.NewLine}";
            }

            return systemInfo;
        }

        public virtual List<string> GetLogFiles()
        {
            List<string> logFiles = new List<string>();

            if (LogManager.Configuration != null)
            {
                foreach (FileTarget target in LogManager.Configuration.AllTargets.OfType<FileTarget>())
                {
                    Layout layout = target.FileName;
                    var logFileFullName = layout.Render(LogEventInfo.CreateNullEvent());
                    var logFileName = Path.GetFileName(logFileFullName);
                    var logFilePath = Path.GetDirectoryName(logFileFullName);
                    var logArchFileFullNames = Directory.EnumerateFiles(logFilePath, $"{Path.GetFileNameWithoutExtension(logFileName)}.*");
                    logFiles.AddRange(logArchFileFullNames);
                }
            }

            return logFiles;
        }

        protected virtual byte[] GetReportData()
        {
            byte[] zipData;

            using (Stream zipStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    #region create report summary entry

                    ZipArchiveEntry reportFileZipEntry = zipArchive.CreateEntry(Constants.REPORT_FILE_NAME);
                    using (var logFileWriter = new StreamWriter(reportFileZipEntry.Open()))
                    {
                        logFileWriter.WriteLine($"The report created at {DateTime.UtcNow} by UTC");
                        logFileWriter.WriteLine(GetSystemInfo());
                    }

                    #endregion
                    #region create report logs entries

                    foreach (var logArchFullFileName in GetLogFiles())
                    {
                        var logArchFileName = Path.GetFileName(logArchFullFileName);
                        zipArchive.CreateEntryFromFile(logArchFullFileName, logArchFileName);
                    }

                    #endregion
                }

                zipData = new byte[zipStream.Length];
                zipStream.Seek(0, SeekOrigin.Begin);
                zipStream.Read(zipData, 0, (int)zipStream.Length);
            }

#if DEBUG
            using (Stream fileStream = new FileStream(Constants.REPORT_ZIP_FILE_NAME, FileMode.Create))
            {
                fileStream.Write(zipData, 0, zipData.Length);
            }
#endif

            return zipData;
        }
    }
}
