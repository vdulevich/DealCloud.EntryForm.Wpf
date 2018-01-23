using DealCloud.Common.Entities.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Entities
{
    public class ListItem : NamedEntry
    {
        /// <summary>
        /// if user can edit selection of this item
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// if this item is selected
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// true if current item is a group header
        /// </summary>
        public bool IsGroup { get; set; }

        /// <summary>
        /// for a choice fields, parent choice value id
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// first of the two fields which has IsUsedInLookups (or any other fields, for example can be used for tooltip or so)
        /// </summary>
        public string Field1Value { get; set; }
        /// <summary>
        /// second of the two fields which has IsUsedInLookups (or any other fields, for example can be used for tooltip or so)
        /// </summary>
        public string Field2Value { get; set; }

        public override NamedEntry Clone()
        {
            return new ListItem { EntryListId = this.EntryListId, Field1Value = this.Field1Value, Field2Value = this.Field2Value, Id = this.Id, IsChecked = this.IsChecked, IsEditable = this.IsEditable, IsGroup = this.IsGroup, Name = this.Name };
        }
    }

    /// <summary>
    /// used for DDL when we need to show selected value(s) without loading all stuff into dropdown
    /// </summary>
    public class SelectorInfo
    {
        /// <summary>
        /// number of the page for the first selected item
        /// for multiselect client may discard it and just show page 0
        /// </summary>
        public int FirstRecordIndex { get; set; }

        /// <summary>
        /// selected item text, for multiselect comma separated list of values
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// total number of records in the query without paging
        /// </summary>
        public int TotalRecords { get; set; }
    }

    /// <summary>
    /// filtering selector for drop down lists
    /// </summary>
    public class SelectorQuery
    {
        /// <summary>
        /// Paging for the DDL
        /// </summary>
        public PageInfo Paging { get; set; }
        /// <summary>
        /// string filter by Name property
        /// </summary>
        public string SearchTerm { get; set; }
        
        /// <summary>
        /// selector for values based on fieldIds
        /// </summary>
        public HashSet<int> FieldIds { get; set; }

        /// <summary>
        /// EntryLists by which we generate entries
        /// </summary>
        public HashSet<int> EntryListIds { get; set; }

        /// <summary>
        /// will filter results to only entries which are related to the supplied list
        /// </summary>
        public HashSet<int> ReferenceIds { get; set; }

        /// <summary>
        /// unique id of the control, by which we may need some custom loading of ListItems
        /// </summary>
        public int ControlId { get; set; }

        public bool? IncludeEmailForUsers { get; set; }

        /// <summary>
        /// control for the selection of items - all selected
        /// </summary>
        public bool AllSelected { get; set; }

        private HashSet<int> _selectedIds;

        /// <summary>
        /// user selected Ids in current session
        /// </summary>
        public HashSet<int> SelectedIds
        {
            get { return _selectedIds ?? (_selectedIds = new HashSet<int>()); }
            set { _selectedIds = value; }
        }

        private HashSet<int> _unselectedIds;

        /// <summary>
        /// user unselected Ids in current session
        /// </summary>
        public HashSet<int> UnselectedIds
        {
            get { return _unselectedIds ?? (_unselectedIds = new HashSet<int>()); }
            set { _unselectedIds = value; }
        }

        private HashSet<int> _originallySelectedIds;

        /// <summary>
        /// Ids which was selected before user stared manipulations
        /// </summary>
        public HashSet<int> OriginallySelectedIds
        {
            get { return _originallySelectedIds ?? (_originallySelectedIds = new HashSet<int>()); }
            set { _originallySelectedIds = value; }
        }
    }
}
