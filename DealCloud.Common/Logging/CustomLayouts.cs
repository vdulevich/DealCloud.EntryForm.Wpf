using System;
using System.Text;
using DealCloud.Common.Interfaces;
using NLog;
using NLog.LayoutRenderers;

namespace DealCloud.Common.Logging
{
    [LayoutRenderer(Constants.LOG_USER_NAME)]
    public sealed class UserNameLayoutRender : LayoutRenderer
    {
        private static readonly Lazy<IUserContextService> LogInfo = new Lazy<IUserContextService>(IoContainer.Resolve<IUserContextService>);

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(LogInfo.Value.GetCurrentUserName());
        }
    }

    [LayoutRenderer(Constants.LOG_CLIENT_NAME)]
    public sealed class ClientNameLayoutRender : LayoutRenderer
    {
        private static readonly Lazy<IUserContextService> LogInfo = new Lazy<IUserContextService>(IoContainer.Resolve<IUserContextService>);

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(LogInfo.Value.GetCurrentClientName(true));
        }
    }

    [LayoutRenderer(Constants.LOG_USER_ID)]
    public sealed class UserIdLayoutRender : LayoutRenderer
    {
        private static readonly Lazy<IUserContextService> LogInfo = new Lazy<IUserContextService>(IoContainer.Resolve<IUserContextService>);

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(LogInfo.Value.GetCurrentUserId());
        }
    }

    [LayoutRenderer(Constants.LOG_CLIENT_ID)]
    public sealed class ClientIdLayoutRender : LayoutRenderer
    {
        private static readonly Lazy<IUserContextService> LogInfo = new Lazy<IUserContextService>(IoContainer.Resolve<IUserContextService>);

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(LogInfo.Value.GetCurrentClientId());
        }
    }

    [LayoutRenderer(Constants.LOG_STATISTIC_TYPE)]
    public sealed class StatisticTypeLayoutRender : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            object value;

            if (logEvent.Properties.TryGetValue(Constants.LOG_PROPERTY_STATISTIC_TYPE, out value))
            {
                if (value != null) builder.Append(value);
            }
        }
    }

    [LayoutRenderer(Constants.LOG_PARAMETERS)]
    public sealed class PageUrlLayoutRender : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            object value;

            if (logEvent.Properties.TryGetValue(Constants.LOG_PROPERTY_PARAMETERS, out value))
            {
                if (value != null) builder.Append(value);
            }
        }
    }

    [LayoutRenderer(Constants.LOG_REFERENCE_ID)]
    public sealed class ReferenceIdLayoutRender : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            if (logEvent.Exception != null && logEvent.Exception.Data.Contains(Constants.ERROR_PARAMETER_REFERENCEID))
            {
                var value = logEvent.Exception.Data[Constants.ERROR_PARAMETER_REFERENCEID];

                if (value != null) builder.Append(value);
            }
        }
    }
}