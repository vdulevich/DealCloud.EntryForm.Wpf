using System.Collections.Generic;

namespace DealCloud.Common.Entities
{
    public class Wildcard
    {
        public string Caption { get; set; }

        public string Value { get; set; }

        public bool IsLink { get; set; }

        public WildCardType[] Types { get; set; }

        public override string ToString()
        {
            return Caption ?? base.ToString();
        }
    }

    public enum WildCardType
    {
        General = 0,
        PasswordCreate = 1,
        PasswordReset = 2,
        Report = 3,
        Notification = 4,
        EmailConfirmation = 5,
        TwoFactor = 6,
        Workflow = 7
        
    }

}