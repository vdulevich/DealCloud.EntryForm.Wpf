namespace DealCloud.Common.Entities.AddInOutlook
{
    public class SyncItemError
    {
        public string Description { get; set; }

        public SyncItemError(string description = null)
        {
            Description = description;
        }

        public override string ToString()
        {
            return Description ?? base.ToString();
        }
    }
}