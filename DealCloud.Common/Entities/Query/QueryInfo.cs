using System.Collections.Generic;
using System.Linq;
using DealCloud.Common.Extensions;
using Newtonsoft.Json;

namespace DealCloud.Common.Entities.Query
{
    /// <summary>
    /// Query for the info on the portal
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class QueryInfo : NamedEntry // query can be named and have Id ( for templates and add-in )
    {
        /// <summary>
        /// Determine whether to show count in titles or not
        /// </summary>
        public bool IncludeRecordsCount { get; set; }

        /// <summary>
        /// Indicates whether multi level sorting is enabled. For now this is only possible to configure in add-ins
        /// </summary>
        public bool IsMultisortingEnabled { get; set; }


        /// <summary>
        /// Lists by which we are going to iterate ( create rows )
        /// </summary>
        public List<int> IterationEntryListIds { get; set; }

        /// <summary>
        /// Fields by which we are filtering relationships between entries:   Contacts -> Deals -> Companies
        /// building grid by Deals, but relationship for Contacts we using from Contacts and from Deals to Companies
        /// </summary>
        public HashSet<int> RelationshipFields { get; set; }

        /// <summary>
        /// for Entry Details we may need to filter by EntryId, this are the fields we are filtering on By Id
        /// this will be taken into account only if entryId is supplied. UI will need to draw it and a filter but store just a relationship fields id
        /// this relationships will from from the EntryList of entryId to IterationEntryListIds or backwards.
        /// </summary>
        public HashSet<int> EntryDetailsRelationshipFields { get; set; }

        /// <summary>
        /// main filters for a grid/query
        /// 1. not allowed to have different operators inside group
        /// 2. not allowed to have different operators between groups
        /// everything else will work
        /// examples:
        /// (A || Name) &amp; C &amp; D - ok
        /// (A || B) || (C &amp; D) || (E &amp; F &amp; Z) - ok
        /// (A || B) || (C &amp; D) &amp; F - not ok(diff operators between groups)
        /// (A || Name &amp; F) &amp; C &amp; D - not ok(diff operators inside group)
        /// </summary>
        public List<FilterTerm> PrimaryFilters { get; set; }

        /// <summary>
        /// secondary filters on the grid/query
        /// Always AND between this filters and primary filters
        /// </summary>
        public List<FilterInfo> SecondaryFilters { get; set; }

        /// <summary>
        /// Grouping by Columns
        /// </summary>
        public GroupInfo Grouping { get; set; }

        /// <summary>
        /// sorting by columns
        /// </summary>
        public SortInfo Sorting { get; set; }

        /// <summary>
        /// paging if any
        /// </summary>
        public PageInfo Paging { get; set; }

        /// <summary>
        /// Columns information ( map to fields )
        /// </summary>
        public ColumnMetadata Columns { get; set; }

        public List<int> FlatteningEntryListIds { get; set; }

        public bool IsSecondaryColumnsFilterEnabled { get; set; }

        public QueryInfo ShallowClone()
        {
            return new QueryInfo
            {
                Id = Id,
                Name = Name,
                Columns = this.Columns?.Clone(),
                PrimaryFilters = this.PrimaryFilters,
                SecondaryFilters = this.SecondaryFilters,
                Grouping = this.Grouping,
                IterationEntryListIds = this.IterationEntryListIds,
                FlatteningEntryListIds = this.FlatteningEntryListIds,
                RelationshipFields = this.RelationshipFields,
                EntryDetailsRelationshipFields = this.EntryDetailsRelationshipFields,
                Paging = this.Paging,
                Sorting = this.Sorting,
                IncludeRecordsCount = this.IncludeRecordsCount,
                IsMultisortingEnabled = this.IsMultisortingEnabled,
                IsSecondaryColumnsFilterEnabled = this.IsSecondaryColumnsFilterEnabled
            };
        }

        public override int GetHashCode()
        {
            var result = 37;

            unchecked
            {
                result = result * 17 + base.GetHashCode();
                result = result * 17 + (IterationEntryListIds?.UncheckedSum() ?? 0);
                result = result * 17 + (FlatteningEntryListIds?.UncheckedSum() ?? 0);
                result = result * 17 + (RelationshipFields?.UncheckedSum() ?? 0);
                result = result * 17 + (EntryDetailsRelationshipFields?.UncheckedSum() ?? 0);
                result = result * 17 + (PrimaryFilters?.Select(f => f.GetHashCode()).UncheckedSum() ?? 0);
                result = result * 17 + (SecondaryFilters?.Select(f => f.GetHashCode()).UncheckedSum() ?? 0);
                result = result * 17 + (Grouping?.GetHashCode() ?? 0);
                result = result * 17 + (Sorting?.GetHashCode() ?? 0);
                result = result * 17 + (Paging?.GetHashCode() ?? 0);
                result = result * 17 + (Columns?.GetHashCode() ?? 0);
                result = result * 17 + IncludeRecordsCount.GetHashCode();
                result = result * 17 + IsSecondaryColumnsFilterEnabled.GetHashCode();
                result = result * 17 + IsMultisortingEnabled.GetHashCode();
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            var result = base.Equals(obj);

            if (result)
            {
                var query = obj as QueryInfo;

                if (query != null)
                {
                    result = (IterationEntryListIds == null && query.IterationEntryListIds == null) ||
                             (IterationEntryListIds != null && query.IterationEntryListIds != null && IterationEntryListIds.OrderBy(id => id).SequenceEqual(query.IterationEntryListIds.OrderBy(id => id)));

                    result = (FlatteningEntryListIds == null && query.FlatteningEntryListIds == null) ||
                             (FlatteningEntryListIds != null && query.FlatteningEntryListIds != null && FlatteningEntryListIds.OrderBy(id => id).SequenceEqual(query.FlatteningEntryListIds.OrderBy(id => id)));

                    result = result &&
                           ((RelationshipFields == null && query.RelationshipFields == null) ||
                            (RelationshipFields != null && query.RelationshipFields != null && RelationshipFields.OrderBy(id => id).SequenceEqual(query.RelationshipFields.OrderBy(id => id))));

                    result = result &&
                           ((EntryDetailsRelationshipFields == null && query.EntryDetailsRelationshipFields == null) ||
                            (EntryDetailsRelationshipFields != null && query.EntryDetailsRelationshipFields != null && EntryDetailsRelationshipFields.OrderBy(id => id).SequenceEqual(query.EntryDetailsRelationshipFields.OrderBy(id => id))));

                    result = result &&
                           ((PrimaryFilters == null && query.PrimaryFilters == null) ||
                            (PrimaryFilters != null && query.PrimaryFilters != null && PrimaryFilters.SequenceEqual(query.PrimaryFilters))); // NOTE: need to be same order

                    result = result &&
                           ((SecondaryFilters == null && query.SecondaryFilters == null) ||
                            (SecondaryFilters != null && query.SecondaryFilters != null && SecondaryFilters.OrderBy(f => f.ColumnId).SequenceEqual(query.SecondaryFilters.OrderBy(f => f.ColumnId))));

                    result = result &&
                           ((Grouping == null && query.Grouping == null) ||
                            (Grouping != null && query.Grouping != null && Grouping.Equals(query.Grouping)));

                    result = result &&
                           ((Sorting == null && query.Sorting == null) ||
                            (Sorting != null && query.Sorting != null && Sorting.Equals(query.Sorting)));

                    result = result &&
                           ((Paging == null && query.Paging == null) ||
                            (Paging != null && query.Paging != null && Paging.Equals(query.Paging)));

                    result = result &&
                           ((Columns == null && query.Columns == null) ||
                            (Columns != null && query.Columns != null && Columns.Equals(query.Columns)));

                    result = result &&
                             IncludeRecordsCount == query.IncludeRecordsCount;

                    result = result &&
                             IsMultisortingEnabled == query.IsMultisortingEnabled;

                    result = result &&
                             IsSecondaryColumnsFilterEnabled == query.IsSecondaryColumnsFilterEnabled;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
