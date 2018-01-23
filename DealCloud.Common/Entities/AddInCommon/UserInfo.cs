using DealCloud.Common.Enums;

namespace DealCloud.Common.Entities.AddInCommon
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string ClientName { get; set; }

        public int ClientId { get; set; }

        public string HelpUrl { get; set; }

        public Capabilities[] Capabilities { get; set; }

        public string GetIdentityString()
        {
            return $"{ClientId}_{Id}";
        }
    }
}
