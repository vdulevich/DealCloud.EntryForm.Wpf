using System.Collections.Generic;

namespace DealCloud.Common.Entities.AddInCommon
{
    public static class OutlookSyncHelper
    {
        public static readonly List<SystemFieldTypes> ContactFields = new List<SystemFieldTypes>
        {
            SystemFieldTypes.Email,
            SystemFieldTypes.FirstName,
            SystemFieldTypes.LastName,
            SystemFieldTypes.JobTitle,
            SystemFieldTypes.MobilePhone,
            SystemFieldTypes.DirectOffice,
            SystemFieldTypes.Address,
            SystemFieldTypes.City,
            SystemFieldTypes.State,
            SystemFieldTypes.PostalCode,
            SystemFieldTypes.Country,
            SystemFieldTypes.Notes,
            SystemFieldTypes.Company
        };

        public static readonly List<SystemFieldTypes> EventFields = new List<SystemFieldTypes>
        {
            SystemFieldTypes.Name,
            SystemFieldTypes.Location,
            SystemFieldTypes.StartTime, // NOTE: DateTime? in UTC
            SystemFieldTypes.EndTime, // NOTE: DateTime? in UTC
            SystemFieldTypes.Body,
            SystemFieldTypes.IsAllDayEvent, // NOTE: bool
            SystemFieldTypes.RecipientEmailAddress // NOTE: List<string> item.Trim().ToLowerInvariant()
        };
    }
}