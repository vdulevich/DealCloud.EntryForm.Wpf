using DealCloud.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using DealCloud.Common.Extensions;
using DealCloud.Common.Serialization;
using Newtonsoft.Json;

namespace DealCloud.Common.Entities
{
    /// <summary>
    /// Columns in a generic ResultSet
    /// Key is number of a column
    /// Value Info about a column
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    [JsonConverter(typeof(DictionaryJsonConverter))]
    public class ColumnMetadata : Dictionary<int, ColumnInfo>
    {
        public List<NamedEntry> ColumnGroups { get; set; }

        public ColumnMetadata()
        {
        }

        public ColumnMetadata(IDictionary<int, ColumnInfo> source) : base(source)
        {
        }

        public override int GetHashCode()
        {
            var result = 17;

            unchecked
            {
                result = result * 37 + Keys.UncheckedSum();
                result = result * 37 + Values.Select(v => v.GetHashCode()).UncheckedSum();
                if (ColumnGroups != null)
                {
                    result = result * 37 + ColumnGroups.Select(v => v.GetHashCode()).UncheckedSum();

                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            var dict = obj as ColumnMetadata;

            return dict != null && Keys.OrderBy(id => id).SequenceEqual(dict.Keys.OrderBy(id => id)) &&
                   Values.OrderBy(v => v.SeqNumber).SequenceEqual(dict.Values.OrderBy(v => v.SeqNumber)) &&
                   (ColumnGroups ?? Enumerable.Empty<NamedEntry>()).SequenceEqual((dict.ColumnGroups ?? Enumerable.Empty<NamedEntry>()));
        }

        public ColumnMetadata Clone()
        {
            var ret = new ColumnMetadata();
            foreach (var kvp in this)
            {
                ret[kvp.Key] = kvp.Value.Clone() as ColumnInfo;
            }
            ret.ColumnGroups = this.ColumnGroups?.Select(x => x.Clone()).ToList();
            return ret;
        }
    }

    /// <summary>
    /// informarion about column in a generic resultset
    /// Id - FieldId
    /// Name - Field Name
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ColumnInfo : NamedEntry
    {
        public ColumnInfo()
        {
            IsVisible = true;
        }

        public int SeqNumber { get; set; }

        /// <summary>
        /// Field(s) for the columns and associated relationship field it was get from.
        /// for example Deal -> PrimaryContact.Name (Contacts)
        /// field Contacts.Name was obtained via Relationhsip field PrimaryContact on Deal entry
        /// </summary>
        public HashSet<FieldViaRelationship> ColumnFields { get; set; }

        [JsonIgnore]
        public IEnumerable<int> FieldIds { get { return ColumnFields?.Select(cf => cf.FieldId); } }

        public FieldTypes FieldType { get; set; }

        public int FormatTypeId { get; set; }

        public string CurrencyCode { get; set; }

        public int? ColumnGroupId { get; set; }

        /// <summary>
        /// has subtotals for groups
        /// </summary>
        public bool HasSubtotals { get; set; }

        /// <summary>
        /// true if column has grand totals for this column
        /// </summary>
        public bool HasTotals { get; set; }

        /// <summary>
        /// if totals or subtotals on for this column what function we will apply
        /// </summary>
        public ColumnAggregation TotalsAggregation { get; set; }

        /// <summary>
        /// indicates if this column should be visible in the grid/table
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// if we can sort by this field - we can't sort on multiselect fields
        /// this is runtime property and don't need to be in equals or hashcode
        /// </summary>
        public bool IsSortable { get; set; }

        /// <summary>
        /// true if we can group by this field - we can't group by multiselect fields
        /// this is runtime property and don't need to be in equals or hashcode
        /// </summary>
        public bool IsGroupable { get; set; }

        /// <summary>
        /// width of the column
        /// </summary>
        public int Width { get; set; } 

        public override int GetHashCode()
        {
            var result = 37;

            unchecked
            {
                result = result * 17 + base.GetHashCode();
                result = result * 17 + SeqNumber;
                result = result * 17 + (ColumnFields?.Select(f => f.GetHashCode()).UncheckedSum() ?? 0);
                result = result * 17 + (int)FieldType;
                result = result * 17 + FormatTypeId;
                result = result * 17 + (CurrencyCode?.GetHashCode() ?? 0);
                result = result * 17 + HasSubtotals.GetHashCode();
                result = result * 17 + HasTotals.GetHashCode();
                result = result * 17 + (int)TotalsAggregation;
                result = result * 17 + IsVisible.GetHashCode();
                if (ColumnGroupId != null) result = result * 17 + ColumnGroupId.Value;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            var result = base.Equals(obj);

            if (result)
            {
                var column = obj as ColumnInfo;

                if (column != null)
                {
                    result = SeqNumber == column.SeqNumber && FieldType == column.FieldType &&
                             FormatTypeId == column.FormatTypeId && string.Equals(CurrencyCode, column.CurrencyCode) &&
                             HasSubtotals == column.HasSubtotals && HasTotals == column.HasTotals &&
                             TotalsAggregation == column.TotalsAggregation && ColumnGroupId == column.ColumnGroupId && IsVisible == column.IsVisible;

                    result = result &&
                           ((ColumnFields == null && column.ColumnFields == null) ||
                            (ColumnFields != null && column.ColumnFields != null && ColumnFields.OrderBy(f => f.FieldId).SequenceEqual(column.ColumnFields.OrderBy(f => f.FieldId))));
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public override NamedEntry Clone()
        {
            return new ColumnInfo
            {
                ColumnFields = this.ColumnFields?.Select(x => new FieldViaRelationship { FieldId = x.FieldId, RelationshipFieldId = x.RelationshipFieldId }).ToHashSet(),
                ColumnGroupId =  this.ColumnGroupId,
                CurrencyCode = this.CurrencyCode, 
                EntryListId = this.EntryListId,
                FieldType = this.FieldType,
                FormatTypeId = this.FormatTypeId,
                HasSubtotals = this.HasSubtotals,
                HasTotals = this.HasTotals,
                Id = this.Id,
                IsVisible = this.IsVisible,
                Name = this.Name,
                SeqNumber = this.SeqNumber,
                TotalsAggregation = this.TotalsAggregation,
                Width = this.Width
            };
        }
    }

    public class FieldViaRelationship
    {
        public int FieldId { get; set; }

        public int RelationshipFieldId { get; set; }

        public override int GetHashCode()
        {
            var result = 17;

            unchecked
            {
                result = result * 37 + FieldId;
                result = result * 37 + RelationshipFieldId;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            var f = obj as FieldViaRelationship;

            return f != null && f.FieldId == FieldId && f.RelationshipFieldId == RelationshipFieldId;
        }
    }


}
