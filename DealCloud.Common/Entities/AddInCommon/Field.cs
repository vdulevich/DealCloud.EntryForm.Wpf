using System.Collections.Generic;
using DealCloud.Common.Enums;

namespace DealCloud.Common.Entities.AddInCommon
{
    public class Field: NamedEntry
    {
        public bool IsMoney { get; set; }

        public bool IsCalculated { get; set; }

        public bool IsEditable { get; set; }

        public bool IsMultiselect { get; set; }

        public SystemFieldTypes SystemFieldType { get; set; }

        public FieldTypes FieldType { get; set; }

        public HashSet<int> EntryLists { get; set; }

        public bool IsName => SystemFieldType == SystemFieldTypes.Name; 

        public bool IsSystemCreatedModified => SystemFieldType == SystemFieldTypes.Created ||
                                                SystemFieldType == SystemFieldTypes.CreatedBy ||
                                                SystemFieldType == SystemFieldTypes.Modified ||
                                                SystemFieldType == SystemFieldTypes.ModifiedBy;
    }
}
