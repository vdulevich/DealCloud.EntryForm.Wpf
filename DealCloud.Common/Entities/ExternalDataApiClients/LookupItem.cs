namespace DealCloud.Common.Entities.ExternalDataApiClients
{
    public class LookupItem
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public override string ToString()
        {
            return $"{Id}: {Name}";
        }
    }
}