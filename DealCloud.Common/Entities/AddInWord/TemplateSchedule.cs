using System.Collections.Generic;

namespace DealCloud.Common.Entities.TemplateReports
{
    public class TemplateSchedule
    {
        public int Id { get; set; }

        public Dictionary<int, Frequency> Frequency { get; set; }

        public int EmailTemplateId { get; set; }

        public HashSet<int> Recipients { get; set; }

        public HashSet<int> UserGroups { get; set; }

        public HashSet<int> ExcludedRecipients { get; set; }

        public override string ToString()
        {
            return $"Frequencies count = {Frequency?.Count ?? 0}";
        }
    }
}