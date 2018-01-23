namespace DealCloud.Common.Entities.IncomingEmail
{
    public class ContactEntry : NamedEntry
    {
        public string EmailAddress { get; set; }

        public override int GetHashCode()
        {
            var result = 37;

            unchecked
            {
                result = result * 17 + base.GetHashCode();
                result = result * 17 + (EmailAddress?.ToLower().GetHashCode() ?? 0);
            }

            return result;
        }
    }
}
