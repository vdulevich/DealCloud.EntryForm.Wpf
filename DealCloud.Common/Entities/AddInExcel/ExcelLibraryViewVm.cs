using System;

namespace DealCloud.Common.Entities.AddInExcel
{
    public class ExcelLibraryViewVm: NamedEntry
    {
        public string Description { get; set; }

        public bool IsPublic { get; set; }

        public DateTime Modified { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public ExcelLibraryViewVm()
        {
        }

        public ExcelLibraryViewVm(LibraryView libraryView, Func<DateTime, DateTime> convertToUtc = null)
        {
            Id = libraryView.Id;
            Name = libraryView.Name;
            IsPublic = libraryView.IsPublic;
            Description = libraryView.Description;
            CreatedBy = libraryView.CreatedBy;
            Created = convertToUtc?.Invoke(libraryView.Created) ?? libraryView.Created;
            ModifiedBy = libraryView.ModifiedBy;
            Modified = convertToUtc?.Invoke(libraryView.Modified) ?? libraryView.Modified;
        }
    }
}
